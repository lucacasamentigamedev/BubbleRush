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
    private float currentTime;
    private float timeToCheck;
    private float maxTime;
    private bool isActive;

    public Action onTimerEnd;

    private bool soundBeepExecuted = false;

    public void InitTimer(float maxTime, bool isActive)
    {
        soundBeepExecuted = false;
        gameObject.SetActive(isActive); 
        this.isActive = isActive;
        bar.localScale = Vector3.one;
        this.maxTime = maxTime;
        currentTime = maxTime;
        timeToCheck = Time.time;
        Debug.Log($"INIT → MaxTime: {maxTime} currentTime : {currentTime} timeToCheck: {timeToCheck} Time.time: {Time.time} Scale: {scale} LocalScale: {bar.localScale}");

    }

    public float GetTimerPercent()
    {
        return currentTime / maxTime;
    }

    public void ReduceTimer(float time)
    {
        currentTime -= time;
        InternalResizeTimer();
    }
    private void InternalResizeTimer()
    {
        text.text = string.Format(currentTime.ToString("00"));
        bar.localScale = new Vector2(currentTime / maxTime, scale.y);
        InternalOnLose();
    }

    private void InternalOnLose()
    {
        if (currentTime <= 0)
        {
            onTimerEnd?.Invoke();
            isActive = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        scale = bar.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        int t = (int)(currentTime);
        if (((t <= 3 && t > 2) || (t <= 1 && t > 0)) && soundBeepExecuted) {
            AudioManager.PlayOneShotSound("TimeEndBeep");
            soundBeepExecuted = false;
        } else if (((t <= 4 && t > 3) || (t <= 2 && t > 1) || t == 0) && !soundBeepExecuted) {
            AudioManager.PlayOneShotSound("TimeEndBeep");
            soundBeepExecuted = true;
        }
        ReduceTimer(Time.time - timeToCheck);
        timeToCheck = Time.time;
    }
}
