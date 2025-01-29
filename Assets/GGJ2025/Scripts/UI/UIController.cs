using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Internal Variables
    //Menus
    [Header("Menu Prefabs")]
    private BaseUI[] uiPrefabs;
    [SerializeField] private BaseUI mainMenuPrefab;
    [SerializeField] private BaseUI creditsMenuPrefab;
    [SerializeField] private GameplayHUD gameplayHUDPrefab;
    [SerializeField] private BaseUI pauseMenuPrefab;
    [SerializeField] private BaseUI endLevelWinMenuPrefab;
    [SerializeField] private BaseUI endLevelLoseMenuPrefab;
    [SerializeField] private TutorialMenu tutorialMenuPrefab;
    //Other
    private BaseUI currentMenu;
    private bool firstTime;
    private bool isPrevented;
    //Tutorials
    private readonly Dictionary<uint, EUITutorialType> levelToTutorialMap = new Dictionary<uint, EUITutorialType>()
    {
        { 1, EUITutorialType.FingerSimple },
        { 2, EUITutorialType.FingerMultiple },
        { 3, EUITutorialType.TimeLimit },
        { 5, EUITutorialType.Chisel },
        { 7, EUITutorialType.ToyHammer },
        { 10, EUITutorialType.WireCutter }
    };
    [SerializeField]
    private RectTransform weapon;
    #endregion

    #region Mono
    private void OnEnable() {
        //Base UI Menu
        GlobalEventSystem.AddListener(EventName.OpenUI, OnOpenUI);
        GlobalEventSystem.AddListener(EventName.ChangeUILevelLabel, OnChangeUILevelLabel);
    }

    private void Awake() {
        //pass UIController to every button
        BRButton[] buttons = GetComponentsInChildren<BRButton>(true);
        foreach (BRButton button in buttons) {
            Debug.Log("UICOntroller - assegno UICOntroller a " + button.gameObject.name);
            button.Init(this);
        }
        //collect every UI into array
        uiPrefabs = new BaseUI[]
        {
            mainMenuPrefab,
            creditsMenuPrefab,
            gameplayHUDPrefab,
            pauseMenuPrefab,
            endLevelWinMenuPrefab,
            endLevelLoseMenuPrefab,
            tutorialMenuPrefab
        };
    }

    private void Start()
    {
        //hide all
        foreach (BaseUI baseUI in uiPrefabs) {
            baseUI.Hide();
        }
        //open main menu
        OpenMenu(EUIType.MainMenu);
        //Check Tutorials
        LevelManager.Get().OnStart += OnCheckTutorial;
        if (!firstTime) {
            OnCheckTutorial();
            firstTime = true;
        }
        //play bg music
        AudioManager.PlayBackgroundMusic("MainMenuMusic");
    }
    #endregion Mono

    #region Global Event System
    private void OnOpenUI(EventArgs message) {
        EventArgsFactory.OpenUIParser(message, out EUIType UIType);
        OpenMenu(UIType);
    }
    private void OnChangeUILevelLabel(EventArgs message) {
        gameplayHUDPrefab.ChangeLevelLabel();
    }
    #endregion

    #region Internal Methods
    public void OpenMenu(EUIType UIType)
    {
        if(isPrevented) return;
        //close current
        CloseCurrentMenu();
        //set requested
        switch (UIType) {
            case EUIType.MainMenu:
                currentMenu = mainMenuPrefab;
                break;
            case EUIType.CreditsMenu:
                currentMenu = creditsMenuPrefab;
                break;
            case EUIType.GameplayMenu:
                currentMenu = gameplayHUDPrefab;
                break;
            case EUIType.PauseMenu:
                currentMenu = pauseMenuPrefab;
                break;
            case EUIType.EndLevelWinMenu:
                currentMenu = endLevelWinMenuPrefab;
                break;
            case EUIType.EndLevelLoseMenu:
                currentMenu = endLevelLoseMenuPrefab;
                break;
        }
        //open
        if (currentMenu != null) {
            currentMenu.Show();
        } else {
            Debug.Log("UIController - Menu to open not found" + UIType.ToString());
        }

        //if gameplay reactive time and show weapon
        if(UIType == EUIType.GameplayMenu) {
            InputManager.Player.Enable();
            InputManager.Menu.Disable();
            Time.timeScale = 1f;
            weapon.gameObject.SetActive(true);
        } else {
            //if other menu stop time, hide weapon
            InputManager.Player.Disable();
            InputManager.Menu.Enable();
            Time.timeScale = 0f;
            weapon.gameObject.SetActive(false);
        }
        StartCoroutine(WaitBeforeUIInteract());
    }

    public void CloseCurrentMenu()
    {
        if(currentMenu == null) {
            Debug.Log("UIController - Nothing to close");
            return;
        };
        currentMenu.Hide();
        currentMenu = null;
    }

    private void OnCheckTutorial() {
        uint currentLevel = LevelManager.Get().Level;
        if (levelToTutorialMap.TryGetValue(currentLevel, out var tutorialType)) {
            OpenTutorial(tutorialType);
        } else {
            Debug.Log($"No tutorial associated with level {currentLevel}");
        }
    }

    private void OpenTutorial(EUITutorialType UITutorialType) {
        //FIXME: per ora l'ho commentato
        return;
        CloseCurrentMenu();
        tutorialMenuPrefab.prepareTutorial(UITutorialType);
        tutorialMenuPrefab.Show();
    }
    #endregion

    #region Wrapper methods
    public void OpenMainMenu() => OpenMenu(EUIType.MainMenu);
    public void OpenCreditsMenu() => OpenMenu(EUIType.CreditsMenu);
    public void OpenGameplayMenu() => OpenMenu(EUIType.GameplayMenu);
    public void OpenPauseMenu() => OpenMenu(EUIType.PauseMenu);
    public void OpenEndLevelWinMenu() => OpenMenu(EUIType.EndLevelWinMenu);
    public void OpenEndLevelLoseMenu() => OpenMenu(EUIType.EndLevelLoseMenu);
    #endregion

    #region Coroutine
    private IEnumerator WaitBeforeUIInteract() {
        isPrevented = true;
        Debug.Log("START CORUTINE BUTTON TOGGLE");
        yield return new WaitForSecondsRealtime(0.5f);
        isPrevented = false;
        Debug.Log("STOP CORUTINE BUTTON TOGGLE");
    }
    #endregion
}