using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

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
    [SerializeField]
    private Button nextLevelButton;
    [SerializeField]
    private Button backToMainMenuButtonPreNewLevel;
    [SerializeField]
    private TextMeshProUGUI textMeshProText;
    [SerializeField]
    private Button creditsButton;
    [SerializeField]
    private Button exitFromCreditsButton;
    [SerializeField]
    private Button deleteSaveButton;
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
    [SerializeField]
    private GameObject preNextLevelMenu;
    [SerializeField]
    private GameObject creditsMenu;
    #endregion

    #region Internal variables
    bool isPause = false;
    bool canGoInPause = false;
    #endregion

    #region Weapon
    [SerializeField]
    private RectTransform currentWeaponRectElem;
    #endregion

    #region stars
    [SerializeField]
    private GameObject[] starsUI;
    #endregion

    #region Mono
    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        backToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        //recycle other function callback for this two
        backToMainMenuButtonEnd.onClick.AddListener(OnBackToMainMenuButtonClick);
        backToMainMenuButtonPreNewLevel.onClick.AddListener(OnBackToMainMenuButtonClick);
        retryLevelButton.onClick.AddListener(OnRetryLevel);
        closeButton.onClick.AddListener(OnCloseButtonClick);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
        creditsButton.onClick.AddListener(OnCreditsButtonClick);
        exitFromCreditsButton.onClick.AddListener(OnExitFromCreditsButtonClick);
        deleteSaveButton.onClick.AddListener(OnDeleteSaveButtonClick);
    }

    private void OnDeleteSaveButtonClick() {
        AudioManager.PlayOneShotSound("MenuConfirm");
        LevelManager.Get().Level = SaveSystem.RemoveFile();
    }

    private void OnExitFromCreditsButtonClick() {
        AudioManager.PlayOneShotSound("MenuClose");
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void OnCreditsButtonClick() {
        AudioManager.PlayOneShotSound("MenuOpen");
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    private void Start()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        gameplayMenu.SetActive(false);
        pauseMenu.SetActive(false);
        endMenu.SetActive(false);
        preNextLevelMenu.SetActive(false);
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
        LevelManager.Get().RetryLevel();
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

    private void OnNextLevelButtonClick()
    {
        Debug.Log("UIBehavior - OnNextLevelButtonClick");
        currentWeaponRectElem.gameObject.SetActive(true);
        preNextLevelMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        InputManager.Player.Enable();
        InputManager.Menu.Disable();
        LevelManager.Get().StartGame();
        canGoInPause = true;
        Time.timeScale = 1f;
    }
    #endregion

    #region Level
    public void ChangeLevelLabel()
    {
        textMeshProText.text = "Level " + LevelManager.Get().Level;
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

    #region preNextLevel
    public void OnpePreLevelMenu(int stars=0)
    {
        for(int i =0; i< starsUI.Length; i++)
        {
            starsUI[i].SetActive(false);
        }
        Debug.Log("UIBehavior - OnpePreLevelMenu");
        InputManager.Player.Disable();
        InputManager.Menu.Enable();
        preNextLevelMenu.SetActive(true);
        gameplayMenu.SetActive(false);
        canGoInPause = false;
        Time.timeScale = 0f;
        currentWeaponRectElem.gameObject.SetActive(false);
        for(int i = 1; i <= stars; i++)
        {
            starsUI[i-1].SetActive(true);

        }
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