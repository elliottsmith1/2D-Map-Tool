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
    [SerializeField] FilesList files_list;
    [SerializeField] InputField text_input;
    [SerializeField] MapCreator map_creator;
    [SerializeField] Dropdown files_dropdown;

    void Start()
    {
        files_list.Load();
        files_list.Save();        

        files_dropdown.ClearOptions();
        files_dropdown.AddOptions(files_list.file_names);
    }

    public void Save()
    {
        if (text_input.text != null)
        {
            name = text_input.text;

            FileManagement.SaveFile(this, text_input.text);

            files_list.AddFile(text_input.text);
            files_list.Save();

            files_dropdown.ClearOptions();
            files_dropdown.AddOptions(files_list.file_names);
        }
    }

    public void Load()
    {
        rooms = FileManagement.LoadMapRooms(files_dropdown.captionText.text);
        grid_size = FileManagement.LoadMapSize(files_dropdown.captionText.text);
        map = FileManagement.LoadMap(files_dropdown.captionText.text);

        map_creator.LoadMap();
    }
}
