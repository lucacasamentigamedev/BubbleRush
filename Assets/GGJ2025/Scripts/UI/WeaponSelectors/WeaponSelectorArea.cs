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
        Debug.Log("Backward selected");

        /*
        weaponBackward.Move(E_Move.MIDDLE);
        weaponBackward.Resize(E_Resize.MAXIMIZE);

        mainWeapon.Move(E_Move.DOWN);
        mainWeapon.Resize(E_Resize.REDUCE);
        
        weaponForward.Move(E_Move.HIDE_DOWN);
        weaponForward.Resize(E_Resize.MINIMIZE);
        */

        
        
        foreach (var weapon in weapons)
        {
            weapon.MoveDown();
            weapon.Position = weapon.Position == E_ICON_POSITION.HIDE_DOWN ? E_ICON_POSITION.HIDE_UP : weapon.Position + 1;
        }
    }

    private void onChangeWeaponForward(InputAction.CallbackContext context)
    {
        Debug.Log("Forward selected");
        /*
        weaponBackward.Move(E_Move.HIDE_UP);
        weaponBackward.Resize(E_Resize.MINIMIZE);

        mainWeapon.Move(E_Move.UP);
        mainWeapon.Resize(E_Resize.REDUCE);

        weaponForward.Move(E_Move.MIDDLE);
        weaponForward.Resize(E_Resize.MAXIMIZE);
        */

        foreach (var weapon in weapons)
        {
            weapon.MoveUp();
            weapon.Position = weapon.Position== E_ICON_POSITION.HIDE_UP ? E_ICON_POSITION.HIDE_DOWN : weapon.Position-1;
        }
    }

    private void onChangeWeaponWheel(InputAction.CallbackContext context)
    {
        Debug.Log("WHEEEEEEEEL!!");
    }
}
