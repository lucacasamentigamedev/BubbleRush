
using System;
using UnityEngine;

public class TeleportBubble : Bubble
{
    private float minTimeToDisappeared = 2.0f;      //Default Value
    private float maxTimeToDisappeared = 5.0f;

    public Action<TeleportBubble> TeleportEvent;
    public Action<TeleportBubble> WrongWeapTeleportEvent;

    private float timeToDisappeared;
    private float currentTimeAppeared;
    private float timeToSubtract;
    override protected void InternalOnAwake()
    {
        bubbleType = EBubbleType.Teleport;
        base.InternalOnAwake();
        timeToSubtract = Time.time;
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
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timeToDisappeared = UnityEngine.Random.Range(minTimeToDisappeared, maxTimeToDisappeared);
        currentTimeAppeared = timeToDisappeared;
    }


    public override void SetTimerDisappeared( float minTimeToDisappeared, float maxTimeToDisappeared )
    {
        if (minTimeToDisappeared * maxTimeToDisappeared <= 0.0f) return;
        this.minTimeToDisappeared = minTimeToDisappeared;
        this.maxTimeToDisappeared = maxTimeToDisappeared;
        ResetTimer();
    }



    public override void InternalOnHit(int damage, EWeaponType weaponType)
    {
        //Se sono morto
        if (!isAlive) {            
            AudioManager.PlayOneShotSound("BubbleTool", new FMODParameter[] {
                    new FMODParameter("BUBBLE_TOOL", 0.0f)
            });
            return;
        }

        //quando mi colpiscono con l'arma corretta faccio il doppio del danno (default 2)
        foreach (EWeaponType weapon in requiredWeapon)
        {
            if (weapon == weaponType)
            {
                TakeDamage(damage);
                return;
            }
            WrongWeapTeleportEvent?.Invoke(this);
        }
    }

}
