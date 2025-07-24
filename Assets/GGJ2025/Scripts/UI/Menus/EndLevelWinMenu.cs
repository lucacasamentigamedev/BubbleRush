using UnityEngine;
using UnityEngine.UI;

public class EndLevelWinMenu : BaseUI {

    [SerializeField]
    private Image[] stars;

    private void OnEnable() {
        LevelManager.Get().Level = LevelManager.Get().Level + 1;
        SaveSystem.SaveFile(LevelManager.Get().Level, AudioManager.GetAllRawVolumes());
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
    }


}