using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SerializableVolumeEntry {
    public EAudioCategory category;
    public float value;
}

[System.Serializable]
public class SaveData {
    public uint level;
    public List<SerializableVolumeEntry> volumes = new();
}

public static class SaveSystem
{
    public static void SaveFile(uint level, Dictionary<EAudioCategory, float> volumesDict) {
        Debug.Log("SaveFile: level = " + level + ", volumesDict = " + volumesDict);
        string path = Application.persistentDataPath + "/save.fish";
        SaveData data = new SaveData();
        data.level = level;
        foreach (var kvp in volumesDict) {
            data.volumes.Add(new SerializableVolumeEntry {
                category = kvp.Key,
                value = kvp.Value
            });
        }
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