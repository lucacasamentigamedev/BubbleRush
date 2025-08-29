using UnityEngine;

public class ChangeWeaponButton : BRButton
{
    [SerializeField]
    private EWeaponType type;
    protected override void OnClick()
    {
        if (UIController.isPrevented) return;
        base.OnClick();
        GlobalEventSystem.CastEvent(EventName.ChangeWeaponWithType, EventArgsFactory.ChangeWeaponWithTypeFactory(type));

    }
}