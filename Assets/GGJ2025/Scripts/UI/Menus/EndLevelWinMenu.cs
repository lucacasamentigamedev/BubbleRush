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

        int starNumbers = 3;
        /*
        float[] startsThreshold = LevelManager.Get().ActiveEntryData.stars_for_level;
        for (int i = 0; i < startsThreshold.Length; i++)
        {
            if (startsThreshold[i] <= timer.GetTimerPercent())
            {
                starNumbers++;
            }
            else
            {
                break;
            }
        }
        */

        for (int i = 0; i < starNumbers; i++) {

            stars[i].gameObject.SetActive(true);
        }
    }


}