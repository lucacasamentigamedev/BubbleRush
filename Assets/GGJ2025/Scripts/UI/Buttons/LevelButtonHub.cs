using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUBLevelButton : BRButton
{
    [SerializeField]
    private uint levelIndex;

    private TextMeshPro btnText;

    void Start()
    {
        if(LevelManager.Get().Level<levelIndex)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
        btnText = GetComponentInChildren<TextMeshPro>();
        if(btnText != null )
        {
            btnText.text = "Level " + levelIndex.ToString();
        }
    }

    protected override void OnClick()
    {
        if (UIController.isPrevented) return;
        base.OnClick();
        LevelManager.Get().StartLevel(levelIndex);
        AudioManager.PlayBackgroundMusic("GameplayMusic");
    }

}
