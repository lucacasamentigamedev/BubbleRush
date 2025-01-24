using System;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    [SerializeField]
    private float maxTime;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private RectTransform bar;

    private Vector2 scale;
    private float currentTime;
    private float timeToCheck;

    public Action onTimerEnd;

    private bool soundBeepExecuted = false;

    public void InitTimer(float maxTime, bool isActive)
    {
        soundBeepExecuted = false;
        gameObject.SetActive(isActive); 
        this.isActive = isActive;
        bar.localScale = scale;
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
        bar.localScale = new Vector2(currentTime / maxTime, scale.y);
        Debug.Log($"UPDATE → LocalScale: {bar.localScale} MaxTime: {maxTime} currentTime : {currentTime}");
        InternalOnLose();
    }

    private void InternalOnLose()
    {
        if (currentTime <= 0)
        {
            AudioManager.PlayOneShotSound("WinLose", new FMODParameter[] {
                    new FMODParameter("WIN_LOSE", 1.0f)
            });
            onTimerEnd?.Invoke();
            isActive = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        scale = bar.localScale;
        InitTimer(maxTime, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        int t = (int)(Time.time - timeToCheck);
        if (((t <= 5 && t > 4) || (t <= 3 && t > 2) || (t <= 1 && t > 0)) && !soundBeepExecuted) {
            AudioManager.PlayOneShotSound("TimeEndBeep");
            soundBeepExecuted = true;
        } else if (((t <= 4 && t > 3) || (t <= 2 && t > 1)) && soundBeepExecuted) {
            AudioManager.PlayOneShotSound("TimeEndBeep");
            soundBeepExecuted = false;
        }

        ReduceTimer(Time.time - timeToCheck);
        timeToCheck = Time.time;
    }
}
