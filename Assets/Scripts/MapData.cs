using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapData : MonoBehaviour
{
    //map save data
    public string name = "map";
    public int[] map = new int[3]; //tiles
    public int rooms = 0;
    public int grid_size = 0;
    public int spawn_point = 0;
    public List<int> doors = new List<int>();
    public List<int> keys = new List<int>();

    [SerializeField] FilesList files_list; //saved files
    [SerializeField] InputField text_input; //file name input
    [SerializeField] MapCreator map_creator;
    [SerializeField] Dropdown files_dropdown;

    void Start()
    {
        //get current file
        files_list.Load();
        files_list.Save();        

        //reset files dropdown list
        files_dropdown.ClearOptions();

        if (files_list.file_names != null)
        {
            files_dropdown.AddOptions(files_list.file_names);
        }
    }

    public void Save()
    {
        //save map file
        if (text_input.text != null)
        {
            name = text_input.text;

            FileManagement.SaveFile(this, text_input.text);

            files_list.AddFile(text_input.text);
            files_list.Save();

            //update dropdown
            files_dropdown.ClearOptions();
            files_dropdown.AddOptions(files_list.file_names);
        }
    }

    public void Load()
    {
        //load map data from file
        rooms = FileManagement.LoadMapRooms(files_dropdown.captionText.text);
        grid_size = FileManagement.LoadMapSize(files_dropdown.captionText.text);
        map = FileManagement.LoadMap(files_dropdown.captionText.text);
        spawn_point = FileManagement.LoadSpawnPoint(files_dropdown.captionText.text);
        doors = FileManagement.LoadDoors(files_dropdown.captionText.text);doors = FileManagement.LoadDoors(files_dropdown.captionText.text);
        keys = FileManagement.LoadKeys(files_dropdown.captionText.text);

        //load map
        map_creator.LoadMap();
    }
}
