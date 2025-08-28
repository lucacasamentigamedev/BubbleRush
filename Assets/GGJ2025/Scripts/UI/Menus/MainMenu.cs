using TMPro;
using UnityEngine;

public class MainMenu : BaseUI 
{
    [SerializeField]
    private TextMeshProUGUI levelReachedText;

    public void OnEnable()
    {
        levelReachedText.text = "Level Reached: " + LevelManager.Get().EndlessReachedLevel;
    }
}