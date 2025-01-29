using UnityEngine;

public class QuitButton : BRButton {
    protected override void OnClick() {
        base.OnClick();
        Application.Quit();
    }
}