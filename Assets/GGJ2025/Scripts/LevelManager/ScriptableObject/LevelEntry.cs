using UnityEngine;

[CreateAssetMenu(fileName = "Level_Entry", menuName = "LevlSystem/Level_Entry", order = 1)]
public class LevelEntry : ScriptableObject
{
    [SerializeField]
    private LevelEntryStruct data;

    //[SerializeField]
    //private uint unlock_Lvl;
    //[SerializeField]
    //private Vector2 grid_Size;
    // Ball list
    //[SerializeField]
    //private int[] ball_Type;

    //public uint UnlockLvl { get { return unlock_Lvl; } }
    //public Vector2 GridSize { get { return grid_Size; } }

    public LevelEntryStruct Data {  get { return data; } }
}
