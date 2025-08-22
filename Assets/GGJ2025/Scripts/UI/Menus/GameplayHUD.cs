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
        LevelManager.Get().OnStartEndlessLevel += OnStartEndlessLevel;
    }

    private void OnStartLevel(uint levelIndex)
    {
        if(LevelManager.Get().ActiveEntryData.is_Timer_Activate)
        {
            timerUI.SetActive(true);
        }
        else
        {
            timerUI.SetActive(false);
        }
        textMeshProText.text = "Level " + levelIndex;
    }
    private void OnStartEndlessLevel()
    {
        if (LevelManager.Get().ActiveEntryData.is_Timer_Activate)
        {
            timerUI.SetActive(true);
        }
        else
        {
            timerUI.SetActive(false);
        }
        textMeshProText.text = "Score ";
    }
}