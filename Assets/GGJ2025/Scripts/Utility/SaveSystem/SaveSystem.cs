using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/*[System.Serializable]
public class SaveData {
    public uint level;
    public Dictionary<EAudioCategory, float> volumes = new();
}*/

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
    /*public static void SaveFile(uint level, Dictionary<EAudioCategory, float> volumes) {
        if (level <= 0) level = 1;
        string destination = Application.persistentDataPath + "/save.json";
        Debug.Log("SaveSystem - Save file at destination " + destination);
        FileStream file;

        try {
            file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
        } catch (Exception e) {
            Debug.LogException(e);
            return;
        }

        SaveData data = new SaveData();
        data.level = level;
        //data.volumes = volumes;
        // 🔁 Converti il Dictionary in una lista serializzabile
        foreach (var kvp in volumes) {
            data.volumes.Add(new SerializableVolumeEntry {
                category = kvp.Key,
                value = kvp.Value
            });
        }

        // 💾 Serializza in JSON
        string json = JsonUtility.ToJson(data, prettyPrint: true);

        *//*BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);*//*

        try {
            File.WriteAllText(destination, json);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        file.Close();
    }*/

    public static void SaveFile(uint level, Dictionary<EAudioCategory, float> volumes) {
        if (level <= 0) level = 1;
        string destination = Application.persistentDataPath + "/save.json";
        Debug.Log("SaveSystem - Save file at destination " + destination);

        SaveData data = new SaveData();
        data.level = level;

        // 🔁 Converti il Dictionary in una lista serializzabile
        foreach (var kvp in volumes) {
            data.volumes.Add(new SerializableVolumeEntry {
                category = kvp.Key,
                value = kvp.Value
            });
        }

        // 💾 Serializza in JSON
        string json = JsonUtility.ToJson(data, prettyPrint: true);

        try {
            File.WriteAllText(destination, json);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }


    /*public static void LoadLevel(out uint level)
    {
        string destination = Application.persistentDataPath + "/save.json";
        Debug.Log("SaveSystem - Load level at destination " + destination);
        FileStream file;

        if (File.Exists(destination))
            file = File.OpenRead(destination);
        else
        {
            Debug.Log("File save.json not found");
            level = 1;
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SaveData data = (SaveData)bf.Deserialize(file);
        level = data.level;


        file.Close();
    }*/

    public static void LoadLevel(out uint level) {
        string destination = Application.persistentDataPath + "/save.json";
        Debug.Log("SaveSystem - Load level at destination " + destination);

        level = 1; // default fallback

        if (!File.Exists(destination)) {
            Debug.Log("File save.json not found");
            return;
        }

        try {
            string json = File.ReadAllText(destination);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            level = data.level;
            Debug.Log($"SaveSystem - Loaded level: {level}");
        } catch (Exception e) {
            Debug.LogError("SaveSystem - Error while loading level");
            Debug.LogException(e);
            level = 1;
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
