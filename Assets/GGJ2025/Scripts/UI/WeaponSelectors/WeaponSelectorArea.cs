using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelectorArea : MonoBehaviour, IDraggable
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
    [SerializeField]
    private WeaponsDatabase weaponsDatabase;

    private int weaponCount;
    private int lastMove;
    private WeaponSelector[] weapons;


    #region MONO
    void Start()
    {        
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
    #endregion MONO

    #region Input Callback
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

    #endregion Input Callback

    #region Private methods
    private void ChangeWeapon(int forward)
    {
        if (!CanChangeWeapon()) return;
        if (forward> 0)     //Seleziona l'arma più in alto
        {
            foreach (var weapon in weapons)
            {
                if (!CanMove(forward)) return;
                if(!weapon.isActiveAndEnabled) continue;
                weapon.MoveDown();
                weapon.Position = weapon.Position == E_ICON_POSITION.HIDE_DOWN ? E_ICON_POSITION.HIDE_UP : weapon.Position + 1;
               
            }
        }
        else               //Seleziona l'arma più in basso   
        {                   
            foreach (var weapon in weapons)
            {
                if (!CanMove(forward)) return;
                if (!weapon.isActiveAndEnabled) continue;
                weapon.MoveUp();
                weapon.Position = weapon.Position == E_ICON_POSITION.HIDE_UP ? E_ICON_POSITION.HIDE_DOWN : weapon.Position - 1;
            }
        }
        SetHiddenIcons(forward);
        lastMove = forward;
    }

    private void SetHiddenIcons(int forward)
    {
        if (weaponCount < 3) return;
        if (weaponCount == 3)
        {
            if (forward > 0)     //Seleziona l'arma più in alto
            {
                var weapon = GetWeaponSelector(E_ICON_POSITION.HIDE_UP);
                weapon.SetSprite(GetWeaponSelector(E_ICON_POSITION.DOWN).GetSprite());
            }else
            {

                var weapon = GetWeaponSelector(E_ICON_POSITION.HIDE_DOWN);
                weapon.SetSprite(GetWeaponSelector(E_ICON_POSITION.UP).GetSprite());
            }
        }
        if(weaponCount == 4)
        {
            if (forward > 0)     //Seleziona l'arma più in alto
            {
                var weapon = GetWeaponSelector(E_ICON_POSITION.HIDE_UP);
                weapon.SetSprite(GetWeaponSelector(E_ICON_POSITION.HIDE_DOWN).GetSprite());
            }
            else
            {

                var weapon = GetWeaponSelector(E_ICON_POSITION.HIDE_DOWN);
                weapon.SetSprite(GetWeaponSelector(E_ICON_POSITION.HIDE_UP).GetSprite());
            }
        }
    }

    // controlliamo che le coroutine di movimento delle icone siano tutte finite prima di poter eseguire nuovamente il cambio arma
    private bool CanChangeWeapon()
    {        
        foreach (var weapon in weapons)
        {
            if(weapon.IsPrevented)
                return false;
        }
        return true;        
    }

    private bool CanMove(int forward)
    {
        if(weaponCount <2) return false;
        if (weaponCount == 2 && lastMove == forward)  return false;        
        return true;
    }

    private WeaponSelector GetWeaponSelector(E_ICON_POSITION position)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.Position == position)
                return weapon;
        }
        return null;
    }
    #endregion Private methods


    #region Public Methods
    public void Init(uint levelIndex)
    {
        mainWeapon.gameObject.SetActive(false);
        weaponBackward.gameObject.SetActive(false);
        weaponBackwHide.gameObject.SetActive(false);
        weaponForward.gameObject.SetActive(false);
        weaponForwdHide.gameObject.SetActive(false);

        mainWeapon.Position = E_ICON_POSITION.MIDDLE;
        weaponBackward.Position = E_ICON_POSITION.UP;
        weaponBackwHide.Position = E_ICON_POSITION.HIDE_UP;
        weaponForward.Position = E_ICON_POSITION.DOWN;
        weaponForwdHide.Position = E_ICON_POSITION.HIDE_DOWN;

        weapons = new WeaponSelector[] { mainWeapon, weaponBackward, weaponForward, weaponBackwHide, weaponForwdHide };

        WeaponData[] entries = weaponsDatabase.GetEntries();
        weaponCount = 0;

        for (int i = 0; i< entries.Length; i++)
        {
            var weaponEntry = entries[i];
            if (weaponEntry.levelToUnlock <= levelIndex)
            {
                weapons[i].SetSprite(weaponEntry.UI_IconSelector);
                weapons[i].gameObject.SetActive(true);
                weaponCount++;
            }
        }

        if (weaponCount > 2)
        {
            foreach (var weapon in weapons)
            {
                weapon.gameObject.SetActive(true);
            }

            //con tre armi disponibili, abbiamo che lo slot hide up è uguale allo slot down e lo slot hide down è uguale all'up
            if(weaponCount == 3)
            {
                weaponForwdHide.SetSprite(weaponBackward.GetSprite());
                weaponBackwHide.SetSprite(weaponForward.GetSprite());
            }
            if (weaponCount == 4)
            {
                weaponForwdHide.SetSprite(weaponBackwHide.GetSprite());
            }
        }
       
    }
    #endregion Public Methods



    #region Interface I_Draggable

    public void OnHoldAndRelease(bool up)
    {
        Debug.Log("HOLD AND RELEASE");
        if (up)
        {
            ChangeWeapon(-1);
        }else
        { 
            ChangeWeapon(1); 
        }
    }
    #endregion Interface I_Draggable
}
