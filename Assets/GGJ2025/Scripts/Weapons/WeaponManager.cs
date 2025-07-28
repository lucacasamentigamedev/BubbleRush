using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    #region StaticMembers
    private static WeaponManager instance;

    public static WeaponManager Get()
    {
        if (instance != null) return instance;
        instance = FindObjectOfType<WeaponManager>();
        return instance;
    }
    #endregion

    [SerializeField]
    private WeaponsDatabase weaponDatabase;
    private Weapon[] avaiableWeapons;
    private int currentIndexWeapon;
    private Weapon[] unlockWeapons;

    public Weapon CurrentWeapon { get; private set; }

    

    public Action<int> OnChangeWeapon;
    public Action OnStartWeaponLevel;


    #region Mono
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        LevelManager.Get().OnStartLevel += OnStartLevel;
        
        avaiableWeapons = new Weapon[(int)EWeaponType.LAST];
        for (int i = 0; i < avaiableWeapons.Length; i++)
        {
            avaiableWeapons[i] = new Weapon();
            avaiableWeapons[i].prepareWeapon(weaponDatabase.GetWeaponData((EWeaponType)i));
        }
    }
    private void OnDestroy()
    {
        Debug.Log("Weapon Selector Mng Destroy");
        LevelManager.Get().OnStartLevel -= OnStartLevel;
       
    }

    private void OnStartLevel(uint levelIndex)
    {
        CurrentWeapon = avaiableWeapons[0];
        currentIndexWeapon = 0;

        foreach (Weapon weapon in avaiableWeapons)
        {
            if (weapon.weaponData.levelToUnlock <= levelIndex && !weapon.weaponData.IsUnlocked)
            {
                weapon.weaponData.IsUnlocked = true;
            }
        }
        SetUnlockWeapon();
        OnStartWeaponLevel?.Invoke();
    }
      
    #endregion


    public void ChangeWeapon(int forward)
    {

        currentIndexWeapon += forward;
        if(currentIndexWeapon  < 0)
        {
            currentIndexWeapon = unlockWeapons.Length - 1;
        }else
        {
            currentIndexWeapon = currentIndexWeapon % unlockWeapons.Length;
        }
        CurrentWeapon = unlockWeapons[currentIndexWeapon];




        OnChangeWeapon?.Invoke(forward);

    }


    public Weapon[] GetWheelWeapons()
    {
        Weapon[] wheelWeapon;
        switch (unlockWeapons.Length)
        {
            case 0:
                return null;
            case 1:
                wheelWeapon = new Weapon[1];
                wheelWeapon[0] = CurrentWeapon;
                break;
            case 2:
                wheelWeapon = new Weapon[2];
                wheelWeapon[0] = CurrentWeapon;
                wheelWeapon[1] = unlockWeapons[currentIndexWeapon + 1 % unlockWeapons.Length];
                break;
            case 3:
                //  formula buffer circolare ((c + i + N) % N) 
                //(currentIndexWeapon -1 + 1 + unlockWeapons.Length)  % unlockWeapons.Length ) 
                wheelWeapon = new Weapon[3];
                wheelWeapon[0] = CurrentWeapon;
                wheelWeapon[1] = unlockWeapons[(currentIndexWeapon + 1 + unlockWeapons.Length) % unlockWeapons.Length ];
                wheelWeapon[2] = unlockWeapons[(currentIndexWeapon - 1 + unlockWeapons.Length) % unlockWeapons.Length ];
                break;
            case 4:
                wheelWeapon = new Weapon[4];
                wheelWeapon[0] = CurrentWeapon;
                wheelWeapon[1] = unlockWeapons[(currentIndexWeapon + 1 + unlockWeapons.Length) % unlockWeapons.Length];
                wheelWeapon[2] = unlockWeapons[(currentIndexWeapon - 1 + unlockWeapons.Length) % unlockWeapons.Length];
                wheelWeapon[3] = unlockWeapons[(currentIndexWeapon + 2 + unlockWeapons.Length) % unlockWeapons.Length];
                break;
            default:
                wheelWeapon = new Weapon[5];
                wheelWeapon[0] = CurrentWeapon;
                wheelWeapon[1] = unlockWeapons[(currentIndexWeapon + 1 + unlockWeapons.Length) % unlockWeapons.Length];
                wheelWeapon[2] = unlockWeapons[(currentIndexWeapon - 1 + unlockWeapons.Length) % unlockWeapons.Length];
                wheelWeapon[3] = unlockWeapons[(currentIndexWeapon + 2 + unlockWeapons.Length) % unlockWeapons.Length];
                wheelWeapon[4] = unlockWeapons[(currentIndexWeapon - 2 + unlockWeapons.Length) % unlockWeapons.Length];
                break;

        }
        return wheelWeapon;
    }


   
    public Weapon[] GetUnlockedWeapons()
    {
        return unlockWeapons;
    }

    private void SetUnlockWeapon()
    {

        List<Weapon> list = new List<Weapon>();

        foreach (Weapon weapon in avaiableWeapons)
        {
            if (weapon.weaponData.IsUnlocked)
                list.Add(weapon);
        }
        unlockWeapons= list.ToArray();
    }
}
