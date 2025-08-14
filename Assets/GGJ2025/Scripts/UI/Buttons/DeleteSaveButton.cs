public class DeleteSaveButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        if(SaveSystem.DeleteSave() == 1)
        {
            LevelManager.Get().OnDeleteSaves();
        }
    }
}