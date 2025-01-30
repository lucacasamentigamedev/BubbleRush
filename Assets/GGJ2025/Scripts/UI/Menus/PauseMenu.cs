public class PauseMenu : BaseUI {
    private void OnEnable() {
        AudioManager.PlayOneShotSound("MenuOpen");
    }

    private void OnDisable() {
        AudioManager.PlayOneShotSound("MenuClose");
    }
}