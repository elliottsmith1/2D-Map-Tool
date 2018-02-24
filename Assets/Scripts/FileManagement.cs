using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class FileManagement 
{
    public static void SaveFile(MapData _map, String _file_name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + _file_name + ".dat", FileMode.Create);

        Map data = new Map(_map);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static int[] LoadMap(String _file_name)
    {
        if (File.Exists(Application.persistentDataPath + "/map.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + _file_name + ".dat", FileMode.Open);

            Map data = bf.Deserialize(stream) as Map;

            stream.Close();
            return data.map;
        }

        else
        {
            Debug.LogError("File not found");
            return null;
        }
    }

    public static int LoadMapRooms(String _file_name)
    {
        if (File.Exists(Application.persistentDataPath + "/" + _file_name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + _file_name + ".dat", FileMode.Open);

            Map data = bf.Deserialize(stream) as Map;

            stream.Close();
            return data.rooms;
        }

        else
        {
            Debug.LogError("File not found");
            return 0;
        }
    }

    public static int LoadMapSize(String _file_name)
    {
        if (File.Exists(Application.persistentDataPath + "/" + _file_name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + _file_name + ".dat", FileMode.Open);

            Map data = bf.Deserialize(stream) as Map;

            stream.Close();
            return data.grid_size;
        }

        else
        {
            Debug.LogError("File not found");
            return 0;
        }
    }

    [Serializable]
    public class Map
    {
        public string name;
        public int[] map;
        public int rooms = 0;
        public int grid_size = 0;

        public Map(MapData _map)
        {
            name = _map.name;
            map = _map.map;
            rooms = _map.rooms;
            grid_size = _map.grid_size;
        }
    }    
}
