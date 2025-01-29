public class DeleteSaveButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        LevelManager.Get().Level = SaveSystem.RemoveFile();
    }
}