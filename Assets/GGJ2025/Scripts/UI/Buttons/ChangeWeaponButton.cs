using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeaponButton : MonoBehaviour
{
    private EWeaponType type;
    protected Button button;
    private Image buttonImage;
    public Action OnButtonClick;
    

    public EWeaponType WeaponType { get { return type; } set { type = value; } }

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        button.onClick.AddListener(OnClick);
    }

    protected void OnClick()
    {
        GlobalEventSystem.CastEvent(EventName.ChangeWeaponWithType, EventArgsFactory.ChangeWeaponWithTypeFactory(type));
        OnButtonClick?.Invoke();
    }

    public void SetSprite(Sprite sprite)
    {
        buttonImage.sprite = sprite;
    }
}