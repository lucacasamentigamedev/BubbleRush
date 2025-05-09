using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : BRButton {
    protected override void OnClick() {
        if (UIController.isPrevented) return;
        base.OnClick();
        StopAllCoroutines();
        string currentSceneName = SceneManager.GetActiveScene().name;
        UIController.OpenMenu(EUIType.MainMenu);
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1f;
    }
}