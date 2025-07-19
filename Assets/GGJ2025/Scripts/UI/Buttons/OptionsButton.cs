public class OptionsButton : BRButton
{
    protected override void OnClick()
    {
        if (UIController.isPrevented) return;
        base.OnClick();
        UIController.OpenMenu(EUIType.OptionsMenu);
    }
}

