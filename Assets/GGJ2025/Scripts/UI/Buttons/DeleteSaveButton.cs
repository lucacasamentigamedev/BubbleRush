public class DeleteSaveButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        if(SaveSystem.RemoveFile() == 1)
        {
            LevelManager.Get().OnDeleteSaves();
        }
    }
}