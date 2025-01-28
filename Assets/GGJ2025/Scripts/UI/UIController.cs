using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Menu Prefabs")]
    [SerializeField] private GameObject mainMenuPrefab;
    [SerializeField] private GameObject creditsPrefab;
    [SerializeField] private GameObject gameplayPrefab;
    [SerializeField] private GameObject pausePrefab;
    [SerializeField] private GameObject endLevelWinPrefab;
    [SerializeField] private GameObject endLevelLosePrefab;

    private GameObject currentMenu;

    private void Start()
    {
        //on start Main scene open MainMenu
        OpenMenu(EUIType.MainMenu);
    }

    public void OpenMenu(EUIType UIType)
    {
        CloseCurrentMenu();
        switch (UIType)
        {
            case EUIType.MainMenu:
                currentMenu = mainMenuPrefab;
                break;
            case EUIType.Credits:
                currentMenu = creditsPrefab;
                break;
            case EUIType.Gameplay:
                currentMenu = gameplayPrefab;
                break;
            case EUIType.Pause:
                currentMenu = pausePrefab;
                break;
            case EUIType.EndLevelWin:
                currentMenu = endLevelWinPrefab;
                break;
            case EUIType.EndLevelLose:
                currentMenu = endLevelLosePrefab;
                break;
        }

        if (currentMenu != null)
        {
            currentMenu.SetActive(true);
        } else
        {
            Debug.LogError("UIController: Menu to open not found" + UIType.ToString());
        }
    }

    public void CloseCurrentMenu()
    {
        if (currentMenu != null)
        {
            currentMenu.SetActive(false);
            currentMenu = null;
        }
        else
        {
            Debug.LogError("UIController: U are trying to close a null menu");
        }
    }
}
