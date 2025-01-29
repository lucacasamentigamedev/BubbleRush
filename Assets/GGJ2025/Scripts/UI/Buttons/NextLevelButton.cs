public class NextLevelButton : BRButton {
    protected override void OnClick() {
        base.OnClick();
        AudioManager.ResumeBackgroundMusic();
        UIController.OpenMenu(EUIType.GameplayMenu);
        LevelManager.Get().StartGame();
    }
}