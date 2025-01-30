public class PlayButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        LevelManager.Get().StartGame();
        AudioManager.PlayBackgroundMusic("GameplayMusic");
        UIController.OpenMenu(EUIType.GameplayHUD);
    }
}