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
    [SerializeField] private EndLevelWinMenu endLevelWinMenuPrefab;
    [SerializeField] private EndLevelLoseMenu endLevelLoseMenuPrefab;
    [SerializeField] private TutorialMenu tutorialMenuPrefab;
    [SerializeField] private GameplayUIMenu gameplayMenuPrefab;
    [SerializeField] private LevelHubMenu levelHubMenuPrefab;
    [SerializeField] private OptionsMenu optionsMenuPrefab;
    [SerializeField] private RectTransform weapon;

    //Other
    private BaseUI currentMenu;
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
    #endregion

    #region Mono
    private void OnEnable() {
        LevelManager.Get().OnStartLevel += OnStartLevel;
        LevelManager.Get().OnWinLevel += OnWinLevel;
        LevelManager.Get().OnLoseLevel += OnLoseLevel;
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
            tutorialMenuPrefab,
            gameplayMenuPrefab,
            optionsMenuPrefab,
            levelHubMenuPrefab
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
        AudioManager.PlayBackgroundMusic("MainMenuMusic");
    }
    #endregion Mono

    #region Callback Actions
    private void OnStartLevel(uint levelIndex)
    {
        OpenMenu(EUIType.GameplayHUD);
    }
    private void OnWinLevel(int starNumbers)
    {
        OpenMenu(EUIType.EndLevelWinMenu);
        endLevelWinMenuPrefab.ShowRightStars(starNumbers);
    }
    private void OnLoseLevel()
    {
        OpenMenu(EUIType.EndLevelLoseMenu);
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
            case EUIType.LevelHubMenu:
                currentMenu = levelHubMenuPrefab;
                break;
            case EUIType.OptionsMenu:
                currentMenu = optionsMenuPrefab;
                break;
        }
        //open
        if (currentMenu != null) {
            currentMenu.Show();            
        } else {
            Debug.LogWarning("UIController - Menu to open not found" + UIType.ToString());
        }

        //setting gameplayHUD men�?
        if(UIType == EUIType.GameplayHUD) {
            //have tutorial?
            if (OnCheckTutorial()) {
                SetupForUIMenu();
            } else {
                //normal gameplay
                SetupForGameplayHUD();
            }
        } 
        else 
        {
            //normal UI Menu
            SetupForUIMenu();
            if (waitBeforeUIInteract != null) {
                StopCoroutine(waitBeforeUIInteract);
            }
            waitBeforeUIInteract = StartCoroutine(WaitBeforeUIInteract());
        }
    }

    private void SetupForGameplayHUD() {
        InputManager.Player.Enable();
        InputManager.Menu.Disable();
        Time.timeScale = 1f;
        gameplayMenuPrefab.Show();  // Attivo i tre bottoni Pause Left e Right
        if (weapon != null) {
            weapon.gameObject.SetActive(true);
        }
    }

    private void SetupForUIMenu() {
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
            Debug.LogWarning("UIController - Nothing to close");
            return;
        };
        currentMenu.Hide();
        gameplayMenuPrefab.Hide();  // Disattivo i tre bottoni Pause Left e Right TIPO SEMPRE, BELLA SCHIFEZZ
        currentMenu = null;
    }

    private bool OnCheckTutorial() {
        uint currentLevel = LevelManager.Get().CurrentLevel;
        if (levelToTutorialMap.TryGetValue(currentLevel, out EUITutorialType tutorialType)) {
            //Debug.Log($"UIController - Apro tutorial level {currentLevel}");
            tutorialMenuPrefab.prepareTutorial(tutorialType);
            tutorialMenuPrefab.Show();
            AudioManager.PlayOneShotSound("MenuOpen");
            return true;
        } else {
            //Debug.Log($"UIController - No tutorial associated with level {currentLevel}");
            return false;
        }
    }

    public void CloseCurrentTutorial() {
        tutorialMenuPrefab.Hide();
        AudioManager.PlayOneShotSound("MenuClose");
        SetupForGameplayHUD();
    }

    private void OnTogglePause(InputAction.CallbackContext context) {
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
    public void OpenLevelHubMenu() => OpenMenu(EUIType.LevelHubMenu);
    public void OpenOptionsMenu() => OpenMenu(EUIType.OptionsMenu);

    #endregion

    #region Coroutine
    private IEnumerator WaitBeforeUIInteract() {
        isPrevented = true;
        yield return new WaitForSecondsRealtime(0.5f);
        isPrevented = false;
    }
    #endregion
}