using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Internal Variables
    [Header("Menu Prefabs")]
    [SerializeField] private BaseUI mainMenuPrefab;
    [SerializeField] private BaseUI creditsMenuPrefab;
    [SerializeField] private BaseUI gameplayMenuPrefab;
    [SerializeField] private BaseUI pauseMenuPrefab;
    [SerializeField] private BaseUI endLevelWinMenuPrefab;
    [SerializeField] private BaseUI endLevelLoseMenuPrefab;
    [SerializeField] private TutorialMenu tutorialMenuPrefab;
    private BaseUI currentMenu;
    private bool firstTime;
    #endregion

    #region Mono
    private void OnEnable() {
        //Base UI Menu
        GlobalEventSystem.AddListener(EventName.OpenUI, OnOpenUI);
    }

    private void Start()
    {
        //open main menu by default
        //OpenMenu(EUIType.MainMenu);
        //Check Tutorials
        LevelManager.Get().OnStart += OnCheckTutorial;
        if (!firstTime) {
            OnCheckTutorial();
            firstTime = true;
        }
    }
    #endregion Mono

    #region Global Event System
    private void OnOpenUI(EventArgs message) {
        EventArgsFactory.OpenUIParser(message, out EUIType UIType);
        OpenMenu(UIType);
    }
    #endregion

    #region Internal Methods
    public void OpenMenu(EUIType UIType)
    {
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
                currentMenu = gameplayMenuPrefab;
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
        switch (LevelManager.Get().Level) {
            case 1:
                OpenTutorial(EUITutorialType.FingerSimple);
                break;
            case 2:
                OpenTutorial(EUITutorialType.FingerMultiple);
                break;
            case 3:
                OpenTutorial(EUITutorialType.TimeLimit);
                break;
            case 5:
                OpenTutorial(EUITutorialType.Chisel);
                break;
            case 7:
                OpenTutorial(EUITutorialType.ToyHammer);
                break;
            case 10:
                OpenTutorial(EUITutorialType.WireCutter);
                break;
        }
    }

    private void OpenTutorial(EUITutorialType UITutorialType) {
        CloseCurrentMenu();
        tutorialMenuPrefab.prepareTutorial(UITutorialType);
        tutorialMenuPrefab.Show();
    }
    #endregion
}