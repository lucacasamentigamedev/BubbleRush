using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_Databases", menuName = "LevlSystem/Level_Databases", order = 2)]
public class LevelDatabase : ScriptableObject
{
    [SerializeField]
    private LevelEntry[] entries;
    [SerializeField]
    private LevelEntry[] endlessLvlEntries;

    public LevelEntryStruct GetCurrentEntry(uint level)
    {
        for (int i = entries.Length - 1; i >= 0 ; i--)
        {
            if (level >= entries[i].Data.unlock_Lvl)
            {
                return entries[i].Data;
            }
        }

        return new LevelEntryStruct();
    }

    public LevelEntryStruct GetEndlessLevelEntry()
    {
        if (endlessLvlEntries.Length == 0)
            return new LevelEntryStruct();

        return endlessLvlEntries[UnityEngine.Random.Range(0, endlessLvlEntries.Length)].Data;

    }
}