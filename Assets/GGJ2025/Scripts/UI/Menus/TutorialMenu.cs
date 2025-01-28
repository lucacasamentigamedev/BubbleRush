using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu : BaseUI
{
    [SerializeField]
    private Sprite[] tutorialsImage;
    private bool firstTime;
    [SerializeField]
    private Image imageToChange;

    public void prepareTutorial(EUITutorialType UITutorialType) {
        imageToChange.sprite = tutorialsImage[(int)UITutorialType];
    }
}
