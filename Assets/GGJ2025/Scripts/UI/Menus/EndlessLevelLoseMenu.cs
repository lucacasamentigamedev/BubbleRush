using UnityEngine;
using TMPro;

public class EndlessLevelLoseMenu : BaseUI
{
    [SerializeField]
    private TextMeshProUGUI levelReachedText;

    private void OnEnable()
    {
        levelReachedText.text = "Level Reached: " + LevelManager.Get().EndlessReachedLevel;
        AudioManager.PauseBackgroundMusic();
        AudioManager.PlayOneShotSound("WinLose", new FMODParameter[] {
                new FMODParameter("WIN_LOSE", 1.0f)
        });
    }
}
