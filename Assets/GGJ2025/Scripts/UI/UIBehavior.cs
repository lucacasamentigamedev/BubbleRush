using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehavior : MonoBehaviour
{
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

    #region Mono
    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        backToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        gameplayMenu.SetActive(false);
        pauseMenu.SetActive(false);
        endMenu.SetActive(false);
    }

    private void Update() {
        if (InputManager.Player.TogglePause.WasPressedThisFrame()) {
            Debug.Log("Received input action TogglePause");
            TogglePause();
        }
    }
    #endregion

    #region onButtonCLick
    private void OnPlayButtonClick() {
        Debug.Log("UIBehavior - onPlayButtonClick");
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        canGoInPause = true;
    }

    private void OnBackToMainMenuButtonClick() {
        Debug.Log("UIBehavior - onBackToMainMenuButton");
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1f;
    }

    private void OnQuitButtonClick() {
        Debug.Log("UIBehavior - UIBehavior");
        Application.Quit();
    }

    private void OnCloseButtonClick() {
        Debug.Log("UIBehavior - onCloseButton");
        TogglePause();
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ClosePauseMenu() {
        Debug.Log("UIBehavior - ClosePauseMenu");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    #endregion
}