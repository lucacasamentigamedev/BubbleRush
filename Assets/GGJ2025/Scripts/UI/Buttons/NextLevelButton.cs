public class NextLevelButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        AudioManager.ResumeBackgroundMusic();
        LevelManager.Get().StartLevel(LevelManager.Get().Level);
    }
}