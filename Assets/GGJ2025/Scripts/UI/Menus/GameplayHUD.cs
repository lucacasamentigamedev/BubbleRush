using TMPro;
using UnityEngine;

public class GameplayHUD: BaseUI {

    [SerializeField]
    private TextMeshProUGUI textMeshProText;

    public void ChangeLevelLabel() {
        textMeshProText.text = "Level " + LevelManager.Get().Level;
    }
}