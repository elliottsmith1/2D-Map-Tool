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
        //save new file with data
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + _file_name + ".dat", FileMode.Create);

        Map data = new Map(_map);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveFilesList(FilesList _files)
    {
        //save list of available files
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/FileList.dat", FileMode.Create);

        Files data = new Files(_files);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static List<string> LoadFilesList()
    {
        //load available files
        if (File.Exists(Application.persistentDataPath + "/FileList.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/FileList.dat", FileMode.Open);

            Files data = bf.Deserialize(stream) as Files;

            stream.Close();
            return data.file_names;
        }

        else
        {
            //files list not found
            return null;
        }
    }

    public static int[] LoadMap(String _file_name)
    {
        //load map file data
        if (File.Exists(Application.persistentDataPath + "/" + _file_name + ".dat"))
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
        //load room file data
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
            //file not found
            return 0;
        }
    }

    public static int LoadMapSize(String _file_name)
    {
        //load map size file data
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

    public static int LoadSpawnPoint(String _file_name)
    {
        //load spawn point file data
        if (File.Exists(Application.persistentDataPath + "/" + _file_name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + _file_name + ".dat", FileMode.Open);

            Map data = bf.Deserialize(stream) as Map;

            stream.Close();

            return data.spawn_point;
        }

        else
        {
            Debug.LogError("File not found");
            return 0;
        }
    }

    public static List<int> LoadDoors(String _file_name)
    {
        //load door file data
        if (File.Exists(Application.persistentDataPath + "/" + _file_name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + _file_name + ".dat", FileMode.Open);

            Map data = bf.Deserialize(stream) as Map;

            stream.Close();
            return data.doors;
        }

        else
        {
            Debug.LogError("File not found");
            return null;
        }
    }

    public static List<int> LoadKeys(String _file_name)
    {
        //load key file data
        if (File.Exists(Application.persistentDataPath + "/" + _file_name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + _file_name + ".dat", FileMode.Open);

            Map data = bf.Deserialize(stream) as Map;

            stream.Close();
            return data.keys;
        }

        else
        {
            Debug.LogError("File not found");
            return null;
        }
    }

    [Serializable]
    public class Map
    {
        //saved data
        public string name; //file name
        public int[] map; //tile pieces
        public int rooms = 0; //number of rooms
        public int grid_size = 0; // size of grid
        public int spawn_point = 0; // spawn point in grid
        public List<int> doors; //door positions
        public List<int> keys; //key positions

        public Map(MapData _map)
        {
            name = _map.name;
            map = _map.map;
            rooms = _map.rooms;
            grid_size = _map.grid_size;
            spawn_point = _map.spawn_point;
            doors = _map.doors;
            keys = _map.keys;
        }
    }

    [Serializable]
    public class Files
    {
        //files
        public List<string> file_names;

        public Files(FilesList _files)
        {
            file_names = _files.file_names;
        }
    }
}
