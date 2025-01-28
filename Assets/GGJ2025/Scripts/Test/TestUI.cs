using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{

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

    void Awake()
    {
        mainMenu.onClick.AddListener(OnMainMenuClick);
        credits.onClick.AddListener(OnCreditsClick);
        gameplay.onClick.AddListener(OnGameplayClick);
        pause.onClick.AddListener(OnPauseClick);
        endLevelWin.onClick.AddListener(OnEndLevelWinClick);
        endLevelLose.onClick.AddListener(OnEndLevelLoseClick);
    }

    private void OnMainMenuClick() {
        CastGlobalEventOpenUI(EUIType.MainMenu);
    }

    private void OnCreditsClick() {
        CastGlobalEventOpenUI(EUIType.CreditsMenu);
    }

    private void OnGameplayClick() {
        CastGlobalEventOpenUI(EUIType.GameplayMenu);
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
}
