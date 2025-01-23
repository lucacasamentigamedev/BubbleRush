using System;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
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
        defaultBarSizeDelta = bar.sizeDelta;
        currentTime = maxTime;
        timeToCheck = Time.time;
        Debug.Log($"MaxTime: {maxTime} currentTime : {currentTime} timeToCheck: {timeToCheck} Time.time: {Time.time}");

        if (!isActive)
        {
            gameObject.SetActive(false);
        }
    }

    private void ResizeTimer()
    {
        bar.sizeDelta = new Vector2(defaultBarSizeDelta.x * (currentTime / maxTime), bar.sizeDelta.y);
    }
    // Start is called before the first frame update
    void Start()
    {
        InitTimer(maxTime, false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"MaxTime: {maxTime} currentTime : {currentTime} timeToCheck: {timeToCheck} Time.time: {Time.time}");
        currentTime -= Time.time - timeToCheck;
        timeToCheck = Time.time;
        ResizeTimer();

        if (maxTime - currentTime <= 0)
        {
            Debug.Log("Hai Perso");
            AudioManager.PlayOneShotSound("WinLose", new FMODParameter[] {
                    new FMODParameter("WIN_LOSE", 1.0f)
            });
            onTimerEnd?.Invoke();
        }

    }
}
