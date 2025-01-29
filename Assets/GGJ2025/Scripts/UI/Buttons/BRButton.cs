using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BRButton : MonoBehaviour {

    #region variables
    public ButtonType buttonType;
    private Button button;
    private Image buttonImage;
    private const string SpriteBasePath = "Sprites/Buttons/";
    public Sprite defaultSprite;
    public Sprite highlightedSprite;
    protected UIController UIController;
    private static readonly Dictionary<ButtonType, string> buttonTypeToFileName = new Dictionary<ButtonType, string>
    {
        { ButtonType.Play, "play" },
        { ButtonType.Credits, "credits" },
        { ButtonType.DeleteSave, "deletesave" },
        { ButtonType.Quit, "quit" },
        { ButtonType.Resume, "resume" },
        { ButtonType.MainMenu, "mainmenu" },
        { ButtonType.NextLevel, "nextlevel" },
        { ButtonType.Retry, "retry" },
        { ButtonType.Close, "close" }
    };
    #endregion

    #region Mono
    private void Awake() {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        button.onClick.AddListener(OnClick);
        LoadButtonSprites();
        SetupButton();
    }
    #endregion

    #region Internal Methods
    public void Init(UIController UIController) {
        this.UIController = UIController;
    }

    private void LoadButtonSprites() {
        if (!buttonTypeToFileName.TryGetValue(buttonType, out string buttonFileName)) {
            Debug.LogError($"BRButton - No entry found for button type {buttonType}.");
            return;
        }
        // load sprites
        defaultSprite = Resources.Load<Sprite>($"{SpriteBasePath}{buttonFileName}_unfocus");
        highlightedSprite = Resources.Load<Sprite>($"{SpriteBasePath}{buttonFileName}_focus");
        if (defaultSprite == null || highlightedSprite == null) {
            Debug.LogError($"BRButton - Missing sprite(s) for button type {buttonType}. Ensure paths and names are correct.");
            return;
        }
    }

    private void SetupButton() {
        buttonImage.sprite = defaultSprite;
    }
    #endregion

    #region Button behavior
    public void OnMouseEnter() {
        AudioManager.PlayOneShotSound("MenuSelect");
        buttonImage.sprite = highlightedSprite;
    }

    public void OnMouseExit() {
        buttonImage.sprite = defaultSprite;
    }

    protected virtual void OnClick() {
        buttonImage.sprite = defaultSprite;
        AudioManager.PlayOneShotSound("MenuConfirm");
    }
    #endregion
}
