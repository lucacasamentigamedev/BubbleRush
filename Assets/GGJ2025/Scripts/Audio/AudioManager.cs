using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

public struct FMODParameter {
    public string Name { get; set; }
    public float Value { get; set; }

    public FMODParameter(string name, float value) {
        Name = name;
        Value = value;
    }
}

public static class AudioManager
{
    private static EventInstance currentBackgroundMusic;

    private static readonly Dictionary<string, AudioEvent> soundDictionary = new Dictionary<string, AudioEvent> {
        { "Test", new AudioEvent("event:/Test/Test", AudioCategory.Test) },
        { "BubblePop", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_POP", AudioCategory.Bubbles) },
        { "BubbleTool", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_TOOL", AudioCategory.Tools) },
        { "BubbleToolChange", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_TOOL_CHANGE", AudioCategory.Tools) },
        { "WinLose", new AudioEvent("event:/ACTION/WIN_LOSE", AudioCategory.Other) },
        { "Conveyor", new AudioEvent("event:/SCENE/CONVEYOR", AudioCategory.Other) },
        { "MenuOpen", new AudioEvent("event:/MENU/MENU_OPEN", AudioCategory.UI) },
        { "MenuClose", new AudioEvent("event:/MENU/MENU_CLOSE", AudioCategory.UI) },
        { "MenuConfirm", new AudioEvent("event:/MENU/MENU_CONFIRM", AudioCategory.UI) },
        { "MenuSelect", new AudioEvent("event:/MENU/MENU_SELECT", AudioCategory.UI) }
    };

    private static readonly Dictionary<AudioCategory, float> volumes = new Dictionary<AudioCategory, float> {
        { AudioCategory.Bubbles, 1.0f },
        { AudioCategory.Tools, 1.0f },
        { AudioCategory.Other, 1.0f },
        { AudioCategory.Music, 1.0f },
        { AudioCategory.UI, 1.0f },
        { AudioCategory.Test, 1.0f }
    };

    public static void SetCategoryVolume(AudioCategory category, float volume) {
        if (volumes.ContainsKey(category)) {
            Debug.Log($"Set volume '{volume}' at '{category}'");
            volumes[category] = Mathf.Clamp01(volume);
        }
    }

    public static void PlayOneShotSound(string soundName, FMODParameter[] parameters = null) {
        // check if event exists
        if (soundDictionary.TryGetValue(soundName, out AudioEvent audioEvent)) {
            // get volume
            float categoryVolume = volumes.GetValueOrDefault(audioEvent.Category, 1.0f);
            var instance = audioEvent.CreateInstance();
            // Set FMOD parameters if provided
            if (parameters != null && parameters.Length > 0) {
                foreach (var param in parameters) {
                    instance.setParameterByName(param.Name, param.Value);
                }
            }
            instance.setVolume(categoryVolume);
            // play sound
            //Debug.Log($"Play sound '{soundName}' at volume '{categoryVolume}'");
            instance.start();
            instance.release();
        } else {
            Debug.LogWarning($"Sound '{soundName}' not found in the dictionary");
        }
    }

    public static void PlayBackgroundMusic(string soundPath) {
        // stop actual bg
        if (currentBackgroundMusic.isValid()) {
            currentBackgroundMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        // get event
        if (soundDictionary.TryGetValue(soundPath, out AudioEvent audioEvent)) {
            // get volume
            float categoryVolume = volumes.GetValueOrDefault(audioEvent.Category, 1.0f);
            // play
            currentBackgroundMusic = audioEvent.CreateInstance();
            currentBackgroundMusic.setVolume(categoryVolume);
            currentBackgroundMusic.start();
        }
    }
}