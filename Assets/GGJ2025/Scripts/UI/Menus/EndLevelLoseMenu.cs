using TMPro;
using UnityEngine;

public class EndLevelLoseMenu : BaseUI {

    [SerializeField]
    private TextMeshProUGUI levelText;

    private void OnEnable() {
        AudioManager.PauseBackgroundMusic();
        AudioManager.PlayOneShotSound("WinLose", new FMODParameter[] {
                new FMODParameter("WIN_LOSE", 1.0f)
        });
        levelText.text = "Level " + (LevelManager.Get().CurrentLevel).ToString();
    }
}