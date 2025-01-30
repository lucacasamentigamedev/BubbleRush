using UnityEngine;
using UnityEngine.UI;

public class EndLevelWinMenu : BaseUI {

    [SerializeField]
    private Image[] stars;

    private void OnEnable() {
        AudioManager.PauseBackgroundMusic();
        AudioManager.PlayOneShotSound("WinLose", new FMODParameter[] {
                new FMODParameter("WIN_LOSE", 0.0f)
        });
        hideAllStars();
        showRightStars();
    }

    private void hideAllStars() {
        foreach (Image star in stars) {
            star.gameObject.SetActive(false);
        }
    }

    private void showRightStars() {
        for (int i = 0; i < LevelManager.Get().CurrentLevelStarsObtained; i++) {
            stars[i].gameObject.SetActive(true);
        }
    }
}