using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponData")]
public class WeaponData : ScriptableObject {
    public EWeaponType weaponType;
    public Sprite beforeSprite;
    public Sprite postSprite;
}