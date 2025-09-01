using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsDatabase", menuName = "Weapons/WeaponsDatabase", order = 2)]
public class WeaponsDatabase : ScriptableObject {
    [SerializeField]
    private WeaponData[] entries;

    public WeaponData[] WeaponData {  get { return entries; } }

    public WeaponData GetWeaponData(EWeaponType type) {
        foreach (WeaponData weapon in entries) {
            if(weapon.weaponType == type)
                return weapon;
        } 
        return new WeaponData();
    }
}