using UnityEngine;

public class HUBLevelButton : BRButton
{
    [SerializeField]
    private uint levelIndex;



    void Start()
    {
        if(LevelManager.Get().Level>levelIndex)
        {
            gameObject.SetActive(false);
        }
    }

    protected override void OnClick()
    {
        if (UIController.isPrevented) return;
        base.OnClick();
        LevelManager.Get().StartLevel(levelIndex);
        AudioManager.PlayBackgroundMusic("GameplayMusic");
        UIController.OpenMenu(EUIType.GameplayHUD);
    }

}
