using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    easy, medium, hard
}

public class MapCreator : MonoBehaviour {

    [SerializeField] bool spawn_grid = false;

    [SerializeField] int grid_size = 10;
    private int grid_height = 10;
    private int grid_width = 10;
    [SerializeField] Vector3[] tile_positions;
    [SerializeField] GameObject tile_prefab;
    [SerializeField] GameObject[] tiles;

    [SerializeField] Sprite junction;
    [SerializeField] Sprite blank;
    [SerializeField] Sprite room_blank;

    [SerializeField] Sprite door_right;
    [SerializeField] Sprite door_top;
    [SerializeField] Sprite door_left;
    [SerializeField] Sprite door_bottom;

    [SerializeField] bool spawn_rooms = false;

    [SerializeField] Difficulty difficulty;
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
    }
	
	// Update is called once per frame
	void Update () {        

        if (spawn_grid)
        {
            ResetGrid();            

            spawn_grid = false;
        }
    }

    void SpawnGrid()
    {
        grid_height = grid_size;
        grid_width = grid_size;

        tile_positions = new Vector3[grid_height * grid_width];
        tiles = new GameObject[tile_positions.Length];        

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

        int tile_rand = Random.Range(0, tiles.Length);

        tiles[tile_rand].GetComponent<TileScript>().open_right = true;
        tiles[tile_rand].GetComponent<TileScript>().open_up = true;
        tiles[tile_rand].GetComponent<TileScript>().open_left = true;
        tiles[tile_rand].GetComponent<TileScript>().open_down = true;

        tiles[tile_rand].GetComponent<SpriteRenderer>().sprite = junction;        
    }

    void ResetGrid()
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

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<SpriteRenderer>().sprite = blank;
            tiles[i].GetComponent<TileScript>().ResetTile();
            tiles[i].GetComponent<TileScript>().SetDifficulty(difficulty_num);
            tiles[i].GetComponent<TileScript>().tile_already_set = false;
            tiles[i].GetComponent<TileScript>().SetSpawnRooms(spawn_rooms);

        }

        int tile_rand = Random.Range(0, tiles.Length);

        tiles[tile_rand].GetComponent<TileScript>().open_right = true;
        tiles[tile_rand].GetComponent<TileScript>().open_up = true;
        tiles[tile_rand].GetComponent<TileScript>().open_left = true;
        tiles[tile_rand].GetComponent<TileScript>().open_down = true;

        tiles[tile_rand].GetComponent<SpriteRenderer>().sprite = junction;
    }

    public bool SpawnRoom(GameObject _door)
    {
        int room_height = Random.Range(3, 8);
        int room_width = Random.Range(3, 8);

        TileScript door = _door.GetComponent<TileScript>();

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

                        if (room_floor_tiles[j + (room_height * i)].tag != "Tile")
                        {
                            return false;
                        }
                    }
                }
            }

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


                }
            }
        }

        return true;
    }
}
