using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    #region Serializable
    [SerializeField]
    protected Sprite unpoppedSprite;
    [SerializeField] 
    protected Sprite poppedSprite;
    [SerializeField] 
    protected Sprite[] arraySprite;
    [SerializeField] 
    protected EWeaponType[] requiredWeapon;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected float shakeMagnitude;
    [SerializeField]
    protected float shakeDuration;
    #endregion

    public bool IsAlive { get { return isAlive; } }
    public EBubbleType BubbleType { get { return bubbleType; } }

    #region Protected Members
    protected EBubbleType bubbleType;
    protected bool isAlive;
    protected int currentClickRemains;
    protected int clickToPop;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        InternalOnAwake();
    }

    #endregion

    protected virtual void InternalOnAwake()
    {
        isAlive = true;
        spriteRenderer.sprite = unpoppedSprite;
    }

    #region Public Members
    public Action OnDestroy;
    public Action<float, float> OnCamerShake;

    public Vector2 GetSize()
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector3 scale = transform.localScale;

        return new Vector2 (scale.x * spriteSize.x, scale.y * spriteSize.y) ;
    }
    #endregion

    public void ResetBubble(int life) {
        InternalOnAwake();
        clickToPop= life;
        currentClickRemains = life;
    }
    public void ResetBubble()
    {
        InternalOnAwake();
        currentClickRemains = clickToPop;
    }



    private void InternalOnDestroy()
    {
        Debug.Log("Esplodo");
        //AudioManager.PlayOneShotSound("Test");
        switch(BubbleType) {
            case EBubbleType.Normal:
                AudioManager.PlayOneShotSound("BubblePop", new FMODParameter[] {
                    new FMODParameter("BUBBLE_POP_TYPE", 0.0f)
                });
                break;
            case EBubbleType.Rock:
                AudioManager.PlayOneShotSound("BubblePop", new FMODParameter[] {
                    new FMODParameter("BUBBLE_POP_TYPE", 1.0f)
                });
                break;
        }
        spriteRenderer.sprite = poppedSprite;
        isAlive = false;
        OnCamerShake?.Invoke(shakeMagnitude, shakeDuration);
        OnDestroy?.Invoke();
    }

    public virtual void InternalOnHit(int damage, EWeaponType weaponType) {
        if (!isAlive) {
            Debug.Log("Sono morta, bona");
            AudioManager.PlayOneShotSound("BubbleTool", new FMODParameter[] {
                    new FMODParameter("BUBBLE_TOOL", 0.0f)
            });
            return;
        }
        if (requiredWeapon.Length == 0) {
            Debug.Log("tutto mi può colpire sono tipo Dende");
            TakeDamage(damage);
            return;
        }
        foreach (EWeaponType weapon in requiredWeapon) {
            if (weapon == weaponType) {
                TakeDamage(damage);
                return;
            }
        }
        Debug.Log("Non hai l'arma adatta per me");
    }

    private void TakeDamage(int damage) {
        currentClickRemains -= damage;
        if (currentClickRemains <= 0) {
            InternalOnDestroy();
        }
        Debug.Log("Sono anora viva con vita " + currentClickRemains);
    }
}
