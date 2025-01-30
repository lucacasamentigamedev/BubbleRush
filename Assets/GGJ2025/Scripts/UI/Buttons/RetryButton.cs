public class RetryButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        LevelManager.Get().RetryLevel();
        AudioManager.ResumeBackgroundMusic();
        UIController.OpenMenu(EUIType.GameplayHUD);
    }
}