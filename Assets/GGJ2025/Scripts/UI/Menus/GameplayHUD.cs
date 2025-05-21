using TMPro;
using UnityEngine;

public class GameplayHUD: BaseUI {

    [SerializeField]
    private TextMeshProUGUI textMeshProText;

    public void Awake()
    {
        Canvas canvas = gameObject.GetComponent<Canvas>();
        if(canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {

            canvas.worldCamera = FindAnyObjectByType<Camera>();
        }
    }
    public void ChangeLevelLabel() {
        textMeshProText.text = "Level " + LevelManager.Get().Level;
    }
}