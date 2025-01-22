using UnityEngine;

public enum EWeaponType {
    Finger,
    Chisel,
    ToyHammer,
    WireCutter,
    Rocket,
    GravityGun,
    DoubleSword,
    GoldenHammer
}

public class Weapon : MonoBehaviour
{
    private EWeaponType weaponType;

    public EWeaponType WeaponType {
        get { return weaponType; }
        set {
            weaponType = value;
            Debug.Log("Weapon type changed to: " + weaponType);
        }
    }

    private void Awake() {
        WeaponType = EWeaponType.Finger;
    }
}
