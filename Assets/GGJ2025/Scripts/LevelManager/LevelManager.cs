using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelDatabase LevelDatabase;

    #region PrivateVariable
    private uint currentLevel;
    private LevelEntryStruct currentEntryData;
    #endregion

    #region Properties
    public uint Level { 
        get 
        { 
            return currentLevel; 
        } 
        set 
        { 
            currentLevel = value; 
            currentEntryData = LevelDatabase.GetCurrentEntry(currentLevel); 
        } 
    }

    public LevelEntryStruct ActiveEntryData { get  { return currentEntryData; } }
    #endregion

    #region StaticMembers
    private static LevelManager instance;

    public static LevelManager Get()
    {
        if (instance != null) return instance;
        instance = GameObject.FindObjectOfType<LevelManager>();
        return instance;
    }
    #endregion

    #region MonoBehaviourMethods
    // Start is called before the first frame update
    void Start()
    {
        SaveSystem.LoadFile(out currentLevel);
        Debug.Log(currentLevel);
        currentEntryData = LevelDatabase.GetCurrentEntry(currentLevel);
        Debug.Log(currentEntryData.unlock_Lvl);
    }

    void OnDestroy()
    {
        SaveSystem.SaveFile(currentLevel);
    }
    #endregion
}
