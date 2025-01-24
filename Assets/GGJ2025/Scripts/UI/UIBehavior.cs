using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehavior : MonoBehaviour {
    #region Button references
    //buttons
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button backToMainMenuButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button backToMainMenuButtonEnd;
    [SerializeField]
    private Button retryLevelButton;
    #endregion

    #region Menu references
    //menus
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject gameplayMenu;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject endMenu;
    #endregion

    #region Internal variables
    bool isPause = false;
    bool canGoInPause = false;
    #endregion

    #region Weapon
    [SerializeField]
    private RectTransform currentWeaponRectElem;
    #endregion

    #region Mono
    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        backToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        //recycle other function callback for this two
        backToMainMenuButtonEnd.onClick.AddListener(OnBackToMainMenuButtonClick);
        retryLevelButton.onClick.AddListener(OnRetryLevel);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        gameplayMenu.SetActive(false);
        pauseMenu.SetActive(false);
        endMenu.SetActive(false);
    }

    //fabio non voleva, però io l'ho fatto lo stesso. By Fabri :)
    private void Update() {
        if (InputManager.Player.TogglePause.WasPressedThisFrame() ||
            InputManager.Menu.TogglePause.WasPressedThisFrame()) {
            Debug.Log("Received input action TogglePause");
            TogglePause();
        }
    }
    #endregion

    #region other
    public void OnButtonFocus() {
        AudioManager.PlayOneShotSound("MenuSelect");
    }
    #endregion

    #region onButtonCLick
    private void OnPlayButtonClick() {
        Debug.Log("UIBehavior - onPlayButtonClick");
        AudioManager.PlayOneShotSound("MenuConfirm");
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        canGoInPause = true;
    }

    private void OnRetryLevel() {
        Debug.Log("UIBehavior - onPlayButtonClick");
        AudioManager.PlayOneShotSound("MenuConfirm");
        endMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        currentWeaponRectElem.gameObject.SetActive(true);
        //Cursor.visible = false;
        InputManager.Player.Enable();
        InputManager.Menu.Disable();
        canGoInPause = true;
    }

    private void OnBackToMainMenuButtonClick() {
        Debug.Log("UIBehavior - onBackToMainMenuButton");
        AudioManager.PlayOneShotSound("MenuConfirm");
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1f;
    }

    private void OnQuitButtonClick() {
        Debug.Log("UIBehavior - UIBehavior");
        AudioManager.PlayOneShotSound("MenuConfirm");
        Application.Quit();
    }

    private void OnCloseButtonClick() {
        Debug.Log("UIBehavior - onCloseButton");
        AudioManager.PlayOneShotSound("MenuConfirm");
        TogglePause();
    }
    #endregion

    #region endlevel
    public void OpenEndLevelMenu()
    {
        Debug.Log("UIBehavior - openPauseMenu");
        InputManager.Player.Disable();
        InputManager.Menu.Enable();
        currentWeaponRectElem.gameObject.SetActive(false);
        //Cursor.visible = true;
        endMenu.SetActive(true);
        gameplayMenu.SetActive(false);
        canGoInPause = false;
    }
    #endregion

    #region Pause
    private void TogglePause()
    {
        Debug.Log("UIBehavior - TogglePause");
        if (!canGoInPause) return;
        isPause = !isPause;
        if (isPause) {
            OpenPauseMenu();
        } else {
            ClosePauseMenu();
        }
    }

    private void OpenPauseMenu() {
        Debug.Log("UIBehavior - openPauseMenu");
        AudioManager.PlayOneShotSound("MenuOpen");
        pauseMenu.SetActive(true);
        currentWeaponRectElem.gameObject.SetActive(false);
        //Cursor.visible = true;
        InputManager.Menu.Enable();
        InputManager.Player.Disable();
        Time.timeScale = 0f;
    }

    private void ClosePauseMenu() {
        AudioManager.PlayOneShotSound("MenuClose");
        Debug.Log("UIBehavior - ClosePauseMenu");
        pauseMenu.SetActive(false);
        currentWeaponRectElem.gameObject.SetActive(true);
        //Cursor.visible = false;
        InputManager.Menu.Disable();
        InputManager.Player.Enable();
        Time.timeScale = 1f;
    }
    #endregion
}