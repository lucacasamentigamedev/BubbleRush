public class BackToHubLevelButton : BRButton
{
    protected override void OnClick()
    {
        if (UIController.isPrevented) return;
        base.OnClick();
        AudioManager.ResumeBackgroundMusic();
        UIController.OpenMenu(EUIType.LevelHubMenu);
    }
}

