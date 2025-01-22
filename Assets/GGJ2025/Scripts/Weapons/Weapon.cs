using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;

    public void prepareWeapon(WeaponData weaponData) {
        this.weaponData = weaponData;
    }
}
