using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIMenu : BaseUI
{
    [SerializeField]
    private WeaponsDatabase weaponDatabase;
    [SerializeField]
    private ChangeWeaponButton weaponButtonUp;
    [SerializeField]
    private ChangeWeaponButton weaponButtonDown;
    [SerializeField]
    private Image weaponImageCenter;
    [SerializeField]
    private Animator anim;

    private List<Weapon> availablesWeapon;
    private int currentIndexWeapon;


    public void Awake()
    {
        availablesWeapon = new List<Weapon>();
        weaponButtonUp.OnButtonClick += OnClickUpperWeapon;
        weaponButtonDown.OnButtonClick += OnClickDownerWeapon;
// #if UNITY_ANDROID || UNITY_IOS
        weaponButtonUp.gameObject.SetActive(true);
        weaponButtonDown.gameObject.SetActive(true);
        weaponImageCenter.gameObject.SetActive(true);
// #endif

    }

// #if UNITY_ANDROID || UNITY_IOS
    public void OnEnable()
    {
        foreach (WeaponData weaponData in weaponDatabase.WeaponData)
        {
            if (weaponData.levelToUnlock <= LevelManager.Get().CurrentLevel)
            {
                Weapon weapon = new Weapon();
                weapon.weaponData = weaponData;
                availablesWeapon.Add(weapon);
            }
            
        }
        Debug.Log("Armi sbloccate  = "+ availablesWeapon.Count);

        if (availablesWeapon.Count == 1)
        {
            weaponButtonUp.gameObject.SetActive(false);
            weaponButtonDown.gameObject.SetActive(false);
            weaponImageCenter.sprite = availablesWeapon[0].weaponData.UIicon;
            currentIndexWeapon = 0;
        }
        else
        {
            weaponButtonUp.gameObject.SetActive(true);
            weaponButtonDown.gameObject.SetActive(true);            
            currentIndexWeapon = 0;
            SetUpperAndDownerButtons();
        }        
    }
// #endif

    public void OnClickUpperWeapon()
    {
        currentIndexWeapon = currentIndexWeapon == 0 ? availablesWeapon.Count - 1 : currentIndexWeapon - 1;
        anim.Play("UIScroll");

        // SetUpperAndDownerButtons();

    }

    public void OnClickDownerWeapon()
    {
        currentIndexWeapon = (currentIndexWeapon + 1)% availablesWeapon.Count;
        anim.Play("UIScroll");
    }

    private void SetUpperAndDownerButtons()
    {
        int indexPrev;
        int indexNext;
        if (currentIndexWeapon == 0)
        {
            indexPrev = availablesWeapon.Count - 1;
            indexNext = 1;
        }
        else
        {
            indexPrev = currentIndexWeapon - 1;
            indexNext = (currentIndexWeapon + 1) % availablesWeapon.Count;
        }
         
        weaponButtonUp.SetSprite(availablesWeapon[indexPrev].weaponData.UIicon);
        weaponButtonUp.WeaponType = availablesWeapon[indexPrev].weaponData.weaponType;

        weaponButtonDown.SetSprite(availablesWeapon[indexNext].weaponData.UIicon);
        weaponButtonDown.WeaponType = availablesWeapon[indexNext].weaponData.weaponType;

        weaponImageCenter.sprite = availablesWeapon[currentIndexWeapon].weaponData.UIicon;
    }



    public void OnDisable()
    {
        availablesWeapon.Clear();
    }

    public void SwitchWeaponResourcesButtons()
    {
        SetUpperAndDownerButtons();
    }

}