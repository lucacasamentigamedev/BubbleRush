
public class ChangeWeaponLeftButton : BRButton
{
    protected override void OnClick()
    {
        if (UIController.isPrevented) return;
        base.OnClick();
        GlobalEventSystem.CastEvent(EventName.ChangeWeapon, EventArgsFactory.ChangeWeaponFactory(1));          
    }
}