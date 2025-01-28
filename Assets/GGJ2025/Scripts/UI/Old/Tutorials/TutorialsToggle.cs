using UnityEngine;
using UnityEngine.UI;

public class TutorialsToggle : MonoBehaviour
{

    [SerializeField]
    private Sprite[] tutorialsImage;
    private bool firstTime;
    [SerializeField]
    private Image imageToChange;

    void Start()
    {
        LevelManager.Get().OnStart += OnStartNewLevel;
        if (!firstTime) {
            OnStartNewLevel();
            firstTime = true;
        }
        gameObject.SetActive(false);
    }

    private void OnStartNewLevel() {
        switch (LevelManager.Get().Level)
        {
            case 1:
                OpenTutorialOnScreen(0);
                break;
            case 2:
                OpenTutorialOnScreen(1);
                break;
            case 3:
                OpenTutorialOnScreen(2);
                break;
            case 5:
                OpenTutorialOnScreen(3);
                break;
            case 7:
                OpenTutorialOnScreen(4);
                break;
            case 10:
                OpenTutorialOnScreen(5);
                break;
            default:
                ReactivateTime();
                break;
        }
    }

    private void OpenTutorialOnScreen(uint index) {
        AudioManager.PlayOneShotSound("MenuOpen");
        //AudioManager.PauseBackgroundMusic();
        Time.timeScale = 0f;
        imageToChange.sprite = tutorialsImage[index];
        gameObject.SetActive(true);
    }

    public void ReactivateTime() {
        //AudioManager.ResumeBackgroundMusic();
        AudioManager.PlayOneShotSound("MenuClose");
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
