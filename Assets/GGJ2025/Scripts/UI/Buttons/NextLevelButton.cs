public class NextLevelButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        AudioManager.ResumeBackgroundMusic();
        UIController.OpenMenu(EUIType.GameplayMenu);
        LevelManager.Get().StartGame();
    }
}