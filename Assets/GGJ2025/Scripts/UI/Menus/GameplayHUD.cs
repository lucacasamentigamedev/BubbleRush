using System;
using TMPro;
using UnityEngine;

public class GameplayHUD: BaseUI {

    [SerializeField]
    private TextMeshProUGUI textMeshProText;
    [SerializeField]
    private GameObject timerUI;

    public void Awake()
    {        
        Canvas canvas = gameObject.GetComponent<Canvas>();
        if(canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {

            canvas.worldCamera = FindAnyObjectByType<Camera>();
        }
        LevelManager.Get().OnStartLevel += OnStartLevel;
    }

    private void OnStartLevel()
    {
        if(LevelManager.Get().ActiveEntryData.is_Timer_Activate)
        {
            timerUI.SetActive(true);
        }
        else
        {
            timerUI.SetActive(false);
        }
    }

    public void ChangeLevelLabel() {
        textMeshProText.text = "Level " + LevelManager.Get().Level;
    }
}