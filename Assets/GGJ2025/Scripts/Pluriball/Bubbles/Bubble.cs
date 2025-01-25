using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    #region Serializable
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

        currentClickRemains = clickToPop;
        isAlive = true;
        ChangeSprite(clickToPop);
        //spriteRenderer.sprite = unpoppedSprite;
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
        clickToPop= life;
        InternalOnAwake();
    }
    public void ResetBubble()
    {
        InternalOnAwake();
    }

    private void ChangeSprite(int indexLife)
    {
        spriteRenderer.sprite = arraySprite[indexLife];
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
            case EBubbleType.Bomb:
                AudioManager.PlayOneShotSound("BubbleBombExplode", new FMODParameter[] {
                    new FMODParameter("BUBBLE_MALUS", 0.0f)
                });
                break;
        }
        ChangeSprite(0);
        isAlive = false;
        OnCamerShake?.Invoke(shakeMagnitude, shakeDuration);
        OnDestroy?.Invoke();
    }

    public virtual void InternalOnHit(int damage, EWeaponType weaponType) {

        //Se sono morto
        if (!isAlive) {            
            AudioManager.PlayOneShotSound("BubbleTool", new FMODParameter[] {
                    new FMODParameter("BUBBLE_TOOL", 0.0f)
            });
            return;
        }
        //quando mi colpiscono con un arma sbagliata
        if (requiredWeapon.Length == 0) {
            Debug.Log("tutto mi può colpire sono tipo Dende");
            TakeDamage(damage);
            return;
        }

        //quando mi colpiscono con l'arma corretta
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
        ChangeSprite(currentClickRemains);
        if (currentClickRemains <= 0) {
            InternalOnDestroy();
        } else {
            AudioManager.PlayOneShotSound("BubbleSimpleCLick");
        }
    }
}
