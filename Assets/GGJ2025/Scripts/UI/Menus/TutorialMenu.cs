using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu : BaseUI
{
    [SerializeField]
    private Sprite[] tutorialsImage;
    private Image background;

    public void prepareTutorial(EUITutorialType UITutorialType) {
        background = gameObject.GetComponent<Image>();
        background.sprite = tutorialsImage[(int)UITutorialType];
    }
}
