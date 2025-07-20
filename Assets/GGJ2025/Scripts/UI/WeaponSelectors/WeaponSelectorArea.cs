using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponSelectorArea : MonoBehaviour
{
    [SerializeField]
    private WeaponSelector mainWeapon;
    [SerializeField]
    private WeaponSelector weaponBackward;
    [SerializeField]
    private WeaponSelector weaponForward;
    [SerializeField]
    private WeaponSelector weaponBackwHide;
    [SerializeField]
    private WeaponSelector weaponForwdHide;

    private WeaponSelector[] weapons;
    void Start()
    {
        mainWeapon.Position = E_ICON_POSITION.MIDDLE;
        weaponBackward.Position = E_ICON_POSITION.UP;
        weaponBackwHide.Position = E_ICON_POSITION.HIDE_UP;
        weaponForward.Position = E_ICON_POSITION.DOWN;
        weaponForwdHide.Position = E_ICON_POSITION.HIDE_DOWN;
        weapons = new WeaponSelector[] { mainWeapon, weaponBackward, weaponForward, weaponBackwHide, weaponForwdHide };

        InputManager.Player.ChangeWeaponForward.performed += onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed += onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed += onChangeWeaponWheel;
    }

    private void OnDestroy()
    {
        InputManager.Player.ChangeWeaponForward.performed -= onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed -= onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed -= onChangeWeaponWheel;
    }
    private void onChangeWeaponBackward(InputAction.CallbackContext context)
    {
        ChangeWeapon(-1);
    }

    private void onChangeWeaponForward(InputAction.CallbackContext context)
    {
        ChangeWeapon(1);        
    }

    private void onChangeWeaponWheel(InputAction.CallbackContext context)
    {
        ChangeWeapon(context.ReadValue<Vector2>().y > 0 ? 1 : -1);
    }

    private void ChangeWeapon(int forward)
    {
        if (!CanChangeWeapon()) return;
        if (forward> 0)
        {
            foreach (var weapon in weapons)
            {
                weapon.MoveDown();
                weapon.Position = weapon.Position == E_ICON_POSITION.HIDE_DOWN ? E_ICON_POSITION.HIDE_UP : weapon.Position + 1;
            }
        }
        else
        {
            foreach (var weapon in weapons)
            {
                weapon.MoveUp();
                weapon.Position = weapon.Position == E_ICON_POSITION.HIDE_UP ? E_ICON_POSITION.HIDE_DOWN : weapon.Position - 1;
            }
        }
    }



    

    private bool CanChangeWeapon()
    {
        foreach (var weapon in weapons)
        {
            if(weapon.IsPrevented)
                return false;
        }
        return true;
        
    }
}
