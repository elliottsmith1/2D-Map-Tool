using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public bool door_left = false;
    public bool door_right = false;
    public bool door_top = false;
    public bool door_bottom = true;
    public int id;
    public Color32 door_colour;
    public GameObject door_floor;
    public GameObject door;
}
