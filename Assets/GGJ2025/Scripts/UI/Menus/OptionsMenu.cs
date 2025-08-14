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
        masterSlider.value = AudioManager.GetRawVolume(EAudioCategory.Master);
        bubblesSlider.value = AudioManager.GetRawVolume(EAudioCategory.Bubbles);
        toolsSlider.value = AudioManager.GetRawVolume(EAudioCategory.Tools);
        otherSlider.value = AudioManager.GetRawVolume(EAudioCategory.Other);
        musicSlider.value = AudioManager.GetRawVolume(EAudioCategory.Music);
        UISlider.value = AudioManager.GetRawVolume(EAudioCategory.UI);
        applyButton.onClick.AddListener(OnApplyButtonClicked);
    }

    private void OnDisable() {
        AudioManager.PlayOneShotSound("MenuClose");
        applyButton.onClick.RemoveListener(OnApplyButtonClicked);
    }

    private void OnApplyButtonClicked() {
        Debug.Log("OptionsMenu - Apply button clicked");
        AudioManager.SetRawVolume(EAudioCategory.Master, masterSlider.value);
        AudioManager.SetRawVolume(EAudioCategory.Bubbles, bubblesSlider.value);
        AudioManager.SetRawVolume(EAudioCategory.Tools, toolsSlider.value);
        AudioManager.SetRawVolume(EAudioCategory.Other, otherSlider.value);
        AudioManager.SetRawVolume(EAudioCategory.Music, musicSlider.value);
        AudioManager.SetRawVolume(EAudioCategory.UI, UISlider.value);
        Debug.Log("OptionsMenu - Ora chiamo il SaveFile");
        SaveSystem.SaveFile(LevelManager.Get().ReachedLevel, AudioManager.GetAllRawVolumes());
    }
}