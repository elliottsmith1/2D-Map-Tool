using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapData : MonoBehaviour
{
    public string name = "map";
    public int[] map = new int[3];
    public int rooms = 0;
    public int grid_size = 0;
    [SerializeField] InputField text_input;
    [SerializeField] MapCreator map_creator;

    public void Save()
    {
        FileManagement.SaveFile(this, text_input.text);
    }

    public void Load()
    {
        rooms = FileManagement.LoadMapRooms(text_input.text);
        grid_size = FileManagement.LoadMapSize(text_input.text);
        map = FileManagement.LoadMap(text_input.text);

        map_creator.LoadMap();
    }
}
