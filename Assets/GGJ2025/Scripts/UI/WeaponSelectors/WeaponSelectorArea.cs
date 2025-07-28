using System;
using UnityEngine;

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

    private int weaponCount;
    private int lastMove;
    private WeaponSelector[] weaponIcons;

    #region MONO
    private void Awake()
    {
        weaponIcons = new WeaponSelector[] { mainWeapon, weaponBackward, weaponForward, weaponBackwHide, weaponForwdHide };
    }

    void OnEnable()
    {        
        WeaponManager.Get().OnStartWeaponLevel += OnStartWeaponLevel;
        WeaponManager.Get().OnChangeWeapon += OnChangeWeapon;
    }

    
    private void OnDisable()
    {
        WeaponManager.Get().OnChangeWeapon -= OnChangeWeapon;
        WeaponManager.Get().OnStartWeaponLevel -= OnStartWeaponLevel;
    }

    private void OnStartWeaponLevel()
    {
        Init();
    }
    #endregion MONO

    #region Input Callback
    private void OnChangeWeapon(int forward)
    {
        ChangeWeapon(forward);
    }
    #endregion Input Callback

    #region Private methods
    private void ChangeWeapon(int forward)
    {
        if (!CanChangeWeapon()) return;
        if (forward> 0)     //Seleziona l'arma più in alto
        {
            foreach (var weapon in weaponIcons)
            {
                if (!CanMove(forward)) return;
                if(!weapon.isActiveAndEnabled) continue;
                weapon.MoveDown();
                weapon.Position = weapon.Position == E_ICON_POSITION.HIDE_DOWN ? E_ICON_POSITION.HIDE_UP : weapon.Position + 1;
               
            }
        }
        else               //Seleziona l'arma più in basso   
        {                   
            foreach (var weapon in weaponIcons)
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
        foreach (var weapon in weaponIcons)
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
        foreach (var weapon in weaponIcons)
        {
            if (weapon.Position == position)
                return weapon;
        }
        return null;
    }
    #endregion Private methods


    #region Public Methods
    public void Init()
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


        Weapon[] unlockedWeapons = WeaponManager.Get().GetUnlockedWeapons();
        weaponCount = unlockedWeapons.Length;

        /*
        switch(weaponCount)
        {
            case 0:
                break;
            case 1:
                mainWeapon.SetSprite(unlockedWeapons[0].weaponData.UI_IconSelector);
                mainWeapon.gameObject.SetActive(true);
                break;
            case 2:
                mainWeapon.SetSprite(unlockedWeapons[0].weaponData.UI_IconSelector);
                mainWeapon.gameObject.SetActive(true);
                weaponBackward.SetSprite(unlockedWeapons[1].weaponData.UI_IconSelector);
                weaponBackward.gameObject.SetActive(true);
                break;
            case 3:
                mainWeapon.SetSprite(unlockedWeapons[0].weaponData.UI_IconSelector);
                mainWeapon.gameObject.SetActive(true);
                weaponForward.SetSprite(unlockedWeapons[1].weaponData.UI_IconSelector);
                weaponForward.gameObject.SetActive(true);
                weaponBackward.SetSprite(unlockedWeapons[2].weaponData.UI_IconSelector);
                weaponBackward.gameObject.SetActive(true);

                weaponForwdHide.SetSprite(weaponBackward.GetSprite());
                weaponBackwHide.SetSprite(weaponForward.GetSprite());
                weaponForwdHide.gameObject.SetActive(true);
                weaponBackwHide.gameObject.SetActive(true);

                break;
            case 4:
                mainWeapon.SetSprite(unlockedWeapons[0].weaponData.UI_IconSelector);
                mainWeapon.gameObject.SetActive(true);
                weaponForward.SetSprite(unlockedWeapons[1].weaponData.UI_IconSelector);
                weaponForward.gameObject.SetActive(true);
                weaponBackward.SetSprite(unlockedWeapons[2].weaponData.UI_IconSelector);
                weaponBackward.gameObject.SetActive(true);

                weaponForwdHide.SetSprite(unlockedWeapons[3].weaponData.UI_IconSelector);
                weaponBackwHide.SetSprite(unlockedWeapons[3].weaponData.UI_IconSelector);
                weaponForwdHide.gameObject.SetActive(true);
                weaponBackwHide.gameObject.SetActive(true);
                break;
            default:
                mainWeapon.SetSprite(unlockedWeapons[0].weaponData.UI_IconSelector);
                mainWeapon.gameObject.SetActive(true);
                weaponForward.SetSprite(unlockedWeapons[1].weaponData.UI_IconSelector);
                weaponForward.gameObject.SetActive(true);
                weaponBackward.SetSprite(unlockedWeapons[2].weaponData.UI_IconSelector);
                weaponBackward.gameObject.SetActive(true);
                weaponForwdHide.SetSprite(unlockedWeapons[3].weaponData.UI_IconSelector);
                weaponForwdHide.gameObject.SetActive(true);
                weaponBackwHide.SetSprite(unlockedWeapons[4].weaponData.UI_IconSelector);
                weaponBackwHide.gameObject.SetActive(true);
                break;

        }
        */

        Weapon[] wheelWeapons = WeaponManager.Get().GetWheelWeapons();
        for (int i = 0; i< wheelWeapons.Length; i++)
        {
            weaponIcons[i].SetSprite(wheelWeapons[i].weaponData.UI_IconSelector);
            weaponIcons[i].gameObject.SetActive(true);
        }
        switch (weaponCount)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                weaponForwdHide.SetSprite(weaponBackward.GetSprite());
                weaponBackwHide.SetSprite(weaponForward.GetSprite());
                weaponForwdHide.gameObject.SetActive(true);
                weaponBackwHide.gameObject.SetActive(true);
                break;
            case 4:
                weaponForwdHide.SetSprite(weaponBackwHide.GetSprite());
                weaponForwdHide.gameObject.SetActive(true);
                break;
            default:
                break;
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
