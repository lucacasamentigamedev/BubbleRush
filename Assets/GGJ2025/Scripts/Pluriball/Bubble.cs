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
    #endregion

    #region Protected Members
    protected EBubbleType bubbleType;
    protected bool isAlive;
    protected int currentClickRemains;
    protected SpriteRenderer spriteRenderer;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        OnDestroy += InternalOnDestroy;
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

    

    private void InternalOnDestroy()
    {
        spriteRenderer.sprite = poppedSprite;
        isAlive = false;
    }

    protected virtual void InternalOnHit(EWeaponType weapon)
    {
        if (!isAlive) return;
    }
}
