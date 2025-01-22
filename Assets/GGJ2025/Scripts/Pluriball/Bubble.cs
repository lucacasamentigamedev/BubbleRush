using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    #region Serializable
    [SerializeField]
    protected Vector2 clickRangeToPop;
    [SerializeField]
    protected Sprite unpoppedSprite;
    [SerializeField] 
    protected Sprite poppedSprite;
    [SerializeField] 
    protected EWeaponType[] requiredWeapon;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region Protected Members
    protected EBubbleType bubbleType;
    protected bool isAlive;
    protected int currentClickRemains;
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
    public Vector2 GetSize()
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector3 scale = transform.localScale;

        return new Vector2 (scale.x * spriteSize.x, scale.y * spriteSize.y) ;
    }
    #endregion

    public void ResetBubble(int life) {
        InternalOnAwake ();
        currentClickRemains = life;
    }

    private void InternalOnDestroy()
    {
        Debug.Log("Esplodo");
        AudioManager.PlayOneShotSound("Test");
        spriteRenderer.sprite = poppedSprite;
        isAlive = false;
        OnDestroy?.Invoke();
    }

    public virtual void InternalOnHit(int damage, EWeaponType weaponType) {
        if (!isAlive) {
            Debug.Log("Sono morta, bona");
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
