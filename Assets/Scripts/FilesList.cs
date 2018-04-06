using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilesList : MonoBehaviour
{
    public List<string> file_names; //list of saved files

    void Start()
    {
        file_names = new List<string>();
    }

    public void Save()
    {
        //save files
        FileManagement.SaveFilesList(this);
    }

    public void Load()
    {
        //load files
        file_names = FileManagement.LoadFilesList();
    }

    public void AddFile(string _file)
    {
        //add new file
        if (file_names == null)
        {
            file_names = new List<string>();
        }

        file_names.Add(_file);
    }
}
