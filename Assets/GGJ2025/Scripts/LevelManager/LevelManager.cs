using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelDatabase LevelDatabase;

    #region PrivateVariable
    private uint currentLevel; // playing level
    private uint reachedLevel; // max reached level
    private LevelEntryStruct currentEntryData;
    private float currentLevelTime;
    private bool isTimerActive = false;
    private bool soundBeepExecuted = false;
    private Dictionary<uint, uint> levelScores = new Dictionary<uint, uint>();
    
    private bool endlessMode = false;
    private uint endlessModeLevelReached;
    private float endlessModeTime;
    #endregion

    public Action<uint> OnStartLevel;
    public Action OnRetry;
    public Action<int> OnWinLevel;
    public Action OnLoseLevel;
    public Action<float> OnUpdateTimer;

    public Action OnStartEndlessLevel;
    public Action OnLoseEndlessLevel;

    #region Properties
    public uint CurrentLevel { 
        get 
        { 
            return currentLevel; 
        }
        set {
            currentLevel = value;
            Debug.Log($"Current Level set to: {currentLevel}");
        }
    }
    public uint ReachedLevel {
        get {
            return reachedLevel;
        }
        set {
            reachedLevel = value;
            Debug.Log($"Reached Level set to: {reachedLevel}");
        }
    }
    public bool EndlessMode
    {
        get
        {
            return endlessMode;
        }
        private set
        {
            endlessMode = value;
        }
    }
    public uint EndlessReachedLevel
    {
        get
        {
            return endlessModeLevelReached;
        }
    }
    public Dictionary<uint, uint> LevelScores {
        get {
            return levelScores;
        }
        set {
            levelScores = value;
        }
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
        SaveSystem.LoadLevel(out uint reachedLevelFromSave);
        SaveSystem.LoadEndlessLevel(out uint reachedEndlessLevelFromSave);
        ReachedLevel = reachedLevelFromSave > 0 ? reachedLevelFromSave : 1;
        endlessModeLevelReached = reachedEndlessLevelFromSave;
        Debug.Log($"Reached Level from Save: {ReachedLevel}");
        SaveSystem.LoadLevelScores(out Dictionary<uint, uint> levelScoresFromSave);
        LevelScores = levelScoresFromSave;        
        GlobalEventSystem.AddListener(EventName.ModulateTimer, OnModulateTimer);

        //ResetEndlessMode();
    }
   
    void OnDestroy()
    {
        SaveSystem.SaveFile(ReachedLevel, AudioManager.GetAllRawVolumes(), LevelScores, EndlessReachedLevel);
    }

    //Da convertire in coroutine
    private void Update()
    {
        
        if (!isTimerActive) return;

        if (!EndlessMode)
        {
            //---------STO COSO � PER FARE CASINO
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
            if (currentLevelTime <= 0)
            {
                OnLoseLevel?.Invoke();
                isTimerActive = false;
            }
        }else
        {
            currentLevelTime -= Time.deltaTime;
            OnUpdateTimer?.Invoke(currentLevelTime);
            if (currentLevelTime <= 0)
            {
                OnLoseEndlessLevel?.Invoke();
                isTimerActive = false;
            }
        }
    }
    #endregion

    #region PublicMethods
    public void RetryLevel()
    {
        if(endlessMode)
        {
            ResetEndlessMode();
        }
        OnRetry?.Invoke();
    }

    public void StartLevel(uint levelIndex)
    {
        EndlessMode = false;
        CurrentLevel = levelIndex;
        currentEntryData = LevelDatabase.GetCurrentEntry(CurrentLevel);
        OnStartLevel?.Invoke(CurrentLevel);
    }

    //---TODO
    public void StartEndlessMode()
    {
        EndlessMode = true;
        CurrentLevel = 99;
        currentEntryData = LevelDatabase.GetEndlessLevelEntry();
        OnStartEndlessLevel?.Invoke();
    }
    
    public void ResetEndlessMode()
    {
        endlessModeTime = 40;
        endlessModeLevelReached = 0;
    }
    
    public void WinEndlessModeLevel()
    {
        endlessModeTime = currentLevelTime + currentEntryData.timer_for_level;  //qui si guadagna il bonus tempo
        endlessModeLevelReached++;
        StartEndlessMode();
    }
    //---

    public void OnDeleteSaves()
    {   
        ReachedLevel = 1;
        CurrentLevel = 1;
        LevelScores.Clear();
        //currentEntryData = LevelDatabase.GetCurrentEntry(CurrentLevel);
    }

    public void WinLevel()
    {
        if (!EndlessMode)           //livelli normali
        {
            //Calcolo del punteggio finale del livello
            int starNumbers = 0;
            float[] startsThreshold = ActiveEntryData.stars_for_level;
            for (int i = 0; i < startsThreshold.Length; i++)
            {
                if (startsThreshold[i] <= GetTimerPercent())
                {
                    starNumbers++;
                }
                else
                {
                    break;
                }
            }
            Debug.Log($"Level {CurrentLevel} completed with {starNumbers} stars.");
            //write level only if never writtren or if the new score is better
            if (!LevelScores.ContainsKey(CurrentLevel) || starNumbers > LevelScores[CurrentLevel])
            {
                LevelScores[CurrentLevel] = (uint)starNumbers;
            }
            OnWinLevel?.Invoke(starNumbers);
        }
        else
        {                   //livelli della endless mode
            WinEndlessModeLevel();
        }
    }
    #endregion

    public void StartTiming()
    {
        if (EndlessMode)
        {
            currentLevelTime = endlessModeTime;
            isTimerActive = true;
        }
        else
        {
            currentLevelTime = currentEntryData.timer_for_level;
            isTimerActive = currentEntryData.is_Timer_Activate;
        }
    }

    private void OnModulateTimer(EventArgs message)
    {
        EventArgsFactory.ModulateTimerParser(message, out float arg);
        currentLevelTime += arg;
    }

    private float GetTimerPercent()
    {
        return currentLevelTime / currentEntryData.timer_for_level;
    }
}
