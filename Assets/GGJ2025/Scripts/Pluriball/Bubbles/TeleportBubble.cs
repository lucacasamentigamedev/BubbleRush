
using System;
using UnityEngine;

public class TeleportBubble : Bubble
{
    [SerializeField]
    private int minTimeToDisappeared;
    [SerializeField]
    private int maxTimeToDisappeared;

    public Action<TeleportBubble> TeleportEvent;

    private float timeToDisappeared;
    private float currentTimeAppeared;
    private float timeToSubtract;
    override protected void InternalOnAwake()
    {
        bubbleType = EBubbleType.Teleport;
        base.InternalOnAwake();
        timeToSubtract = Time.time;
        timeToDisappeared = (float)UnityEngine.Random.Range(minTimeToDisappeared, maxTimeToDisappeared);
        currentTimeAppeared = timeToDisappeared;
    }

    void Update()
    {
        if (!isAlive) return;
        ReduceInnerTimer(Time.time - timeToSubtract);
        timeToSubtract = Time.time;
    }
    private void ReduceInnerTimer(float time)
    {
        currentTimeAppeared -= time;
        if (currentTimeAppeared <= 0)
        {
            TeleportEvent?.Invoke(this);
            currentTimeAppeared = timeToDisappeared;
        }
    }

}
