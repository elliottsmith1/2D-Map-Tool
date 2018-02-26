using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilesList : MonoBehaviour
{
    public List<string> file_names;

    void Start()
    {
        file_names = new List<string>();
    }

    public void Save()
    {
        FileManagement.SaveFilesList(this);
    }

    public void Load()
    {
        file_names = FileManagement.LoadFilesList();
    }

    public void AddFile(string _file)
    {
        file_names.Add(_file);
    }
}
