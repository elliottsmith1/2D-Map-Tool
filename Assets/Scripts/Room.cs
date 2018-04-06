using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    //room data
    //if direction has a door
    public bool door_left = false;
    public bool door_right = false;
    public bool door_top = false;
    public bool door_bottom = true;

    public int id;
    public Color32 door_colour; //random colour
    public GameObject[] door_floors = new GameObject[4]; //each direction can have 1 door
    public List<GameObject> doors = new List<GameObject>(); //door refs
}
