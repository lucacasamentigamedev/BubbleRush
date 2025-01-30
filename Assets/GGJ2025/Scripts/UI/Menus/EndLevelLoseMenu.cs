public class EndLevelLoseMenu : BaseUI {
    private void OnEnable() {
        AudioManager.PauseBackgroundMusic();
        AudioManager.PlayOneShotSound("WinLose", new FMODParameter[] {
                new FMODParameter("WIN_LOSE", 1.0f)
        });
    }
}