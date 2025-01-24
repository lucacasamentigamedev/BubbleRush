using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData", order = 1)]
public class WeaponData : ScriptableObject {
    public EWeaponType weaponType;
    public bool IsUnlocked { get; set; }
    public uint levelToUnlock;
    public int damage;
    public Vector2 area;
    public Sprite preInteract;
    public Sprite postInteract;

}