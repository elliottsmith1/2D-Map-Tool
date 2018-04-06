using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    [Header("Parameters")]
    public int id;
    public int tile_type;
    public int value;

    //which way the tunnel can go
    public bool open_up = false;
    public bool open_down = false;
    public bool open_left = false;
    public bool open_right = false;

    //if tile has already been assigned 
    public bool tile_already_set = false;
    public bool room_tile_already_set = false;

    public int room_id;

    [Header("References")]
    public GameObject[] adjacent_tiles; //the 4 adjacent tiles 
    public SpriteRenderer sprite_rend;    

    [Header("Sprite References")]
    //all possible sprites to use
    [SerializeField] Sprite bottom_left;
    [SerializeField] Sprite bottom_right;
    [SerializeField] Sprite top_left;
    [SerializeField] Sprite top_right;

    [SerializeField] Sprite door_left;
    [SerializeField] Sprite door_right;
    [SerializeField] Sprite door_top;
    [SerializeField] Sprite door_bottom;

    [SerializeField] Sprite room_top;
    [SerializeField] Sprite room_right;
    [SerializeField] Sprite room_bottom;
    [SerializeField] Sprite room_left;

    [SerializeField] Sprite room_top_right;
    [SerializeField] Sprite room_top_left;
    [SerializeField] Sprite room_bottom_right;
    [SerializeField] Sprite room_bottom_left;

    [SerializeField] Sprite room_blank;

    [SerializeField] Sprite vertical;
    [SerializeField] Sprite horizontal;

    [SerializeField] Sprite vertical_gap_left;
    [SerializeField] Sprite vertical_gap_right;

    [SerializeField] Sprite horizontal_gap_top;
    [SerializeField] Sprite horizontal_gap_bottom;

    [SerializeField] Sprite junction;

    [SerializeField] Sprite blank;

    //*deprecated* difficult/chance to deviate current path
    private int difficulty;

    //timer for tile placement delay
    private float delay_timer = 0.0f;
    private float delay_threshold = 0.075f;

    private bool timer_on = false;
    private bool spawn_rooms = false; //whether or not to spawn rooms
    private bool updated_map_creator = false; //check if current map state is saved

    private MapCreator map_creator;

    // Use this for initialization
    void Start ()
    {
        adjacent_tiles = new GameObject[4];

        sprite_rend = GetComponent<SpriteRenderer>();

        map_creator = GameObject.Find("Map Creator").GetComponent<MapCreator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        SetTile();
        EdgeCases();

        //update state when assigned
        if ((sprite_rend.sprite != blank) && (!updated_map_creator))
        {
            updated_map_creator = true;
            map_creator.AddTile();
        }

        if (timer_on)
        {
            delay_timer += Time.deltaTime;
        }
	}

    public void SetDifficulty(int _dif)
    {
        difficulty = _dif;
    }

    public void SetSpawnRooms(bool _spwn)
    {
        spawn_rooms = _spwn;
    }

    public void LoadTile()
    {
        //function to set correct sprite
        switch (tile_type)
        {
            case 1:
                sprite_rend.sprite = bottom_left;
                break;
            case 2:
                sprite_rend.sprite = bottom_right;
                break;
            case 3:
                sprite_rend.sprite = top_left;
                break;
            case 4:
                sprite_rend.sprite = top_right;
                break;
            case 5:
                sprite_rend.sprite = door_left;
                break;
            case 6:
                sprite_rend.sprite = door_right;
                break;
            case 7:
                sprite_rend.sprite = door_top;
                break;
            case 8:
                sprite_rend.sprite = door_bottom;
                break;
            case 9:
                sprite_rend.sprite = room_top;
                break;
            case 10:
                sprite_rend.sprite = room_right;
                break;
            case 11:
                sprite_rend.sprite = room_bottom;
                break;
            case 12:
                sprite_rend.sprite = room_left;
                break;
            case 13:
                sprite_rend.sprite = room_top_right;
                break;
            case 14:
                sprite_rend.sprite = room_top_left;
                break;
            case 15:
                sprite_rend.sprite = room_bottom_right;
                break;
            case 16:
                sprite_rend.sprite = room_bottom_left;
                break;
            case 17:
                sprite_rend.sprite = room_blank;
                break;
            case 18:
                sprite_rend.sprite = vertical;
                break;
            case 19:
                sprite_rend.sprite = horizontal;
                break;
            case 20:
                sprite_rend.sprite = vertical_gap_left;
                break;
            case 21:
                sprite_rend.sprite = vertical_gap_right;
                break;
            case 22:
                sprite_rend.sprite = horizontal_gap_top;
                break;
            case 23:
                sprite_rend.sprite = horizontal_gap_bottom;
                break;
            case 24:
                sprite_rend.sprite = junction;
                break;
            case 25:
                sprite_rend.sprite = blank;
                break;
        }
    }

    public void SetType()
    {
        //function to assign sprite type when saving
        if (sprite_rend.sprite == bottom_left)
        {
            tile_type = 1;
        }

        else if (sprite_rend.sprite == bottom_right)
        {
            tile_type = 2;
        }

        else if(sprite_rend.sprite == top_left)
        {
            tile_type = 3;
        }

        else if(sprite_rend.sprite == top_right)
        {
            tile_type = 4;
        }

        else if(sprite_rend.sprite == door_left)
        {
            tile_type = 5;
        }

        else if(sprite_rend.sprite == door_right)
        {
            tile_type = 6;
        }

        else if(sprite_rend.sprite == door_top)
        {
            tile_type = 7;
        }

        else if(sprite_rend.sprite == door_bottom)
        {
            tile_type = 8;
        }

        else if(sprite_rend.sprite == room_top)
        {
            tile_type = 9;
        }

        else if(sprite_rend.sprite == room_right)
        {
            tile_type = 10;
        }

        else if(sprite_rend.sprite == room_bottom)
        {
            tile_type = 11;
        }

        else if(sprite_rend.sprite == room_left)
        {
            tile_type = 12;
        }

        else if(sprite_rend.sprite == room_top_right)
        {
            tile_type = 13;
        }

        else if(sprite_rend.sprite == room_top_left)
        {
            tile_type = 14;
        }

        else if(sprite_rend.sprite == room_bottom_right)
        {
            tile_type = 15;
        }

        else if(sprite_rend.sprite == room_bottom_left)
        {
            tile_type = 16;
        }

        else if(sprite_rend.sprite == room_blank)
        {
            tile_type = 17;
        }

        else if(sprite_rend.sprite == vertical)
        {
            tile_type = 18;
        }

        else if(sprite_rend.sprite == horizontal)
        {
            tile_type = 19;
        }

        else if(sprite_rend.sprite == vertical_gap_left)
        {
            tile_type = 20;
        }

        else if(sprite_rend.sprite == vertical_gap_right)
        {
            tile_type = 21;
        }

        else if(sprite_rend.sprite == horizontal_gap_top)
        {
            tile_type = 22;
        }

        else if(sprite_rend.sprite == horizontal_gap_bottom)
        {
            tile_type = 23;
        }

        else if (sprite_rend.sprite == junction)
        {
            tile_type = 24;
        }

        else if (sprite_rend.sprite == blank)
        {
            tile_type = 25;
        }
    }

    public void ResetTile()
    {
        //reset everything
        open_down = false;
        open_left = false;
        open_right = false;
        open_up = false;
        tile_already_set = false;
        room_tile_already_set = false;
        updated_map_creator = false;
        delay_timer = 0.0f;
        gameObject.tag = "Tile";
    }

    void OnMouseDown()
    {
        //function to spawn new tunnels when clicked on
        //defaults to 4-way junction if not adjacent to other tunnels
        if (sprite_rend.sprite == blank)
        {
            bool tunnel_top = false;
            bool tunnel_bottom = false;
            bool tunnel_left = false;
            bool tunnel_right = false;

            if (adjacent_tiles[2])
            {
                if ((adjacent_tiles[2].tag == "Tile") && (adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite != blank))
                {
                    open_up = true;
                    tunnel_top = true;
                }
            }

            if (adjacent_tiles[0])
            {
                if ((adjacent_tiles[0].tag == "Tile") && (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite != blank))
                {
                    open_down = true;
                    tunnel_bottom = true;
                }
            }

            if (adjacent_tiles[3])
            {
                if ((adjacent_tiles[3].tag == "Tile") && (adjacent_tiles[3].GetComponent<TileScript>().sprite_rend.sprite != blank))
                {
                    open_left = true;
                    tunnel_left = true;
                }
            }

            if (adjacent_tiles[1])
            {
                if ((adjacent_tiles[1].tag == "Tile") && (adjacent_tiles[1].GetComponent<TileScript>().sprite_rend.sprite != blank))
                {
                    open_right = true;
                    tunnel_right = true;
                }
            }

            if ((!tunnel_bottom) && (!tunnel_left) && (!tunnel_right) && (!tunnel_top))
            {
                open_down = true;
                open_left = true;
                open_right = true;
                open_up = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //assign adjacent tiles when they spawn
        //only triggered once per tile
        if (!adjacent_tiles[3])
        {
            if ((other.transform.position.x < transform.position.x) && (other.transform.position.y == transform.position.y))
            {
                adjacent_tiles[3] = other.gameObject;
            }
        }

        if (!adjacent_tiles[0])
        {
            if ((other.transform.position.x == transform.position.x) && (other.transform.position.y < transform.position.y))
            {
                adjacent_tiles[0] = other.gameObject;
            }
        }

        if (!adjacent_tiles[1])
        {
            if ((other.transform.position.x > transform.position.x) && (other.transform.position.y == transform.position.y))
            {
                adjacent_tiles[1] = other.gameObject;
            }
        }

        if (!adjacent_tiles[2])
        {
            if ((other.transform.position.x == transform.position.x) && (other.transform.position.y > transform.position.y))
            {
                adjacent_tiles[2] = other.gameObject;
            }
        }
    }

    public void SetTile()
    {
        //check if adjacent tiles are open
        //if so change sprite to match
        for (int i = 0; i < adjacent_tiles.Length; i++)
        {
            if (adjacent_tiles[i])
            {
                if ((gameObject.tag == "RoomTile") && (adjacent_tiles[i].tag == "Tile"))
                {
                    ChangeRoomTile();
                }                
            }
        }

        if (!room_tile_already_set)
        {
            if (adjacent_tiles[0])
            {
                if (adjacent_tiles[0].GetComponent<TileScript>().open_up)
                {
                    open_down = true;
                    ChangeTile();
                }
            }

            if (adjacent_tiles[1])
            {
                if (adjacent_tiles[1].GetComponent<TileScript>().open_left)
                {
                    open_right = true;
                    ChangeTile();
                }
            }

            if (adjacent_tiles[2])
            {
                if (adjacent_tiles[2].GetComponent<TileScript>().open_down)
                {
                    open_up = true;
                    ChangeTile();
                }
            }

            if (adjacent_tiles[3])
            {
                if (adjacent_tiles[3].GetComponent<TileScript>().open_right)
                {
                    open_left = true;
                    ChangeTile();
                }
            }
        }
    }

    void ChangeRoomTile()
    {
        //assign room sprite function
        //gets adjacent tile if available
        //sets to room door sprite
        //saves relavent data

        timer_on = true;

        if (timer_on)
        {
            delay_timer += Time.deltaTime;
        }
        
        if (delay_timer > delay_threshold)
        {
            timer_on = false;
            delay_timer = 0.0f;

            int room_complexity = 10;

            if (adjacent_tiles[0])
            {
                TileScript adjacent_room_tile = adjacent_tiles[0].GetComponent<TileScript>();

                if (!adjacent_room_tile.room_tile_already_set)
                {
                    int tile_rand = Random.Range(0, room_complexity);

                    adjacent_room_tile.open_down = false;
                    adjacent_room_tile.open_left = false;
                    adjacent_room_tile.open_right = false;
                    adjacent_room_tile.open_up = false;

                    if (tile_rand <= room_complexity - 2)
                    {
                        adjacent_room_tile.sprite_rend.sprite = room_bottom;
                    }

                    else if (tile_rand == room_complexity - 1)
                    {
                        if (adjacent_room_tile.adjacent_tiles[0])
                        {
                            if (adjacent_room_tile.adjacent_tiles[0].tag == "Tile")
                            {
                                if (!map_creator.room_stats[room_id].door_bottom)
                                {
                                    adjacent_room_tile.sprite_rend.sprite = door_bottom;

                                    adjacent_room_tile.open_down = true;

                                    map_creator.room_stats[room_id].door_bottom = true;
                                    map_creator.room_stats[room_id].door_floors[0] = (adjacent_room_tile.gameObject);
                                }

                                else
                                {
                                    adjacent_room_tile.sprite_rend.sprite = room_bottom;
                                }
                            }
                        }

                        else
                        {
                            adjacent_room_tile.sprite_rend.sprite = room_bottom;
                        }
                    }

                    adjacent_room_tile.tile_already_set = true;
                    adjacent_room_tile.room_tile_already_set = true;
                    adjacent_room_tile.gameObject.tag = "RoomTileWall";
                }
            }

            if (adjacent_tiles[1])
            {
                TileScript adjacent_room_tile = adjacent_tiles[1].GetComponent<TileScript>();

                if (!adjacent_room_tile.room_tile_already_set)
                {
                    int tile_rand = Random.Range(0, room_complexity);

                    adjacent_room_tile.open_down = false;
                    adjacent_room_tile.open_left = false;
                    adjacent_room_tile.open_right = false;
                    adjacent_room_tile.open_up = false;

                    if (tile_rand <= room_complexity - 2)
                    {
                        adjacent_room_tile.sprite_rend.sprite = room_right;
                    }

                    else if (tile_rand == room_complexity - 1)
                    {
                        if (adjacent_room_tile.adjacent_tiles[1])
                        {
                            if (adjacent_room_tile.adjacent_tiles[1].tag == "Tile")
                            {
                                if (!map_creator.room_stats[room_id].door_right)
                                {
                                    adjacent_room_tile.sprite_rend.sprite = door_right;

                                    adjacent_room_tile.open_right = true;

                                    map_creator.room_stats[room_id].door_right = true;
                                    map_creator.room_stats[room_id].door_floors[1] = (adjacent_room_tile.gameObject);
                                }

                                else
                                {
                                    adjacent_room_tile.sprite_rend.sprite = room_right;
                                }
                            }
                        }

                        else
                        {
                            adjacent_room_tile.sprite_rend.sprite = room_right;
                        }
                    }

                    adjacent_room_tile.tile_already_set = true;
                    adjacent_room_tile.room_tile_already_set = true;
                    adjacent_room_tile.gameObject.tag = "RoomTileWall";
                }
            }

            if (adjacent_tiles[2])
            {
                TileScript adjacent_room_tile = adjacent_tiles[2].GetComponent<TileScript>();

                if (!adjacent_room_tile.room_tile_already_set)
                {
                    int tile_rand = Random.Range(0, room_complexity);

                    adjacent_room_tile.open_down = false;
                    adjacent_room_tile.open_left = false;
                    adjacent_room_tile.open_right = false;
                    adjacent_room_tile.open_up = false;

                    if (tile_rand <= room_complexity - 2)
                    {
                        adjacent_room_tile.sprite_rend.sprite = room_top;
                    }

                    else if (tile_rand == room_complexity - 1)
                    {
                        if (adjacent_room_tile.adjacent_tiles[2])
                        {
                            if (adjacent_room_tile.adjacent_tiles[2].tag == "Tile")
                            {
                                if (!map_creator.room_stats[room_id].door_top)
                                {
                                    adjacent_room_tile.sprite_rend.sprite = door_top;

                                    adjacent_room_tile.open_up = true;

                                    map_creator.room_stats[room_id].door_top = true;
                                    map_creator.room_stats[room_id].door_floors[2] = (adjacent_room_tile.gameObject);
                                }

                                else
                                {
                                    adjacent_room_tile.sprite_rend.sprite = room_top;
                                }
                            }
                        }

                        else
                        {
                            adjacent_room_tile.sprite_rend.sprite = room_top;
                        }
                    }

                    adjacent_room_tile.tile_already_set = true;
                    adjacent_room_tile.room_tile_already_set = true;
                    adjacent_room_tile.gameObject.tag = "RoomTileWall";
                }
            }

            if (adjacent_tiles[3])
            {
                TileScript adjacent_room_tile = adjacent_tiles[3].GetComponent<TileScript>();

                if (!adjacent_room_tile.room_tile_already_set)
                {
                    int tile_rand = Random.Range(0, room_complexity);

                    adjacent_room_tile.open_down = false;
                    adjacent_room_tile.open_left = false;
                    adjacent_room_tile.open_right = false;
                    adjacent_room_tile.open_up = false;

                    if (tile_rand <= room_complexity - 2)
                    {
                        adjacent_room_tile.sprite_rend.sprite = room_left;
                    }

                    else if (tile_rand == room_complexity - 1)
                    {
                        if (adjacent_room_tile.adjacent_tiles[3])
                        {
                            if (adjacent_room_tile.adjacent_tiles[3].tag == "Tile")
                            {
                                if (!map_creator.room_stats[room_id].door_left)
                                {
                                    adjacent_room_tile.sprite_rend.sprite = door_left;

                                    adjacent_room_tile.open_left = true;

                                    map_creator.room_stats[room_id].door_left = true;
                                    map_creator.room_stats[room_id].door_floors[3] = (adjacent_room_tile.gameObject);
                                }

                                else
                                {
                                    adjacent_room_tile.sprite_rend.sprite = room_left;
                                }
                            }
                        }

                        else
                        {
                            adjacent_room_tile.sprite_rend.sprite = room_left;
                        }
                    }

                    adjacent_room_tile.tile_already_set = true;
                    adjacent_room_tile.room_tile_already_set = true;
                    adjacent_room_tile.gameObject.tag = "RoomTileWall";
                }
            }
        }
    }

    void EdgeCases()
    {
        //function to check edge cases of sprites 
        //reassigns sprite if tile does not match correctly 

        if ((open_down) && (open_left) && (open_right) && (open_up) && (sprite_rend.sprite != junction))
        {
            sprite_rend.sprite = junction;
            return;
        }

        else if ((open_down) && (open_left) && (open_right) && (!open_up) && (sprite_rend.sprite != horizontal_gap_bottom))
        {
            sprite_rend.sprite = horizontal_gap_bottom;
        }

        else if ((open_left) && (open_right) && (open_up) && (!open_down) && (sprite_rend.sprite != horizontal_gap_top))
        {
            sprite_rend.sprite = horizontal_gap_top;
        }

        else if ((open_down) && (open_right) && (open_up) && (!open_left) && (sprite_rend.sprite != vertical_gap_right))
        {
            sprite_rend.sprite = vertical_gap_right;
        }

        else if ((open_down) && (open_left) && (open_up) && (!open_right) && (sprite_rend.sprite != vertical_gap_left))
        {
            sprite_rend.sprite = vertical_gap_left;
        }
    
        if (((tile_already_set) || (room_tile_already_set)) && (sprite_rend.sprite == blank))
        {
            tile_already_set = false;
            room_tile_already_set = false;
        }

        if (gameObject.tag == "RoomTileWall")
        {
            if (sprite_rend.sprite == door_bottom)
            {
                if (((adjacent_tiles[3]) && (adjacent_tiles[3].GetComponent<TileScript>().sprite_rend.sprite == room_blank)) ||
                    ((adjacent_tiles[0]) && (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_blank)) ||
                    ((adjacent_tiles[1]) && (adjacent_tiles[1].GetComponent<TileScript>().sprite_rend.sprite == room_blank)))
                {
                    sprite_rend.sprite = room_blank;
                    gameObject.tag = "RoomTile";
                }

                if (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_top)
                {
                    sprite_rend.sprite = room_bottom;
                }
            }

            if (sprite_rend.sprite == door_left)
            {
                if (((adjacent_tiles[3]) && (adjacent_tiles[3].GetComponent<TileScript>().sprite_rend.sprite == room_blank)) ||
                    ((adjacent_tiles[0]) && (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_blank)) ||
                    ((adjacent_tiles[2]) && (adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite == room_blank)))
                {
                    sprite_rend.sprite = room_blank;
                    gameObject.tag = "RoomTile";
                }

                if (adjacent_tiles[3].GetComponent<TileScript>().sprite_rend.sprite == room_right)
                {
                    sprite_rend.sprite = room_left;
                }
            }

            if (sprite_rend.sprite == door_right)
            {
                if (((adjacent_tiles[2]) && (adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite == room_blank)) ||
                    ((adjacent_tiles[0]) && (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_blank)) ||
                    ((adjacent_tiles[1]) && (adjacent_tiles[1].GetComponent<TileScript>().sprite_rend.sprite == room_blank)))
                {
                    sprite_rend.sprite = room_blank;
                    gameObject.tag = "RoomTile";
                }

                if (adjacent_tiles[1].GetComponent<TileScript>().sprite_rend.sprite == room_left)
                {
                    sprite_rend.sprite = room_right;
                }
            }

            if (sprite_rend.sprite == door_top)
            {
                if (((adjacent_tiles[3]) && (adjacent_tiles[3].GetComponent<TileScript>().sprite_rend.sprite == room_blank)) ||
                    ((adjacent_tiles[2]) && (adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite == room_blank)) ||
                    ((adjacent_tiles[1]) && (adjacent_tiles[1].GetComponent<TileScript>().sprite_rend.sprite == room_blank)))
                {
                    sprite_rend.sprite = room_blank;
                    gameObject.tag = "RoomTile";
                }

                if (adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite == room_bottom)
                {
                    sprite_rend.sprite = room_top;
                }
            }

            if ((sprite_rend.sprite == room_right) || (sprite_rend.sprite == door_right))
            {
                if (adjacent_tiles[0])
                {
                    if (adjacent_tiles[0].tag == "Tile")
                    {
                        adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite = room_bottom_right;
                        adjacent_tiles[0].tag = "RoomTileWall";
                    }
                }

                if (adjacent_tiles[2])
                {
                    if (adjacent_tiles[2].tag == "Tile")
                    {
                        adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite = room_top_right;
                        adjacent_tiles[2].tag = "RoomTileWall";
                    }
                }

                if (adjacent_tiles[0] && adjacent_tiles[2])
                {
                    if (adjacent_tiles[0].GetComponent<SpriteRenderer>().sprite == room_right)
                    {
                        if (adjacent_tiles[2].GetComponent<SpriteRenderer>().sprite == room_right)
                        {
                            sprite_rend.sprite = room_right;
                        }
                    }
                }
            }

            else if ((sprite_rend.sprite == room_left) || (sprite_rend.sprite == door_left))
            {
                if (adjacent_tiles[0])
                {
                    if (adjacent_tiles[0].tag == "Tile")
                    {
                        adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite = room_bottom_left;
                        adjacent_tiles[0].tag = "RoomTileWall";
                    }
                }

                if (adjacent_tiles[2])
                {
                    if (adjacent_tiles[2].tag == "Tile")
                    {
                        adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite = room_top_left;
                        adjacent_tiles[2].tag = "RoomTileWall";
                    }
                }

                if (adjacent_tiles[0] && adjacent_tiles[2])
                {
                    if (adjacent_tiles[0].GetComponent<SpriteRenderer>().sprite == room_left)
                    {
                        if (adjacent_tiles[2].GetComponent<SpriteRenderer>().sprite == room_left)
                        {
                            sprite_rend.sprite = room_left;
                        }
                    }
                }
            }

            if ((sprite_rend.sprite == room_left) || (sprite_rend.sprite == room_right) || (sprite_rend.sprite == room_top_left) || (sprite_rend.sprite == room_bottom_left)
                || (sprite_rend.sprite == room_top_right) || (sprite_rend.sprite == room_bottom_right))
            {
                if (adjacent_tiles[0])
                {
                    if ((adjacent_tiles[0].tag == "RoomTile") || 
                        (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_top) ||
                            (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_bottom))
                            {
                                sprite_rend.sprite = room_blank;
                                gameObject.tag = "RoomTile";
                            }
                }

                if (adjacent_tiles[2])
                {
                    if ((adjacent_tiles[2].tag == "RoomTile") || 
                        (adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite == room_top) ||
                            (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_bottom))
                            {
                                sprite_rend.sprite = room_blank;
                                gameObject.tag = "RoomTile";
                            }
                }
            }

            if ((sprite_rend.sprite == room_top) || (sprite_rend.sprite == room_bottom))
            {
                if (adjacent_tiles[1])
                {
                    if ((adjacent_tiles[1].tag == "RoomTile") || 
                        (adjacent_tiles[1].GetComponent<TileScript>().sprite_rend.sprite == room_left) ||
                            (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_right))
                            {
                                sprite_rend.sprite = room_blank;
                                gameObject.tag = "RoomTile";
                            }
                }

                if (adjacent_tiles[3])
                {
                    if ((adjacent_tiles[3].tag == "RoomTile") || 
                        (adjacent_tiles[3].GetComponent<TileScript>().sprite_rend.sprite == room_left) ||
                            (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_right))
                            {
                                sprite_rend.sprite = room_blank;
                                gameObject.tag = "RoomTile";
                            }       
                }
            }

            if (sprite_rend.sprite == room_top)
            {
                if (adjacent_tiles[3])
                {
                    if (adjacent_tiles[3].GetComponent<TileScript>().sprite_rend.sprite == room_bottom)
                    {
                        sprite_rend.sprite = room_top_left;
                    }
                }

                if (adjacent_tiles[1])
                {
                    if (adjacent_tiles[1].GetComponent<TileScript>().sprite_rend.sprite == room_bottom)
                    {
                        sprite_rend.sprite = room_top_right;
                    }
                }
            }

            if (sprite_rend.sprite == room_bottom)
            {
                if (adjacent_tiles[3])
                {
                    if (adjacent_tiles[3].GetComponent<TileScript>().sprite_rend.sprite == room_top)
                    {
                        sprite_rend.sprite = room_bottom_left;
                    }
                }

                if (adjacent_tiles[1])
                {
                    if (adjacent_tiles[1].GetComponent<TileScript>().sprite_rend.sprite == room_top)
                    {
                        sprite_rend.sprite = room_bottom_right;
                    }
                }
            }

            if (sprite_rend.sprite == room_left)
            {
                if (adjacent_tiles[2])
                {
                    if (adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite == room_right)
                    {
                        sprite_rend.sprite = room_top_left;
                    }
                }

                if (adjacent_tiles[0])
                {
                    if (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_right)
                    {
                        sprite_rend.sprite = room_bottom_left;
                    }
                }
            }

            if (sprite_rend.sprite == room_right)
            {
                if (adjacent_tiles[0])
                {
                    if (adjacent_tiles[0].GetComponent<TileScript>().sprite_rend.sprite == room_top)
                    {
                        sprite_rend.sprite = room_bottom_right;
                    }
                }

                if (adjacent_tiles[2])
                {
                    if (adjacent_tiles[2].GetComponent<TileScript>().sprite_rend.sprite == room_top)
                    {
                        sprite_rend.sprite = room_top_right;
                    }
                }
            }
        }
    }

    void ChangeTile()
    {
        //function to assign sprite 

        //for each direction
        //assign random value
        //acts as percentage/chance to change direction or spawn a room
        //assign relavent sprite

        timer_on = true;

        if (timer_on)
        {
            delay_timer += Time.deltaTime;
        }

        if (delay_timer > delay_threshold)
        {
            timer_on = false;
            delay_timer = 0.0f;

            if (!tile_already_set)
            {
                if (open_down)
                {                    
                    int tile_rand = Random.Range(0, difficulty);

                    if (tile_rand <= difficulty - 5)
                    {
                        sprite_rend.sprite = vertical;
                        open_up = true;
                    }

                    if (tile_rand == difficulty - 4)
                    {
                        if (spawn_rooms)
                        {       
                            if (!map_creator.SpawnRoom(this.gameObject))
                            {
                                ChangeTile();
                                return;
                            }

                            sprite_rend.sprite = door_bottom;
                            gameObject.tag = "RoomTileWall";
                            room_tile_already_set = true;
                        }
                        else
                        {
                            ChangeTile();
                            return;
                        }
                    }

                    if (tile_rand == difficulty - 3)
                    {
                        sprite_rend.sprite = vertical_gap_left;
                        open_left = true;
                        open_up = true;
                    }

                    if (tile_rand == difficulty - 2)
                    {
                        sprite_rend.sprite = top_left;
                        open_right = true;
                    }

                    if (tile_rand == difficulty - 1)
                    {
                        sprite_rend.sprite = vertical_gap_right;
                        open_right = true;
                        open_up = true;
                    }

                    tile_already_set = true;
                }

                else if (open_up)
                {
                    int tile_rand = Random.Range(0, difficulty);

                    if (tile_rand <= difficulty - 5)
                    {
                        sprite_rend.sprite = vertical;
                        open_down = true;
                    }

                    if (tile_rand == difficulty - 4)
                    {                        
                        ChangeTile();
                        return;                        
                    }

                    if (tile_rand == difficulty - 3)
                    {
                        sprite_rend.sprite = vertical_gap_left;
                        open_left = true;
                        open_down = true;
                    }

                    if (tile_rand == difficulty - 2)
                    {
                        sprite_rend.sprite = bottom_left;
                        open_right = true;
                    }

                    if (tile_rand == difficulty - 1)
                    {
                        sprite_rend.sprite = vertical_gap_right;
                        open_right = true;
                        open_down = true;
                    }

                    tile_already_set = true;
                }

                else if (open_left)
                {
                    int tile_rand = Random.Range(0, difficulty);

                    if (tile_rand <= difficulty - 5)
                    {
                        sprite_rend.sprite = horizontal;
                        open_right = true;
                    }

                    if (tile_rand == difficulty - 4)
                    {
                        ChangeTile();
                        return;
                        
                    }

                    if (tile_rand == difficulty - 3)
                    {
                        sprite_rend.sprite = horizontal_gap_bottom;
                        open_down = true;
                        open_right = true;
                    }

                    if (tile_rand == difficulty - 2)
                    {
                        sprite_rend.sprite = bottom_right;
                        open_up = true;
                    }

                    if (tile_rand == difficulty - 1)
                    {
                        sprite_rend.sprite = horizontal_gap_top;
                        open_up = true;
                        open_right = true;
                    }

                    tile_already_set = true;
                }

                else if (open_right)
                {
                    int tile_rand = Random.Range(0, difficulty);

                    if (tile_rand <= difficulty - 5)
                    {
                        sprite_rend.sprite = horizontal;
                        open_left = true;
                    }

                    if (tile_rand == difficulty - 4)
                    {
                        ChangeTile();
                        return;                        
                    }

                    if (tile_rand == difficulty - 3)
                    {
                        sprite_rend.sprite = horizontal_gap_bottom;
                        open_down = true;
                        open_left = true;
                    }

                    if (tile_rand == difficulty - 2)
                    {
                        sprite_rend.sprite = bottom_left;
                        open_up = true;
                    }

                    if (tile_rand == difficulty - 1)
                    {
                        sprite_rend.sprite = horizontal_gap_top;
                        open_up = true;
                        open_left = true;
                    }

                    tile_already_set = true;
                }
            }
        }
    }
}


