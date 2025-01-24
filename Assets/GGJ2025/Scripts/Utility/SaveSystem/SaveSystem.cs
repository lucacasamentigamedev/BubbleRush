using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveFile(uint level)
    {
        string destination = Application.persistentDataPath + "/save.fish";
        FileStream file;

        if(File.Exists(destination))
            file = File.OpenWrite(destination);
        else
            file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, level);
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
            Debug.LogWarning("File save.fish not found");
            level = 1;
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        level = (uint)bf.Deserialize(file);
        file.Close();
    }
}
