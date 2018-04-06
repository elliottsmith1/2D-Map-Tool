using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [Header("Sprite References")] //sprites to reference
    [SerializeField] GameObject wall_prefab;
    [SerializeField] GameObject floor_prefab;
    [SerializeField] GameObject door_prefab;
    [SerializeField] GameObject key_prefab;
    [SerializeField] GameObject key_UI_prefab;
    [SerializeField] GameObject player;

    [Header("References")] //outer walls to dungeons
    [SerializeField] List<GameObject> outer_walls;


    private int[] map = new int[3]; //map data
    private List<GameObject> floor_tiles = new List<GameObject>(); //tiles
    private MapCreator map_creator;

    private List<Color> keys_collected = new List<Color>(); //picked up keys
    private List<GameObject> keys_ui = new List<GameObject>(); //UI for collected keys

    private int doors_opened = 0; 
    private int locked_doors = 0;

    void Awake()
    {
        map_creator = (MapCreator)FindObjectOfType(typeof(MapCreator));              
    }

    void Start()
    {
        //get map data
        map_creator.CreateGame(this);
    }

    void Update()
    {
        //reassign new key UI positions
        for (int i = 0; i < keys_ui.Count; i++)
        {
            float pos = -600 + (i * 100);

            if (keys_ui[i].transform.localPosition.x != pos)
            {
                Vector3 new_pos = transform.localPosition;
                new_pos.x = pos;
                new_pos.y = -275;

                keys_ui[i].transform.localPosition = new_pos;
            }
        }
    }

    public void CreateMap(int[] _map, int _size, int _spawn, List<int> _doors, List<int> _keys)
    {
        //function to spawn map

        //set fog settings
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.2f;
        RenderSettings.fogColor = Color.black;

        map_creator.gameObject.SetActive(false);

        map = _map;

        Vector3 new_pos;

        //assign surrounding wall positions
        new_pos = outer_walls[1].transform.position;
        new_pos.z += _size * 2;
        new_pos.z -= 1;
        outer_walls[1].transform.position = new_pos;
        new_pos = outer_walls[3].transform.position;
        new_pos.x += _size * 2;
        new_pos.x -= 1;
        outer_walls[3].transform.position = new_pos;

        float width = 0;
        float height = 0;

        //spawn map pieces
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                Vector3 spawn_pos = new Vector3(width, 1.5f, height);

                int map_num = map[i + _size * j];

                //if not tunnel or room then spawn wall piece
                if ((map_num == 25) || (map_num == 9) || (map_num == 10) || 
                        (map_num == 11) || (map_num == 12) || (map_num == 13)
                         || (map_num == 14) || (map_num == 15) || (map_num == 16))
                {
                    GameObject wall = Instantiate(wall_prefab, spawn_pos, Quaternion.identity);
                    floor_tiles.Add(wall);            
                }

                //otherwise spawn floor/tunnel
                else
                {
                    spawn_pos.y = 0;
                    GameObject floor = Instantiate(floor_prefab, spawn_pos, Quaternion.identity);

                    floor_tiles.Add(floor);
                }

                //set spawn position for player
                if ((i + _size * j) == _spawn)
                {
                    player.transform.position = spawn_pos;
                }

                width += 2.0f;
            }
            height += 2.0f;
            width = 0.0f;
        }        

        //spawn doors if door and key match
        for (int i = 0; i < _doors.Count; i++)
        {
            if (i < _keys.Count)
            {
                if ((_doors[i] <= floor_tiles.Count) && (_keys[i] <= floor_tiles.Count))
                {
                    Vector3 spawn_pos = floor_tiles[_doors[i]].transform.position;
                    spawn_pos.y = 1.5f;

                    GameObject door = Instantiate(door_prefab, spawn_pos, Quaternion.identity);

                    locked_doors++;

                    spawn_pos = floor_tiles[_keys[i]].transform.position;
                    spawn_pos.y = 1.5f;

                    GameObject key = Instantiate(key_prefab, spawn_pos, Quaternion.identity);

                    //random matching colour
                    int rand_col_1 = Random.Range(0, 255);
                    int rand_col_2 = Random.Range(0, 255);
                    int rand_col_3 = Random.Range(0, 255);
                    Color32 col = new Color32((byte)rand_col_1, (byte)rand_col_2, (byte)rand_col_3, 255);

                    key.GetComponent<Renderer>().material.color = col;
                    door.transform.GetChild(0).GetComponent<Renderer>().material.color = col;
                }
            }
        }        
    }

    public void CollectKey(Color _color)
    {
        //collect key function
        keys_collected.Add(_color);

        GameObject key = Instantiate(key_UI_prefab, new Vector3(0, 0, 0), Quaternion.identity);

        key.transform.GetChild(0).GetComponent<Image>().color = _color;

        keys_ui.Add(key.transform.GetChild(0).gameObject);
    }

    public bool CheckKey(Color _color)
    {
        //check if key matches door
        //if so, open door
        for (int i = 0; i < keys_collected.Count; i++)
        {
            if (_color == keys_collected[i])
            {
                keys_collected.RemoveAt(i);

                keys_ui[i].transform.parent.gameObject.SetActive(false);

                keys_ui.RemoveAt(i);

                doors_opened++;

                //if all doors opened, go back to map creator (currently no delay on this)
                if (doors_opened == locked_doors)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("main");
                }

                return true;
            }
        }

        return false;
    }
}
