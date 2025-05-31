using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelDatabase LevelDatabase;

    #region PrivateVariable
    private uint currentLevel;
    private LevelEntryStruct currentEntryData;
    private float currentLevelTime;
    private bool isTimerActive = false;
    private bool soundBeepExecuted = false;
    private uint currentLevelUnlocked = 10;
   
    private Dictionary<uint, uint> levelScores = new Dictionary<uint, uint>();  //Creiamo una variabile per salvare i punteggi effettuati nei vari livelli

    #endregion

    public Action<uint> OnStartLevel;
    public Action OnRetry;
    public Action OnWinLevel;
    public Action OnLoseLevel;
    public Action<float> OnUpdateTimer;

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
            if(currentLevelUnlocked < currentLevel)
                currentLevelUnlocked = currentLevel;
        } 
    }

    public float CurrentLevelTimer
    {
        get { return currentLevelTime; }
        set { currentLevelTime = value; }
    }

    public LevelEntryStruct ActiveEntryData { get  { return currentEntryData; } }
    #endregion

    #region StaticMembers
    private static LevelManager instance;

    public static LevelManager Get()
    {
        if (instance != null) return instance;
        instance = FindObjectOfType<LevelManager>();
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
        currentLevelUnlocked = currentLevel;
        currentEntryData = LevelDatabase.GetCurrentEntry(currentLevel);
        GlobalEventSystem.AddListener(EventName.StartTimer, OnStartLevelCallback);
        GlobalEventSystem.AddListener(EventName.ModulateTimer, OnModulateTimer);
    }

   
    void OnDestroy()
    {
        Debug.Log("OnDestroy LevelMngr");
        SaveSystem.SaveFile(currentLevel);
    }

    //Da convertire in coroutine
    private void Update()
    {
        
        if (!isTimerActive) return;

        //---------STO COSO è PER FARE CASINO
        int t = (int)currentLevelTime;
        if (((t <= 3 && t > 2) || (t <= 1 && t > 0)) && soundBeepExecuted)
        {
            AudioManager.PlayOneShotSound("TimeEndBeep");
            soundBeepExecuted = false;
        }
        else if (((t <= 4 && t > 3) || (t <= 2 && t > 1) || t == 0) && !soundBeepExecuted)
        {
            AudioManager.PlayOneShotSound("TimeEndBeep");
            soundBeepExecuted = true;
        }
        //----------------------------
        currentLevelTime -= Time.deltaTime;
        OnUpdateTimer?.Invoke(currentLevelTime);
        if(currentLevelTime <= 0) 
        {
            GlobalEventSystem.CastEvent(EventName.OpenUI, EventArgsFactory.OpenUIFactory(EUIType.EndLevelLoseMenu));
            OnLoseLevel?.Invoke();
            isTimerActive = false;
        }
    }
    #endregion

    #region PublicMethods
    public void RetryLevel()
    {
        OnRetry?.Invoke();
    }

    public void StartLevel(uint levelIndex)
    {
        OnStartLevel?.Invoke(levelIndex);
        currentLevel = levelIndex;
    }
    
    public void WinLevel()
    {
        OnWinLevel?.Invoke(); 
        GlobalEventSystem.CastEvent(EventName.OpenUI, EventArgsFactory.OpenUIFactory(EUIType.EndLevelWinMenu));
        if (levelScores[currentLevel] > 0)
        {

        }
        UnlockNewLevel();
    }

    public LevelEntryStruct GetLevelEntryData(uint levelIndex)
    {
        return LevelDatabase.GetCurrentEntry(levelIndex);
    }
    #endregion

    private void OnStartLevelCallback(EventArgs message)
    {
        currentLevelTime = currentEntryData.timer_for_level;
        isTimerActive = currentEntryData.is_Timer_Activate;
    }
    private void OnModulateTimer(EventArgs message)
    {
        EventArgsFactory.ModulateTimerParser(message, out float arg);
        currentLevelTime += arg;
    }
    private void UnlockNewLevel()
    {
        if (currentLevelUnlocked == currentLevel)
            currentLevelUnlocked +=1;
    }

}
