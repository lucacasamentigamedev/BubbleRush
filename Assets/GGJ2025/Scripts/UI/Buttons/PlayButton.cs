public class PlayButton : BRButton {
    protected override void OnClick() {
        base.OnClick();
        LevelManager.Get().StartGame();
        AudioManager.PlayBackgroundMusic("GameplayMusic");
        UIController.OpenMenu(EUIType.GameplayMenu);
    }
}