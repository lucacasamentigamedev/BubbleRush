using UnityEngine;
using UnityEngine.UI;

public class EndlessModeButton : MonoBehaviour
{
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

   
    private void OnClick()
    {
        
        AudioManager.PlayOneShotSound("BubblePop", new FMODParameter[] {
                new FMODParameter("BUBBLE_POP_TYPE", 0.0f)
            });
        LevelManager.Get().StartEndlessMode();
        //AudioManager.PlayBackgroundMusic("GameplayMusic");
       
    }
}
