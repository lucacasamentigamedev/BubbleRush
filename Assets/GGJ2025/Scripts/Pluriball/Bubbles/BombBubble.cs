using System;
using UnityEngine;

public class BombBubble : Bubble
{
    [SerializeField]
    private float maxTimeForDisinnescation;
    [SerializeField]
    private float wrongWeaponTimeReduction;
    [SerializeField]
    private float noHitExplosionTimeReduction;
    [SerializeField]
    private EWeaponType toolForNeutralization;

    private float currentTimeForDisinnescation;
    private float timeToSubtract;
    public Action<float> OnExplode;

    override protected void InternalOnAwake()
    {
        bubbleType = EBubbleType.Bomb;
        
        timeToSubtract = Time.time;
        currentTimeForDisinnescation = maxTimeForDisinnescation;
        base.InternalOnAwake();
    }

    void Update()
    {
        if (!isAlive) return;
        ReduceInnerTimer(Time.time - timeToSubtract);
        timeToSubtract = Time.time;
    }
    private void ReduceInnerTimer(float time)
    {
        currentTimeForDisinnescation -= time;
        if (currentTimeForDisinnescation <= 0)
        {
            InternalOnHit(1, EWeaponType.LAST);   //se il timer finisce, la bomb prende danno. nella logica del danno c'è la discriminazione dell'arma
        }
    }


    public override void InternalOnHit(int damage, EWeaponType weaponType)
    {
        base.InternalOnHit(damage, weaponType);
        if (weaponType == toolForNeutralization)

            return;
        if (weaponType == EWeaponType.LAST)     //esplode per i fatti suoi
        {
            OnExplode?.Invoke(noHitExplosionTimeReduction);
        }
        else     //esplode perchè cliccato con l'arma sbagliata
        {
            OnExplode?.Invoke(wrongWeaponTimeReduction);
        }
           
    }
    
    
}
