using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SaveData {
    public uint level;
    public Dictionary<AudioCategory, float> volumes = new();
}

public static class SaveSystem
{
    public static void SaveFile(uint level) {
        string destination = Application.persistentDataPath + "/save.fish";
        FileStream file;

        try {
            file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
        } catch (Exception e) {
            Debug.LogException(e);
            return;
        }

        SaveData data = new SaveData();
        data.level = level;
        data.volumes = new Dictionary<AudioCategory, float>(AudioManager.GetAllVolumes());

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public static void LoadFile(out uint level)
    {
        string destination = Application.persistentDataPath + "/save.fish";
        FileStream file;

        if (File.Exists(destination))
            file = File.OpenRead(destination);
        else
        {
            Debug.Log("File save.fish not found");
            level = 1;
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        level = (uint)bf.Deserialize(file);
        file.Close();
    }

    public static uint RemoveFile()
    {
        string destination = Application.persistentDataPath + "/save.fish";
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
