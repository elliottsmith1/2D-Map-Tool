using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum Difficulty
{
    easy, medium, hard
}

public class MapCreator : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField] bool spawn_grid = false;
    [SerializeField] int grid_size = 10;
    [SerializeField] int tile_total = 0;
    [SerializeField] int tiles_required = 0;
    [SerializeField] int rooms = 0;
    [SerializeField] int rooms_required = 3;
    [SerializeField] bool spawn_rooms = false;
    [SerializeField] Difficulty difficulty;

    [Header("References")]
    [SerializeField] MapData map_data;
    [SerializeField] Vector3[] tile_positions;
    [SerializeField] GameObject tile_prefab;
    [SerializeField] GameObject door_prefab;
    [SerializeField] GameObject[] tiles;
    public List<Room> room_stats;

    [Header("Sprite References")]
    [SerializeField] Sprite junction;
    [SerializeField] Sprite blank;
    [SerializeField] Sprite room_blank;
    [SerializeField] Sprite door_right;
    [SerializeField] Sprite door_top;
    [SerializeField] Sprite door_left;
    [SerializeField] Sprite door_bottom;

    private bool map_set = false;
    private bool activated_doors = false;
    private bool keys_set = false;
    private bool doors_spawned = false;

    private int grid_height = 10;
    private int grid_width = 10;

    public Slider size_slider;
    public Slider rooms_slider;
    public Text room_text;
    public Text tiles_text;
    private float tile_timer = 0.0f;
    private float tile_timer_threshold = 0.5f;   
    
    private int difficulty_num; 

    // Use this for initialization
    void Start ()
    {
        switch(difficulty)
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
	
	// Update is called once per frame
	void Update () {

        tile_timer += Time.deltaTime;

        if (grid_size != size_slider.value)
        {
            grid_size = (int)size_slider.value;
        }

        if (rooms_required != rooms_slider.value)
        {
            rooms_required = (int)rooms_slider.value;
            room_text.text = "Rooms: " + rooms.ToString() + " / " + rooms_required.ToString();

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

        if ((spawn_grid) || ((tile_timer > tile_timer_threshold) && ((tile_total < (tiles.Length / 5)) || (rooms < rooms_required))))
        {
            ResetGrid();            

            spawn_grid = false;
            map_set = false;
        }

        if (tile_timer > tile_timer_threshold)
        {
            if (!map_set)
            {
                UpdateMapState();
            }
        }

        if (map_set)
        {
            if (!doors_spawned)
            {
                SpawnDoorSystem();

                doors_spawned = true;

                return;
            }

            if ((doors_spawned) && (!activated_doors))
            {
                for (int i = 0; i < room_stats.Count; i++)
                {
                    room_stats[i].door.GetComponent<Door>().checking = true;
                }

                activated_doors = true;
                return;
            }

            if ((activated_doors) && (!keys_set))
            {
                for (int i = 0; i < room_stats.Count; i++)
                {
                    if (!room_stats[i].door.GetComponent<Door>().checking)
                    {
                        room_stats[i].door.GetComponent<Door>().SpawnKey();

                        if (i == room_stats.Count)
                        {
                            keys_set = true;
                        }
                    }
                }
            }
        }
    }

    public void LoadMap()
    {
        map_set = true;

        grid_size = map_data.grid_size;
        size_slider.value = grid_size;   

        ResetGrid();

        rooms = map_data.rooms;
        rooms_slider.value = rooms;

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
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<TileScript>().SetType();

            map_data.map[i] = tiles[i].GetComponent<TileScript>().tile_type;
        }

        map_data.rooms = rooms;
        map_data.grid_size = grid_size;

        map_set = true;
    }

    void SpawnGrid()
    {
        grid_height = grid_size;
        grid_width = grid_size;

        map_data.map = new int[grid_size * grid_size];

        tile_positions = new Vector3[grid_height * grid_width];
        tiles = new GameObject[tile_positions.Length];
        tiles_required = tiles.Length / 5;

        float width = 0;
        float height = 0;

        for (int i = 0; i < grid_height; i++)
        {
            for (int j = 0; j < grid_width; j++)
            {
                tile_positions[i + grid_width * j].x = width;
                tile_positions[i + grid_width * j].y = height;

                width += 0.64f;
            }
            height += 0.64f;
            width = 0.0f;
        }

        for (int i = 0; i < tile_positions.Length; i++)
        {
            Vector3 position;
            position.z = 0.0f;
            position.x = tile_positions[i].x;
            position.y = tile_positions[i].y;

            GameObject tile = tile_prefab;

            tile.GetComponent<TileScript>().id = i;
            tile.GetComponent<TileScript>().SetDifficulty(difficulty_num);
            tile.GetComponent<TileScript>().SetSpawnRooms(spawn_rooms);

            tiles[i] = Instantiate(tile, position, Quaternion.identity);
        }

        if (!map_set)
        {
            int tile_rand = Random.Range(0, tiles.Length);

            tiles[tile_rand].GetComponent<TileScript>().open_right = true;
            tiles[tile_rand].GetComponent<TileScript>().open_up = true;
            tiles[tile_rand].GetComponent<TileScript>().open_left = true;
            tiles[tile_rand].GetComponent<TileScript>().open_down = true;

            tiles[tile_rand].GetComponent<SpriteRenderer>().sprite = junction;
        }       
    }

    public void ResetGrid()
    {
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

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<SpriteRenderer>().sprite = blank;
            tiles[i].GetComponent<TileScript>().ResetTile();
            tiles[i].GetComponent<TileScript>().SetDifficulty(difficulty_num);
            tiles[i].GetComponent<TileScript>().SetSpawnRooms(spawn_rooms);
        }

        if (!map_set)
        {
            int tile_rand = Random.Range(0, tiles.Length);

            tiles[tile_rand].GetComponent<TileScript>().open_right = true;
            tiles[tile_rand].GetComponent<TileScript>().open_up = true;
            tiles[tile_rand].GetComponent<TileScript>().open_left = true;
            tiles[tile_rand].GetComponent<TileScript>().open_down = true;

            tiles[tile_rand].GetComponent<SpriteRenderer>().sprite = junction;
        }
    }

    public void AddTile()
    {
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
        int room_height = Random.Range(3, 8);
        int room_width = Random.Range(3, 8);

        TileScript door = _door.GetComponent<TileScript>();

        //GameObject door_pref = Instantiate(door_prefab, _door.transform.position, _door.transform.rotation);
        //door_pref.GetComponent<Door>().door_pos = Door_Position.bottom;
        //door_pref.GetComponent<Door>().floor = door;
        //door_pref.GetComponent<Door>().possibile_locations_to_check.Add(door);

        GameObject[] room_floor_tiles = new GameObject[room_height * room_width];

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
            room.door_floor = _door; 

            int rand_col_1 = Random.Range(0, 255);
            int rand_col_2 = Random.Range(0, 255);
            int rand_col_3 = Random.Range(0, 255);

            Color32 col = new Color32((byte)rand_col_1, (byte)rand_col_2, (byte)rand_col_3, 255);

            room.door_colour = col;

            //door_pref.GetComponent<SpriteRenderer>().color = room.door_colour;

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
        for (int i = 0; i < room_stats.Count; i++)
        {
            TileScript door = room_stats[i].door_floor.GetComponent<TileScript>();

            GameObject door_pref = Instantiate(door_prefab, room_stats[i].door_floor.transform.position, room_stats[i].door_floor.transform.rotation);
            door_pref.GetComponent<Door>().door_pos = Door_Position.bottom;
            door_pref.GetComponent<Door>().floor = door;
            door_pref.GetComponent<Door>().possibile_locations_to_check.Add(door);
            door_pref.GetComponent<SpriteRenderer>().color = room_stats[i].door_colour;

            room_stats[i].door = door_pref;
        }
    }
}
