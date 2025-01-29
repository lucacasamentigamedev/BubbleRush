using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1f;
    }
}