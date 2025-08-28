public class EndlessModeButton : BRButton
{
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();

        LevelManager.Get().ResetEndlessMode();
        LevelManager.Get().StartEndlessMode();
    }
}
