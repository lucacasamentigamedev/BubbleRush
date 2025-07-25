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

    //--test
    private bool isDragging = false;
    private Vector2 startMousePos;
    public float upwardThreshold = 50f;
    //------

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
        lastMove = forward;
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
       
    }
    #endregion Public Methods

    private void OnMouseDown()
    {
        isDragging = true;
        startMousePos = Mouse.current.position.ReadValue();
    }
    private void OnMouseUp()
    {
        if (isDragging)
        {
            Vector2 endMousePos = Mouse.current.position.ReadValue();
            float deltaY = endMousePos.y - startMousePos.y;

            if (deltaY > upwardThreshold)
            {
                Debug.Log("Hai trascinato verso l'alto!");
            }
        }

        isDragging = false;
    }

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
}
