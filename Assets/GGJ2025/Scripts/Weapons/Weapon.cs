using UnityEngine;

public class Weapon
{
    public WeaponData weaponData;

    public void prepareWeapon(WeaponData weaponData) {
        this.weaponData = weaponData;
        this.weaponData.IsUnlocked = false;
    }
}
