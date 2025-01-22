using System;
using UnityEngine;

[Serializable]
public struct LevelEntryStruct
{
    public bool is_Timer_Activate;
    public uint unlock_Lvl;
    public Vector2 grid_Size;
    public BubbleToCreate[] bubbles;
}
