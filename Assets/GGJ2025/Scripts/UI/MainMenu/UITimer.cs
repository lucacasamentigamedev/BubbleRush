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
    [SerializeField]
    private RectTransform background;

    private Vector2 defaultBarSizeDelta;
    private float currentTime;
    private float timeToCheck;

    public Action onTimerEnd;

    public void InitTimer(float maxTime, bool isActive)
    {
        gameObject.SetActive(true);
        defaultBarSizeDelta = bar.sizeDelta;
        currentTime = maxTime;
        timeToCheck = Time.time;
        Debug.Log($"MaxTime: {maxTime} currentTime : {currentTime} timeToCheck: {timeToCheck} Time.time: {Time.time}");

        if (!isActive)
        {
            gameObject.SetActive(false);
        }
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
        bar.sizeDelta = new Vector2(defaultBarSizeDelta.x * (currentTime / maxTime), bar.sizeDelta.y);
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
        InitTimer(maxTime, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;        
        ReduceTimer(Time.time - timeToCheck);
        timeToCheck = Time.time;
    }
}
