using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SerializableVolumeEntry {
    public EAudioCategory category;
    public float value;
}

[System.Serializable]
public class SerializableLevelScoreEntry {
    public uint levelId;
    public uint stars;
}

[System.Serializable]
public class SaveData {
    public uint level;
    public List<SerializableVolumeEntry> volumes = new();
    public List<SerializableLevelScoreEntry> levelScores = new();
    public uint endlessLevel;
}

public static class SaveSystem
{
    public static void SaveFile(uint level, Dictionary<EAudioCategory, float> volumesDict, Dictionary<uint, uint> levelScores, uint endlessLevel) {
        Debug.Log(
            "SaveFile level" + level +
            "Volumes: " + string.Join(", ", volumesDict.Select(kvp => $"{kvp.Key}:{kvp.Value}")) +
            "LevelScores: " + string.Join(", ", levelScores.Select(kvp => $"{kvp.Key}:{kvp.Value}"))
        );
        string path = Application.persistentDataPath + "/save.fish";
        SaveData data = new SaveData();
        //level reached
        data.level = level;
        //volumes
        foreach (var kvp in volumesDict) {
            data.volumes.Add(new SerializableVolumeEntry {
                category = kvp.Key,
                value = kvp.Value
            });
        }
        //level scores
        foreach (var lscore in levelScores) {
            data.levelScores.Add(new SerializableLevelScoreEntry {
                levelId = lscore.Key,
                stars = lscore.Value
            });
        }
        data.endlessLevel = endlessLevel;
        //save on file
        try {
            using FileStream file = File.Create(path);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            Debug.Log("SaveFile - Save OK");
        } catch (Exception e) {
            Debug.LogError("SaveFile - Failed to save file");
            Debug.LogException(e);
        }

    }

    public static void LoadLevel(out uint level) {
        string path = Application.persistentDataPath + "/save.fish";
        if (!File.Exists(path))
        {
            Debug.Log("LoadLevel - Save file does not exist, setting level to 1");
            level = 1;
            return;
        };
        try {
            using FileStream file = File.OpenRead(path);
            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData)bf.Deserialize(file);
            level = data.level;
            Debug.Log("LoadLevel - OK, loaded level = " + level);
        } catch (Exception e) {
            level = 1;
            Debug.LogError("SaveSystem - Failed to load level, level set to 1");
            Debug.LogException(e);
        }
    }

    public static void LoadLevelScores(out Dictionary<uint, uint> levelScores) {
        string path = Application.persistentDataPath + "/save.fish";
        levelScores = new Dictionary<uint, uint>();
        if (!File.Exists(path)) {
            Debug.Log("LoadLevelScores - Save file does not exist, returning empty dictionary");
            return;
        }
        try {
            using FileStream file = File.OpenRead(path);
            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData)bf.Deserialize(file);
            if (data.levelScores != null) {
                levelScores = data.levelScores
                    .ToDictionary(entry => entry.levelId, entry => entry.stars);
            }
            Debug.Log("LoadLevelScores - OK, loaded " + levelScores.Count + " entries");
            Debug.Log("LevelScores: " + string.Join(", ", levelScores.Select(kvp => $"{kvp.Key}:{kvp.Value}")));
        } catch (Exception e) {
            levelScores.Clear();
            Debug.LogError("SaveSystem - Failed to load level scores, returning empty dictionary");
            Debug.LogException(e);
        }
    }

    public static void LoadEndlessLevel(out uint endlessLevel)
    {
        string path = Application.persistentDataPath + "/save.fish";
        if (!File.Exists(path))
        {
            Debug.Log("LoadLevel - Save file does not exist, setting level to 1");
            endlessLevel = 0;
            return;
        };
        try
        {
            using FileStream file = File.OpenRead(path);
            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData)bf.Deserialize(file);
            endlessLevel = data.endlessLevel;
            Debug.Log("LoadLevel - OK, loaded level = " + endlessLevel);
        }
        catch (Exception e)
        {
            endlessLevel = 1;
            Debug.LogError("SaveSystem - Failed to load level, level set to 1");
            Debug.LogException(e);
        }
    }



    public static uint DeleteSave()
    {
        string destination = Application.persistentDataPath + "/save.fish";
        if (File.Exists(destination))
        {
            File.Delete(destination);
            Debug.Log("RemoveFile - Delete save");
        } else {
            Debug.Log("RemoveFile - Nothing to delete");
        }
        return 1;
    }
}