using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileManagement : MonoBehaviour
{
    string current_name = "Filename";
    int[] map_data = new int[2];

    void Start()
    {
        SaveFile();
        LoadFile();
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        MapData data = new MapData(current_name, map_data);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        MapData data = (MapData)bf.Deserialize(file);
        file.Close();

        current_name = data.name;
        map_data = data.map;

        Debug.Log(data.name);
        Debug.Log(data.map);
    }

}
