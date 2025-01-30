using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu : BaseUI
{
    [SerializeField]
    private Sprite[] tutorialsImage;
    [SerializeField]
    private Image backgroundRef;

    public void prepareTutorial(EUITutorialType UITutorialType) {
        backgroundRef.sprite = tutorialsImage[(int)UITutorialType];
    }
}
