using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    #region Menus
    [SerializeField]
    private Button mainMenu;
    [SerializeField]
    private Button credits;
    [SerializeField]
    private Button gameplay;
    [SerializeField]
    private Button pause;
    [SerializeField]
    private Button endLevelWin;
    [SerializeField]
    private Button endLevelLose;
    #endregion

    #region Tutorials
    [SerializeField]
    private Button tutorial1;
    [SerializeField]
    private Button tutorial2;
    [SerializeField]
    private Button tutorial3;
    [SerializeField]
    private Button tutorial4;
    [SerializeField]
    private Button tutorial5;
    [SerializeField]
    private Button tutorial6;
    #endregion

    #region Mono
    void Awake()
    {
        //Menus
        mainMenu.onClick.AddListener(OnMainMenuClick);
        credits.onClick.AddListener(OnCreditsClick);
        gameplay.onClick.AddListener(OnGameplayClick);
        pause.onClick.AddListener(OnPauseClick);
        endLevelWin.onClick.AddListener(OnEndLevelWinClick);
        endLevelLose.onClick.AddListener(OnEndLevelLoseClick);
        //Tutorials
        tutorial1.onClick.AddListener(OnTutorial1Click);
        tutorial2.onClick.AddListener(OnTutorial1Click);
        tutorial3.onClick.AddListener(OnTutorial1Click);
        tutorial4.onClick.AddListener(OnTutorial1Click);
        tutorial5.onClick.AddListener(OnTutorial1Click);
        tutorial6.onClick.AddListener(OnTutorial1Click);
    }
    #endregion

    #region Menus
    private void OnMainMenuClick() {
        CastGlobalEventOpenUI(EUIType.MainMenu);
    }

    private void OnCreditsClick() {
        CastGlobalEventOpenUI(EUIType.CreditsMenu);
    }

    private void OnGameplayClick() {
        CastGlobalEventOpenUI(EUIType.GameplayHUD);
    }

    private void OnPauseClick() {
        CastGlobalEventOpenUI(EUIType.PauseMenu);
    }

    private void OnEndLevelWinClick() {
        CastGlobalEventOpenUI(EUIType.EndLevelWinMenu);
    }

    private void OnEndLevelLoseClick() {
        CastGlobalEventOpenUI(EUIType.EndLevelLoseMenu);
    }

    private void CastGlobalEventOpenUI(EUIType UIType) {
        Debug.Log("TestUI - Cast event " + EventName.OpenUI.ToString() + " with " + UIType.ToString());
        GlobalEventSystem.CastEvent(EventName.OpenUI, EventArgsFactory.OpenUIFactory(UIType));
    }
    #endregion

    #region Tutorials
    private void OnTutorial1Click() {
        InvokeOnStartToTriggerTutorial(1);
    }

    private void OnTutorial2Click() {
        InvokeOnStartToTriggerTutorial(2);
    }

    private void OnTutorial3Click() {
        InvokeOnStartToTriggerTutorial(3);
    }

    private void OnTutorial4Click() {
        InvokeOnStartToTriggerTutorial(5);
    }

    private void OnTutorial5Click() {
        InvokeOnStartToTriggerTutorial(7);
    }

    private void OnTutorial6Click() {
        InvokeOnStartToTriggerTutorial(10);
    }

    private void InvokeOnStartToTriggerTutorial(uint level) {
        LevelManager.Get().Level = level;
        LevelManager.Get().OnStart.Invoke();
    }
    #endregion
}
