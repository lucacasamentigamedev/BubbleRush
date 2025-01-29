using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : BRButton {
    protected override void OnClick() {
        base.OnClick();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1f;
    }
}