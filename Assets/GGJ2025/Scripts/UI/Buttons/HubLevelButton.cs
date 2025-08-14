using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUBLevelButton : MonoBehaviour {

    [SerializeField]
    private uint levelIndex;

    private Button button;
    private TextMeshProUGUI btnText;
    private Image buttonImage;
    private Sprite[] levelToDoSprites;
    private Sprite levelFinishSprite;
    uint currentLevel;

    private void Awake() {
        levelToDoSprites = new Sprite[5];
        levelToDoSprites[0] = Resources.Load<Sprite>("Sprites/Bubbles/Normal/bubble_normal_damage0_without_background");
        levelToDoSprites[1] = Resources.Load<Sprite>("Sprites/Bubbles/Normal/bubble_normal_damage1_without_background");
        levelToDoSprites[2] = Resources.Load<Sprite>("Sprites/Bubbles/Normal/bubble_normal_damage2_without_background");
        levelToDoSprites[3] = Resources.Load<Sprite>("Sprites/Bubbles/Normal/bubble_normal_damage3_without_background");
        levelToDoSprites[4] = Resources.Load<Sprite>("Sprites/Bubbles/Normal/bubble_normal_damage4_without_background");
        levelFinishSprite = Resources.Load<Sprite>($"Sprites/Bubbles/Popped/bubble_popped_whitout_background");
        btnText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        button.onClick.AddListener(OnClick);
    }

    void OnEnable()
    {
        currentLevel = LevelManager.Get().ReachedLevel;

        //image
        if (currentLevel > levelIndex) {
            //Debug.Log($"HUBLevelButton {levelIndex} unlocked");
            buttonImage.sprite = levelFinishSprite;
            if (ColorUtility.TryParseHtmlString("#BEBEBE", out Color parsedColor)) {
                btnText.color = parsedColor;
            }
        } else{
            //Debug.Log($"HUBLevelButton {levelIndex} locked!!!");
            int randomIndex = Random.Range(0, levelToDoSprites.Length);
            buttonImage.sprite = levelToDoSprites[randomIndex];
        }

        //level number
        btnText.text = levelIndex.ToString();
    }

    private void OnClick() {
        if (currentLevel >= levelIndex) {
            AudioManager.PlayOneShotSound("BubblePop", new FMODParameter[] {
                    new FMODParameter("BUBBLE_POP_TYPE", 0.0f)
                });
            LevelManager.Get().StartLevel(levelIndex);
            //AudioManager.PlayBackgroundMusic("GameplayMusic");
        } else {
            AudioManager.PlayOneShotSound("BubbleSimpleCLick");
        }
    }
}
