public class NextLevelButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        AudioManager.ResumeBackgroundMusic();
        UIController.OpenMenu(EUIType.GameplayHUD);
        LevelManager.Get().StartLevel(LevelManager.Get().Level + 1);
    }
}