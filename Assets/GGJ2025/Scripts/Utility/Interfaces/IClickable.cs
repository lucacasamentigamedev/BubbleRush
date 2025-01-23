using UnityEngine;

public interface IClickable 
{
    void OnClick(Vector2 point, EWeaponType weapon,int damage, Vector2 area);
}
