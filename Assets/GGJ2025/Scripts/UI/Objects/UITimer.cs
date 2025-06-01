using System;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    [SerializeField]
    private RectTransform bar;
    [SerializeField]
    private TextMeshProUGUI text;
    
    private Vector2 scale;
    private float maxTime;

    void Start()
    {
        Debug.Log("Timer Activate");
        bar.localScale = Vector3.one;
        scale = bar.localScale;

        maxTime = LevelManager.Get().ActiveEntryData.timer_for_level;

        LevelManager.Get().OnUpdateTimer += OnUpdate;
    }


    void OnUpdate(float timer)
    {        
        if (!gameObject.activeInHierarchy) return;
        text.text = string.Format(timer.ToString("00"));
        bar.localScale = new Vector2(timer / maxTime, scale.y);
    }
}
