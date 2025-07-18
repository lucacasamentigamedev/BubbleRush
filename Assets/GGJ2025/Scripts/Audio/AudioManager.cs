using FMOD.Studio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        { "Test", new AudioEvent("event:/Test/Test", EAudioCategory.Test) },
        { "TestLoop", new AudioEvent("event:/Test/TestLoop", EAudioCategory.Test) },
        { "BubblePop", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_POP", EAudioCategory.Bubbles) },
        { "BubbleTool", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_TOOL", EAudioCategory.Tools) },
        { "BubbleToolChange", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_TOOL_CHANGE", EAudioCategory.Tools) },
        { "WinLose", new AudioEvent("event:/ACTION/WIN_LOSE", EAudioCategory.Other) },
        { "Conveyor", new AudioEvent("event:/SCENE/CONVEYOR", EAudioCategory.Other) },
        { "MainMenuMusic", new AudioEvent("event:/SCENE/MUSIC_MAINMENU", EAudioCategory.Music) },
        { "IntroMusic", new AudioEvent("event:/SCENE/MUSIC_INTRO", EAudioCategory.Music) },
        { "MenuOpen", new AudioEvent("event:/MENU/MENU_OPEN", EAudioCategory.UI) },
        { "MenuClose", new AudioEvent("event:/MENU/MENU_CLOSE", EAudioCategory.UI) },
        { "MenuConfirm", new AudioEvent("event:/MENU/MENU_CONFIRM", EAudioCategory.UI) },
        { "MenuSelect", new AudioEvent("event:/MENU/MENU_SELECT", EAudioCategory.UI) },
        { "BubbleBombExplode", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_MALUS", EAudioCategory.Bubbles) },
        { "BubbleBombBeep", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_MALUS_BOMB_BEEP", EAudioCategory.Bubbles) },
        { "TimeEndBeep", new AudioEvent("event:/ACTION/TIMEEND", EAudioCategory.Other) },
        { "TutorialMessagePop", new AudioEvent("event:/SCENE/TUTORIAL_MESAGE", EAudioCategory.UI) },
        { "GameplayMusic", new AudioEvent("event:/ACTION/MUSIC", EAudioCategory.Music) },
        { "BubbleSimpleCLick", new AudioEvent("event:/ACTION/BUBBLE/BUBBLE_TOOL_SIMPLECLICK", EAudioCategory.Bubbles) }
    };

    private static readonly Dictionary<EAudioCategory, float> volumes;
    /* = new Dictionary<EAudioCategory, float> {
        { EAudioCategory.Master, 1.0f },
        { EAudioCategory.Bubbles, 1.0f },
        { EAudioCategory.Tools, 1.0f },
        { EAudioCategory.Other, 1.0f },
        { EAudioCategory.Music, 1.0f },
        { EAudioCategory.UI, 1.0f },
        { EAudioCategory.Test, 1.0f }
    };*/

    /*static AudioManager() {
        Debug.Log("AudioManager - static constructor called");
        string destination = Application.persistentDataPath + "/save.fish";
        FileStream file;
        if (File.Exists(destination))
            file = File.OpenRead(destination);
        else {
            //default volumes
            Debug.Log("AudioManager - Save file not found default volumes 1");
            volumes = new Dictionary<EAudioCategory, float> {
                { EAudioCategory.Master, 1.0f },
                { EAudioCategory.Bubbles, 1.0f },
                { EAudioCategory.Tools, 1.0f },
                { EAudioCategory.Other, 1.0f },
                { EAudioCategory.Music, 1.0f },
                { EAudioCategory.UI, 1.0f },
                { EAudioCategory.Test, 1.0f }
            };
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        SaveData data = (SaveData)bf.Deserialize(file);
        Debug.Log("AudioManager - Save file found, set volumes from save into dictionary");
        volumes = data.volumes;
        file.Close();
    }*/

    static AudioManager() {
        Debug.Log("AudioManager - static constructor called");

        string path = Application.persistentDataPath + "/save.json";

        if (!File.Exists(path)) {
            Debug.Log("AudioManager - Save file not found, using default volumes");
            volumes = GetDefaultVolumes();
            return;
        }

        try {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            volumes = new Dictionary<EAudioCategory, float>();

            foreach (var entry in data.volumes) {
                volumes[entry.category] = Mathf.Clamp01(entry.value);
            }

            Debug.Log($"AudioManager - Loaded volumes from save: {volumes.Count} entries");
        } catch (Exception e) {
            Debug.LogError("AudioManager - Failed to load volumes, using default");
            Debug.LogException(e);
            volumes = GetDefaultVolumes();
        }
    }

    private static Dictionary<EAudioCategory, float> GetDefaultVolumes() {
        return new Dictionary<EAudioCategory, float> {
            { EAudioCategory.Master, 1.0f },
            { EAudioCategory.Bubbles, 1.0f },
            { EAudioCategory.Tools, 1.0f },
            { EAudioCategory.Other, 1.0f },
            { EAudioCategory.Music, 1.0f },
            { EAudioCategory.UI, 1.0f },
            { EAudioCategory.Test, 1.0f }
        };
    }


    private static float GetEffectiveVolume(EAudioCategory category) {
        float masterVolume = volumes.GetValueOrDefault(EAudioCategory.Master, 1.0f);
        float categoryVolume = volumes.GetValueOrDefault(category, 1.0f);
        return masterVolume * categoryVolume;
    }

    public static void SetCategoryVolume(EAudioCategory category, float volume) {
        if (volumes.ContainsKey(category)) {
            volumes[category] = Mathf.Clamp01(volume);
        }
    }

    public static void PlayOneShotSound(string soundName, FMODParameter[] parameters = null) {
        // check if event exists
        if (soundDictionary.TryGetValue(soundName, out AudioEvent audioEvent)) {
            // get volume
            float categoryVolume = GetEffectiveVolume(audioEvent.Category);
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
            currentBackgroundMusic.stop(STOP_MODE.ALLOWFADEOUT);
        }
        // get event
        if (soundDictionary.TryGetValue(soundPath, out AudioEvent audioEvent)) {
            // get volume
            float categoryVolume = GetEffectiveVolume(audioEvent.Category);
            // play
            currentBackgroundMusic = audioEvent.CreateInstance();
            currentBackgroundMusic.setVolume(categoryVolume);
            currentBackgroundMusic.start();
        }
    }

    public static void PauseBackgroundMusic() {
        if (currentBackgroundMusic.isValid()) {
            currentBackgroundMusic.setPaused(true); // Pausa l'istanza
        } else {
            Debug.LogWarning("No valid background music to pause.");
        }
    }

    public static void ResumeBackgroundMusic() {
        if (currentBackgroundMusic.isValid()) {
            currentBackgroundMusic.setPaused(false); // Riprendi l'istanza
        } else {
            Debug.LogWarning("No valid background music to resume.");
        }
    }

    public static float GetRawVolume(EAudioCategory category) {
        return volumes.TryGetValue(category, out float v) ? v : 1.0f;
    }

    public static void SetRawVolume(EAudioCategory category, float value) {
        //Debug.Log($"Setting volume for {category} to {value}");
        volumes[category] = Mathf.Clamp01(value);
        //update background music volume if necessary
        if (category == EAudioCategory.Music || category == EAudioCategory.Master) {
            RefreshBackgroundMusicVolume();
        }
    }

    public static void RefreshBackgroundMusicVolume() {
        if (currentBackgroundMusic.isValid()) {
            float newVolume = GetEffectiveVolume(EAudioCategory.Music);
            currentBackgroundMusic.setVolume(newVolume);
            Debug.Log($"Updated background music volume to {newVolume}");
        }
    }

    public static Dictionary<EAudioCategory, float> GetAllRawVolumes() {
        return new Dictionary<EAudioCategory, float>(volumes);
    }
}