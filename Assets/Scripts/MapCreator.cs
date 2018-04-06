using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


//*deprecated* difficult/chance to change direction of tunnel
public enum Difficulty
{
    easy, medium, hard
}

public class MapCreator : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField] bool spawn_grid = false; 
    [SerializeField] int grid_size = 10; //width/height
    [SerializeField] int tile_total = 0;
    [SerializeField] int tiles_required = 0; //percentage of grid to be covered
    [SerializeField] int rooms = 0;
    [SerializeField] int rooms_required = 3; //set by user
    [SerializeField] bool spawn_rooms = false; //whether to spawn rooms
    [SerializeField] Difficulty difficulty;

    [Header("References")]
    [SerializeField] MapData map_data; //saved data on current map
    [SerializeField] Vector3[] tile_positions; //used to spawn grid
    [SerializeField] GameObject tile_prefab; 
    [SerializeField] GameObject door_prefab;
    [SerializeField] GameObject[] tiles; //grid tiles
    [SerializeField] GameObject loading_screen;
    public List<Room> room_stats; //room data

    [Header("Sprite References")] 
    //sprites to reference
    [SerializeField] Sprite junction;
    [SerializeField] Sprite blank;
    [SerializeField] Sprite room_blank;
    [SerializeField] Sprite door_right;
    [SerializeField] Sprite door_top;
    [SerializeField] Sprite door_left;
    [SerializeField] Sprite door_bottom;

    private bool map_set = false; //if tiles are done setting
    private bool activated_doors = false; //doors active or not
    private bool keys_set = false; //spawned keys or not
    private bool doors_spawned = false; //spawned doors or not

    private int grid_height = 10;
    private int grid_width = 10;
    private int spawn_point; //where to start grid/spwan player

    //UI
    public Slider size_slider; 
    public Slider rooms_slider; 
    public Text room_text; 
    public Text tiles_text;
    private float tile_timer = 0.0f;
    private float tile_timer_threshold = 0.5f;   
    
    private int difficulty_num;

    private List<GameObject> doors_and_keys = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        switch (difficulty)
        {
            case Difficulty.easy:
                difficulty_num = 50;
                break;

            case Difficulty.medium:
                difficulty_num = 30;
                break;

            case Difficulty.hard:
                difficulty_num = 10;
                break;
        }

        SpawnGrid();

        room_text.text = "Rooms: " + rooms.ToString() + " / " + rooms_required.ToString();
        tiles_text.text = "Tiles: " + tile_total.ToString() + " / " + (tiles.Length / 5).ToString();
    }

    void Awake()
    {
        //use these when switching to game scene
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(map_data);
    }
	
	// Update is called once per frame
	void Update () {

        tile_timer += Time.deltaTime;

        //set UI
        if (grid_size != size_slider.value)
        {
            grid_size = (int)size_slider.value;
        }

        if (rooms_required != rooms_slider.value)
        {
            rooms_required = (int)rooms_slider.value;

            if (room_text)
            {
                room_text.text = "Rooms: " + rooms.ToString() + " / " + rooms_required.ToString();
            }

            if (rooms_required == 0)
            {
                if (spawn_rooms)
                {
                    spawn_rooms = false;
                }

                if (room_text.color != Color.green)
                {
                    room_text.color = Color.green;
                }
            }

            else
            {
                if (!spawn_rooms)
                {
                    spawn_rooms = true;
                }
            }
        }

        //if parameters not met, restart 
        //tiles spawned doesnt match percentage needed to cover (total / 5)
        //number of rooms not enough
        if ((spawn_grid) || ((tile_timer > tile_timer_threshold) && ((tile_total < (tiles.Length / 5)) || (rooms < rooms_required))))
        {
            if (!loading_screen.activeSelf)
            {
                loading_screen.SetActive(true);
            }

            ResetGrid();            

            spawn_grid = false;
            map_set = false;            
        }

        //when tiles no longer changing, update map state
        if (tile_timer > tile_timer_threshold)
        {
            if (!map_set)
            {
                UpdateMapState();                
            }
        }

        //spawn doors onto rooms
        if (map_set)
        {
            if (!doors_spawned)
            {
                SpawnDoorSystem();

                doors_spawned = true;

                tile_timer = 0.0f;

                return;
            }

            //activate doors (start checking where to spawn key)
            if ((doors_spawned) && (!activated_doors))
            {
                for (int i = 0; i < room_stats.Count; i++)
                {
                    room_stats[i].doors[0].GetComponent<Door>().checking = true;
                    tile_timer = 0.0f;
                }

                activated_doors = true;                
                return;
            }

            //check if each door is ready to spawn key
            //spawn keys
            if ((activated_doors) && (!keys_set))
            {
                for (int i = 0; i < room_stats.Count; i++)
                {
                    if (room_stats[i].doors[0])
                    {
                        if (!room_stats[i].doors[0].GetComponent<Door>().checking)
                        {                            
                            room_stats[i].doors[0].GetComponent<Door>().SpawnKey();

                            if (i == room_stats.Count - 1)
                            {
                                if (tile_timer > tile_timer_threshold)
                                {                                    
                                    keys_set = true;

                                    UpdateMapState();

                                    loading_screen.SetActive(false);                                    
                                }
                                
                                //if taking too long, restart
                                if (tile_timer > 10)
                                {
                                    ResetGrid();
                                }                           
                            }                           
                        }
                    }

                    //delete rooms that no longer exist
                    else
                    {
                        room_stats.RemoveAt(i);
                        return;
                    }
                }
            }
        }
    }

    public void LoadMap()
    {
        //function to load a saved map
        map_set = true;

        //pull map data from save
        grid_size = map_data.grid_size;
        size_slider.value = grid_size;              

        ResetGrid();

        rooms = map_data.rooms;
        rooms_slider.value = rooms;
        spawn_point = map_data.spawn_point;

        room_text.text = "Rooms: " + rooms.ToString() + " / " + rooms_required.ToString();

        if (rooms >= rooms_required)
        {
            if (room_text.color != Color.green)
            {
                room_text.color = Color.green;
            }
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<TileScript>().tile_type = map_data.map[i];
            tiles[i].GetComponent<TileScript>().LoadTile();
        }        
    }

    void UpdateMapState()
    {
        //function to assign each tile a type
        for (int i = 0; i < tiles.Length; i++)
        {            
            tiles[i].GetComponent<TileScript>().SetType();

            map_data.map[i] = tiles[i].GetComponent<TileScript>().tile_type;
        }

        //update save data
        map_data.rooms = rooms;
        map_data.grid_size = grid_size;
        map_data.spawn_point = spawn_point;

        map_set = true;
    }

    void SpawnGrid()
    {
        //function to spawn tiles in correct positions

        grid_height = grid_size;
        grid_width = grid_size;

        map_data.map = new int[grid_size * grid_size];

        tile_positions = new Vector3[grid_height * grid_width];
        tiles = new GameObject[tile_positions.Length];
        tiles_required = tiles.Length / 5;

        float width = 0;
        float height = 0;

        //spawn tiles
        for (int i = 0; i < grid_height; i++)
        {
            for (int j = 0; j < grid_width; j++)
            {
                Vector3 position = new Vector3(width, height, 0.0f);

                GameObject tile = tile_prefab;

                tile.GetComponent<TileScript>().id = j + grid_size * i;
                tile.GetComponent<TileScript>().SetDifficulty(difficulty_num);
                tile.GetComponent<TileScript>().SetSpawnRooms(spawn_rooms);

                tiles[i + grid_size * j] = Instantiate(tile, position, Quaternion.identity);

                width += 0.64f;
            }
            height += 0.64f;
            width = 0.0f;
        }

        //set random start point for dungeon
        if (!map_set)
        {
            int tile_rand = Random.Range(0, tiles.Length);

            spawn_point = tile_rand;

            tiles[tile_rand].GetComponent<TileScript>().open_right = true;
            tiles[tile_rand].GetComponent<TileScript>().open_up = true;
            tiles[tile_rand].GetComponent<TileScript>().open_left = true;
            tiles[tile_rand].GetComponent<TileScript>().open_down = true;

            tiles[tile_rand].GetComponent<SpriteRenderer>().sprite = junction;
        }       
    }

    public void ResetGrid()
    {
        //reset everything

        tile_timer = 0.0f;
        tile_total = 0;
        rooms = 0;
        room_stats.Clear();
        room_text.text = "Rooms: " + rooms.ToString() + " / " + rooms_required.ToString();

        if (spawn_rooms)
        {
            room_text.color = Color.red;
        }

        tiles_text.color = Color.red;

        switch (difficulty)
        {
            case Difficulty.easy:
                difficulty_num = 50;
                break;

            case Difficulty.medium:
                difficulty_num = 30;
                break;

            case Difficulty.hard:
                difficulty_num = 10;
                break;
        }

        if (tiles.Length != (grid_size * grid_size))
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                Destroy(tiles[i]);
                tiles[i] = null;
            }

            tiles = null;

            tile_positions = null;

            SpawnGrid();
        }

        tiles_required = tiles.Length / 5;

        //reset all tiles
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<SpriteRenderer>().sprite = blank;
            tiles[i].GetComponent<TileScript>().ResetTile();
            tiles[i].GetComponent<TileScript>().SetDifficulty(difficulty_num);
            tiles[i].GetComponent<TileScript>().SetSpawnRooms(spawn_rooms);
        }

        //reset start point
        if (!map_set)
        {
            int tile_rand = Random.Range(0, tiles.Length);

            spawn_point = tile_rand;

            tiles[tile_rand].GetComponent<TileScript>().open_right = true;
            tiles[tile_rand].GetComponent<TileScript>().open_up = true;
            tiles[tile_rand].GetComponent<TileScript>().open_left = true;
            tiles[tile_rand].GetComponent<TileScript>().open_down = true;

            tiles[tile_rand].GetComponent<SpriteRenderer>().sprite = junction;
        }

        //destroy all spawned doors and keys
        for (int i = 0; i < doors_and_keys.Count; i++)
        {
            if (doors_and_keys[i])
            {
                doors_and_keys[i].GetComponent<Door>().DestroyDoorAndKey();
            }            
        }

        doors_and_keys.Clear();

        activated_doors = false;
        keys_set = false;
        doors_spawned = false;
        map_set = false;
}

    public void AddTile()
    {
        //tile changed 
        //update UI
        //reset timer

        tile_total++;

        tile_timer = 0.0f;

        tiles_text.text = "Tiles: " + tile_total.ToString() + " / " + (tiles.Length / 5).ToString();

        if (tile_total >= tiles_required)
        {
            if (tiles_text.color != Color.green)
            {
                tiles_text.color = Color.green;
            }
        }

        else 
        {
            if (tiles_text.color != Color.red)
            {
                tiles_text.color = Color.red;
            }
        }
    }

    public bool SpawnRoom(GameObject _door)
    {
        //function to spawn a room

        //random size
        int room_height = Random.Range(3, 8);
        int room_width = Random.Range(3, 8);

        TileScript door = _door.GetComponent<TileScript>();

        GameObject[] room_floor_tiles = new GameObject[room_height * room_width];

        //loop through positions room will take up
        if (door.sprite_rend.sprite = door_bottom)
        {
            room_floor_tiles[0] = door.adjacent_tiles[2];

            for (int i = 1; i < room_height; i++)
            {
                if (room_floor_tiles[i - 1])
                {
                    room_floor_tiles[i] = room_floor_tiles[i - 1].GetComponent<TileScript>().adjacent_tiles[2];
                }
            }

            //if positions are already taken, room will not be spawned over the top
            for (int i = 1; i < room_width; i++)
            {
                for (int j = 0; j < room_height; j++)
                {
                    if (room_floor_tiles[j + (room_height * (i - 1))])
                    {
                        room_floor_tiles[j + (room_height * i)] = room_floor_tiles[j + (room_height * (i - 1))].GetComponent<TileScript>().adjacent_tiles[1];

                        if (room_floor_tiles[j + (room_height * i)])
                        {
                            if (room_floor_tiles[j + (room_height * i)].tag != "Tile")
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            int room_id_num = room_stats.Count;
            Room room = new Room();
            room.id = room_id_num;
            room.door_floors[0] = _door; 

            //assign random colour to door
            int rand_col_1 = Random.Range(0, 255);
            int rand_col_2 = Random.Range(0, 255);
            int rand_col_3 = Random.Range(0, 255);

            Color32 col = new Color32((byte)rand_col_1, (byte)rand_col_2, (byte)rand_col_3, 255);

            room.door_colour = col;

            room_stats.Add(room);
            rooms++;

            if (rooms >= rooms_required)
            {
                if (room_text.color != Color.green)
                {
                    room_text.color = Color.green;
                }
            }

            else
            {
                if (room_text.color != Color.red)
                {
                    room_text.color = Color.red;
                }
            }

            room_text.text = "Rooms: " + rooms.ToString() + " / " + rooms_required.ToString();

            //assign room tiles
            for (int i = 0; i < room_floor_tiles.Length; i++)
            {
                if (room_floor_tiles[i])
                {
                    TileScript floor_tile = room_floor_tiles[i].GetComponent<TileScript>();
                    floor_tile.sprite_rend.sprite = room_blank;

                    floor_tile.open_down = false;
                    floor_tile.open_left = false;
                    floor_tile.open_right = false;
                    floor_tile.open_up = false;

                    floor_tile.gameObject.tag = "RoomTile";
                    floor_tile.room_tile_already_set = true;
                    floor_tile.tile_already_set = true;
                    floor_tile.room_id = room_id_num;
                }
            }
        }        
        return true;
    }

    void SpawnDoorSystem()
    {
        //function to spawn doors onto rooms
        for (int i = 0; i < room_stats.Count; i++)
        {
            //spawn door on original room opening and save to data
            TileScript door = room_stats[i].door_floors[0].GetComponent<TileScript>();

            map_data.doors.Add(room_stats[i].door_floors[0].GetComponent<TileScript>().id);

            GameObject door_pref = Instantiate(door_prefab, room_stats[i].door_floors[0].transform.position, room_stats[i].door_floors[0].transform.rotation);
            door_pref.GetComponent<Door>().door_pos = Door_Position.bottom;
            door_pref.GetComponent<Door>().floor = door;
            door_pref.GetComponent<Door>().possibile_locations_to_check.Add(door);
            door_pref.GetComponent<SpriteRenderer>().color = room_stats[i].door_colour;

            room_stats[i].doors.Add(door_pref);

            doors_and_keys.Add(door_pref);

            //spawn doors in other openings (current doesn't save to data)
            for (int j = 1; j < 4; j++)
            {
                if (room_stats[i].door_floors[j])
                {
                    GameObject door_ad = Instantiate(door_prefab, room_stats[i].door_floors[j].transform.position, room_stats[i].door_floors[j].transform.rotation);

                    door_ad.GetComponent<SpriteRenderer>().color = room_stats[i].door_colour;
                    door_ad.GetComponent<Door>().floor = room_stats[i].door_floors[j].GetComponent<TileScript>();

                    doors_and_keys.Add(door_ad);

                    switch (j)
                    {
                        case 0:
                            door_ad.GetComponent<Door>().door_pos = Door_Position.bottom;
                            break;
                        case 1:
                            door_ad.GetComponent<Door>().door_pos = Door_Position.right;
                            break;
                        case 2:
                            door_ad.GetComponent<Door>().door_pos = Door_Position.top;
                            break;
                        case 3:
                            door_ad.GetComponent<Door>().door_pos = Door_Position.left;
                            break;
                    }
                }
            }
        }
    }

    public void CreateGame(GameManager _manager)
    {
        //called by game manager to get map data
        _manager.CreateMap(map_data.map, map_data.grid_size, map_data.spawn_point, map_data.doors, map_data.keys);
    }

    public void PlayGame()
    {
        //load game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }

    public void AddKey(int _id)
    {
        //reference to spawned key
        map_data.keys.Add(_id);
    }
}
