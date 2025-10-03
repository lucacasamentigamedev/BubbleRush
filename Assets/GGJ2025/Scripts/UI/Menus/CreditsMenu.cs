using UnityEngine;

public class CreditsMenu : BaseUI {
    [SerializeField]
    private GameObject oldCredits;
    [SerializeField]
    private GameObject newCredits;
    [SerializeField]
    private GameObject oldCreditsButton;
    [SerializeField]
    private GameObject newCreditsButton;


    private bool isOldCredits;

    private void OnEnable() {
        //AudioManager.PlayOneShotSound("MenuOpen");
        oldCredits.SetActive(false);
        oldCreditsButton.SetActive(true);
        newCredits.SetActive(true);
        newCreditsButton.SetActive(false);
        isOldCredits = false;
    }

    private void OnDisable() {
        //AudioManager.PlayOneShotSound("MenuClose");
    }

    public void ChangeCredits()
    {
        if(isOldCredits)
        {
            oldCredits.SetActive(false);
            oldCreditsButton.SetActive(true);
            newCredits.SetActive(true);
            newCreditsButton.SetActive(false);
        }
        else
        {
            oldCredits.SetActive(true);
            oldCreditsButton.SetActive(false);
            newCredits.SetActive(false);
            newCreditsButton.SetActive(true);
        }
        isOldCredits = !isOldCredits;
    }
}