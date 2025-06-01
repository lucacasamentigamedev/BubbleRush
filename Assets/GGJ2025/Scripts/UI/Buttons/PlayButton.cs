public class PlayButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        LevelManager.Get().StartLevel(LevelManager.Get().Level);
        AudioManager.PlayBackgroundMusic("GameplayMusic");
    }
}