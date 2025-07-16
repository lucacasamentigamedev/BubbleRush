using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : BaseUI {

    //Menus
    [Header("Audio sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bubblesSlider;
    [SerializeField] private Slider toolsSlider;
    [SerializeField] private Slider otherSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider UISlider;
    [SerializeField] private Button applyButton;

    private void OnEnable() {
        AudioManager.PlayOneShotSound("MenuOpen");
        Debug.Log("OptionsMenu - OnEnable called");
        // Initialize sliders with current volume settings
        masterSlider.value = AudioManager.GetRawVolume(AudioCategory.Master);
        bubblesSlider.value = AudioManager.GetRawVolume(AudioCategory.Bubbles);
        toolsSlider.value = AudioManager.GetRawVolume(AudioCategory.Tools);
        otherSlider.value = AudioManager.GetRawVolume(AudioCategory.Other);
        musicSlider.value = AudioManager.GetRawVolume(AudioCategory.Music);
        UISlider.value = AudioManager.GetRawVolume(AudioCategory.UI);
        applyButton.onClick.AddListener(OnApplyButtonClicked);
    }

    private void OnDisable() {
        AudioManager.PlayOneShotSound("MenuClose");
        applyButton.onClick.RemoveListener(OnApplyButtonClicked);
    }

    private void OnApplyButtonClicked() {
        Debug.Log("OptionsMenu - Apply button clicked");
        AudioManager.SetRawVolume(AudioCategory.Master, masterSlider.value);
        AudioManager.SetRawVolume(AudioCategory.Bubbles, bubblesSlider.value);
        AudioManager.SetRawVolume(AudioCategory.Tools, toolsSlider.value);
        AudioManager.SetRawVolume(AudioCategory.Other, otherSlider.value);
        AudioManager.SetRawVolume(AudioCategory.Music, musicSlider.value);
        AudioManager.SetRawVolume(AudioCategory.UI, UISlider.value);
    }
}