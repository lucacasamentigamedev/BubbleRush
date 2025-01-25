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
    [SerializeField]
    protected float shakeMagnitudeOnExplode;
    [SerializeField]
    protected float shakeDurationOnExplode;

    private float currentTimeForDisinnescation;
    private float timeToSubtract;
    public Action<float> OnExplode;
    private bool isExploded;

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
        if(!isAlive) return;
        if (weaponType != toolForNeutralization)
        {
            if (weaponType == EWeaponType.LAST)     //esplode per i fatti suoi
            {
                OnExplode?.Invoke(noHitExplosionTimeReduction);
            }
            else     //esplode perchè cliccato con l'arma sbagliata
            {
                OnExplode?.Invoke(wrongWeaponTimeReduction);
            }
            isExploded = true;
        }else
        {
            isExploded = false;
        }
        base.InternalOnHit(damage, weaponType);

    }

    public override void InternalOnDestroy()
    {
        Debug.Log("Esplodo");
        if (isExploded)
        {
            AudioManager.PlayOneShotSound("BubbleBombExplode", new FMODParameter[] {
                    new FMODParameter("BUBBLE_MALUS", 0.0f) });     //suono esplosione
            OnCamerShake?.Invoke(shakeMagnitudeOnExplode, shakeDurationOnExplode);
        }
        else
        {
            AudioManager.PlayOneShotSound("BubbleTool", new FMODParameter[] {
                    new FMODParameter("BUBBLE_TOOL", 5.0f)});     //suono disinnescato
            OnCamerShake?.Invoke(shakeMagnitude, shakeDuration);
        }        
        ChangeSprite(0);
        isAlive = false;        
        OnDestroy?.Invoke();
    }

}
