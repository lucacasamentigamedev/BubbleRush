using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameplayHUD: BaseUI {

    [SerializeField]
    private TextMeshProUGUI textMeshProText;
    [SerializeField]
    private GameObject timerUI;
    [SerializeField]
    private Image PCTutorialTip;

    public void Awake()
    {        
        Canvas canvas = gameObject.GetComponent<Canvas>();
        if(canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {

            canvas.worldCamera = FindAnyObjectByType<Camera>();
        }
        LevelManager.Get().OnStartLevel += OnStartLevel;
        LevelManager.Get().OnStartEndlessLevel += OnStartEndlessLevel;

#if UNITY_ANDROID || UNITY_IOS
        PCTutorialTip.gameObject.SetActive(false);
#endif
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
        textMeshProText.text = "Score " + LevelManager.Get().EndlessReachedLevel;
    }
}