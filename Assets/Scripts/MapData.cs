using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public string name = "map";
    public int[] map = new int[3];
    public int rooms = 0;
    public int grid_size = 0;

    [SerializeField] MapCreator map_creator;

    public void Save()
    {
        FileManagement.SaveFile(this);
    }

    public void Load()
    {
        rooms = FileManagement.LoadMapRooms();
        grid_size = FileManagement.LoadMapSize();
        map = FileManagement.LoadMap();

        map_creator.LoadMap();
    }
}
