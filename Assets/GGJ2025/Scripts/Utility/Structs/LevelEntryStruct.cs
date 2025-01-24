using System;
using UnityEngine;

[Serializable]
public struct LevelEntryStruct
{
    public bool is_Timer_Activate;
    public float timer_for_level;    
    public float[] stars_for_level;
    public uint unlock_Lvl;
    public Vector2 grid_Size;
    public BubbleToCreate[] bubbles;
}
