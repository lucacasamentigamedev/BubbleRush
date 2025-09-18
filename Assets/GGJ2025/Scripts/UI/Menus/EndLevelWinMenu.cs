using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelWinMenu : BaseUI {

    [SerializeField]
    private Image[] stars;
    [SerializeField]
    private TextMeshProUGUI levelText;

    private void OnEnable() {
        if (LevelManager.Get().CurrentLevel == LevelManager.Get().ReachedLevel)
        {
            //completed last unlocked level , so we increase the reached level
            LevelManager.Get().ReachedLevel += 1;
        }
        //we set the current level to the next one
        levelText.text = (LevelManager.Get().CurrentLevel).ToString();
        LevelManager.Get().CurrentLevel += 1;
        AudioManager.PauseBackgroundMusic();
        AudioManager.PlayOneShotSound("WinLose", new FMODParameter[] {
            new FMODParameter("WIN_LOSE", 0.0f)
        });
        HideAllStars();
    }

    private void HideAllStars() {
        foreach (Image star in stars) {
            star.gameObject.SetActive(false);
        }
    }

    public void ShowRightStars(int starNumbers) 
    {
        for (int i = 0; i < starNumbers; i++) {
            stars[i].gameObject.SetActive(true);
        }
        SaveSystem.SaveFile(
            LevelManager.Get().ReachedLevel,
            AudioManager.GetAllRawVolumes(),
            LevelManager.Get().LevelScores,
            LevelManager.Get().EndlessReachedLevel,
            LevelManager.Get().EndlessRecordLevel
        );
    }
}