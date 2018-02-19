using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class MapData
{
    public string name;
    public int[] map;

    public MapData(string _name, int[] _map)
    {
        name = _name;
        map = _map;
    }
}
