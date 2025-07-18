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
        string path = Application.persistentDataPath + "/save.fish";
        Debug.Log("SaveSystem - Save file at destination " + path);
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
        } catch (Exception e) {
            Debug.LogError("SaveSystem - Failed to save file");
            Debug.LogException(e);
        }
    }

    public static void LoadLevel(out uint level) {
        level = 1;
        string path = Application.persistentDataPath + "/save.fish";
        if (!File.Exists(path)) return;
        try {
            using FileStream file = File.OpenRead(path);
            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData)bf.Deserialize(file);
            level = data.level;
        } catch (Exception e) {
            Debug.LogError("SaveSystem - Failed to load level");
            Debug.LogException(e);
        }
    }

    public static uint RemoveFile()
    {
        string destination = Application.persistentDataPath + "/save.json";
        if (File.Exists(destination))
        {
            File.Delete(destination);
            Debug.Log("SaveSystem - Delete save");
        } else {
            Debug.Log("SaveSystem - Nothing to delete");
        }
        return 1;
    }
}