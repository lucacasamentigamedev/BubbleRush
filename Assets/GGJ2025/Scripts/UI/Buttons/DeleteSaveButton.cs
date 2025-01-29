public class DeleteSaveButton : BRButton {
    protected override void OnClick() {
        base.OnClick();
        LevelManager.Get().Level = SaveSystem.RemoveFile();
    }
}