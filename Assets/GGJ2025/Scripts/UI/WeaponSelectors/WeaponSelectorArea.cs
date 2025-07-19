using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponSelectorArea : MonoBehaviour
{
    [SerializeField]
    WeaponSelector mainWeapon;
    [SerializeField]
    WeaponSelector weaponBackward;
    [SerializeField]
    WeaponSelector weaponForward;

    void Start()
    {
        InputManager.Player.ChangeWeaponForward.performed += onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed += onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed -= onChangeWeaponWheel;
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
        mainWeapon.Move(E_Move.UP);
        mainWeapon.Resize(E_Resize.REDUCE);

    }

    private void onChangeWeaponForward(InputAction.CallbackContext context)
    {
        Debug.Log("Forward selected");
        mainWeapon.Move(E_Move.MIDDLE);
        mainWeapon.Resize(E_Resize.GROWN);

    }

    private void onChangeWeaponWheel(InputAction.CallbackContext context)
    {
        Debug.Log("WHEEEEEEEEL!!");
    }
}
