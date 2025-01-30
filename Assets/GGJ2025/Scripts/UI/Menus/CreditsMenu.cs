public class CreditsMenu : BaseUI {
    private void OnEnable() {
        AudioManager.PlayOneShotSound("MenuOpen");
    }

    private void OnDisable() {
        AudioManager.PlayOneShotSound("MenuClose");
    }
}