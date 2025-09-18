public class RetryFromEndLevelWinButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        LevelManager.Get().CurrentLevel -= 1;
        AudioManager.ResumeBackgroundMusic();
        LevelManager.Get().StartLevel(LevelManager.Get().CurrentLevel);
    }
}