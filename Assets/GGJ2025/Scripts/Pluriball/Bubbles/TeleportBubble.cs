
using System;
using UnityEngine;

public class TeleportBubble : Bubble
{
    [SerializeField]
    private int minTimeToDisappeared;
    [SerializeField]
    private int maxTimeToDisappeared;

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
