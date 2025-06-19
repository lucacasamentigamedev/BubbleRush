using UnityEngine;

public class OptionsMenu : BaseUI {
    private void OnEnable() {
        AudioManager.PlayOneShotSound("MenuOpen");
        Debug.Log("OptionsMenu - OnEnable called");
    }

    private void OnDisable() {
        AudioManager.PlayOneShotSound("MenuClose");
    }
}