using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelDatabase LevelDatabase;

    #region PrivateVariable
    private uint currentLevel;
    private int currentLevelStarsObtained;
    private LevelEntryStruct currentEntryData;
    #endregion

    public Action OnStart;
    public Action OnRetry;

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

    public int CurrentLevelStarsObtained {
        get { return currentLevelStarsObtained; }
        set { currentLevelStarsObtained = value; }
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
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
        // Start is called before the first frame update
    void Start()
    {
        SaveSystem.LoadFile(out currentLevel);
        Debug.Log("Level Manager - Start current level: " + currentLevel);
        currentEntryData = LevelDatabase.GetCurrentEntry(currentLevel);
        //Debug.Log(currentEntryData.unlock_Lvl);
    }

    void OnDestroy()
    {
        SaveSystem.SaveFile(currentLevel);
    }
    #endregion

    #region PubblicMethods
    public void RetryLevel()
    {
        OnRetry?.Invoke();
    }
    public void StartGame()
    {
        OnStart?.Invoke();
    }
    #endregion
}
