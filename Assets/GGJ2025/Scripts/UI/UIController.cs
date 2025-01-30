using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool firstTime = false;
    public bool isPrevented = false;
    private Coroutine waitBeforeUIInteract;
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
            //Debug.Log("UICOntroller - assegno UICOntroller a " + button.gameObject.name);
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
        //pause input
        InputManager.Player.TogglePause.performed += OnTogglePause;
        InputManager.Menu.TogglePause.performed += OnTogglePause;
    }

    private void Start()
    {
        //hide all menu
        foreach (BaseUI baseUI in uiPrefabs) {
            baseUI.Hide();
        }
        //open main menu by default
        OpenMenu(EUIType.MainMenu);
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
        if (isPrevented) return;
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
            case EUIType.GameplayHUD:
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

        //setting gameplayHUD menù?
        if(UIType == EUIType.GameplayHUD) {
            //have tutorial?
            if (OnCheckTutorial()) {
                SetupForUIMenu();
            } else {
                //normal gameplay
                SetupForGameplayHUD();
            }
        } else {
            //normal UI Menu
            SetupForUIMenu();
            if (waitBeforeUIInteract != null) {
                StopCoroutine(waitBeforeUIInteract);
            }
            waitBeforeUIInteract = StartCoroutine(WaitBeforeUIInteract());
        }
    }

    private void SetupForGameplayHUD() {
        Debug.Log("UIController - SetupForGameplayHUD");
        InputManager.Player.Enable();
        InputManager.Menu.Disable();
        Time.timeScale = 1f;
        if (weapon != null) {
            weapon.gameObject.SetActive(true);
        }
    }

    private void SetupForUIMenu() {
        Debug.Log("UIController - SetupForUIMenu");
        InputManager.Player.Disable();
        InputManager.Menu.Enable();
        Time.timeScale = 0f;
        if (weapon != null) {
            weapon.gameObject.SetActive(false);
        }
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

    private bool OnCheckTutorial() {
        uint currentLevel = LevelManager.Get().Level;
        if (levelToTutorialMap.TryGetValue(currentLevel, out EUITutorialType tutorialType)) {
            Debug.Log($"UIController - Apro tutorial level {currentLevel}");
            tutorialMenuPrefab.prepareTutorial(tutorialType);
            tutorialMenuPrefab.Show();
            AudioManager.PlayOneShotSound("MenuOpen");
            return true;
        } else {
            Debug.Log($"UIController - No tutorial associated with level {currentLevel}");
            return false;
        }
    }

    public void CloseCurrentTutorial() {
        tutorialMenuPrefab.Hide();
        AudioManager.PlayOneShotSound("MenuClose");
        SetupForGameplayHUD();
    }

    private void OnTogglePause(InputAction.CallbackContext context) {
        Debug.Log("UIController - OnTogglePause");
        if (currentMenu != gameplayHUDPrefab && currentMenu != pauseMenuPrefab) return;
        if (currentMenu == pauseMenuPrefab) {
            OpenMenu(EUIType.GameplayHUD);
        } else {
            OpenMenu(EUIType.PauseMenu);
        }
    }
    #endregion

    #region Wrapper menus methods
    public void OpenMainMenu() => OpenMenu(EUIType.MainMenu);
    public void OpenCreditsMenu() => OpenMenu(EUIType.CreditsMenu);
    public void OpenGameplayMenu() => OpenMenu(EUIType.GameplayHUD);
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