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
        if (LevelManager.Get().EndlessMode)
            bar.gameObject.SetActive(false);
        else
        {
            bar.localScale = Vector3.one;
            scale = bar.localScale;
        }

        maxTime = LevelManager.Get().ActiveEntryData.timer_for_level;

        LevelManager.Get().OnUpdateTimer += OnUpdate;
    }


    void OnUpdate(float timer)
    {        
        if (!gameObject.activeInHierarchy) return;
        text.text = string.Format(timer.ToString("00"));
        
        if(bar.gameObject.activeInHierarchy)
            bar.localScale = new Vector2(timer / maxTime, scale.y);
    }
}
