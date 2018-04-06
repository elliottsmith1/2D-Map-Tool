using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Door_Position
{
    top, left, right, bottom
}

public class Door : MonoBehaviour {

    public Door_Position door_pos = Door_Position.bottom; //direction of door
    public TileScript floor; //where places
    public List<TileScript> possibile_locations = new List<TileScript>(); //potential spots to spawn key
    public List<TileScript> possibile_locations_to_check = new List<TileScript>(); //potential spots that need checking

    [SerializeField] Sprite blank;
    [SerializeField] Sprite blank_floor;
    [SerializeField] GameObject key_prefab;

    private MapCreator map_creator_ref;
    private GameObject key_ref;

    public bool checking = false;
    public bool key_spawned = false;

    // Use this for initialization
    void Start ()
    {
        map_creator_ref = GameObject.Find("Map Creator").GetComponent<MapCreator>();

        //offset position
        float offset = 0.24f;

        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
        pos.z = -1;
        pos.x = gameObject.transform.position.x;
        pos.y = gameObject.transform.position.y;

        //set direction//offset
        switch (door_pos)
        {
            case Door_Position.bottom:
                pos.y -= offset;
                gameObject.transform.position = pos;
                break;
            case Door_Position.left:
                pos.x -= offset;
                gameObject.transform.position = pos;
                gameObject.transform.Rotate(0, 0, 90);              
                break;
            case Door_Position.right:
                pos.x += offset;
                gameObject.transform.position = pos;
                gameObject.transform.Rotate(0, 0, 90);
                break;
            case Door_Position.top:
                pos.y += offset;
                gameObject.transform.position = pos;
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (checking)
        {
            CheckPath();
        }

        if (!checking)
        {
            EdgeCases();
        }
	}

    void EdgeCases()
    {
        //check edge cases in case door out of place
        //if so, destroy
        bool destroy = false;

        if ((door_pos == Door_Position.left) || (door_pos == Door_Position.right))
        {
            if (floor.adjacent_tiles[2])
            {
                if ((floor.adjacent_tiles[2].GetComponent<SpriteRenderer>().sprite == blank) ||
                        (floor.adjacent_tiles[2].GetComponent<SpriteRenderer>().sprite == blank_floor))
                {
                    destroy = true;
                }
            }

            if (floor.adjacent_tiles[0])
            {
                if ((floor.adjacent_tiles[0].GetComponent<SpriteRenderer>().sprite == blank) ||
                        (floor.adjacent_tiles[0].GetComponent<SpriteRenderer>().sprite == blank_floor))
                {
                    destroy = true;
                }
            }
        }

        if ((door_pos == Door_Position.top) || (door_pos == Door_Position.bottom))
        {
            if (floor.adjacent_tiles[1])
            {
                if ((floor.adjacent_tiles[1].GetComponent<SpriteRenderer>().sprite == blank) ||
                        (floor.adjacent_tiles[1].GetComponent<SpriteRenderer>().sprite == blank_floor))
                {
                    destroy = true;
                }
            }

            if (floor.adjacent_tiles[3])
            {
                if ((floor.adjacent_tiles[3].GetComponent<SpriteRenderer>().sprite == blank) ||
                        (floor.adjacent_tiles[3].GetComponent<SpriteRenderer>().sprite == blank_floor))
                {
                    destroy = true;
                }
            }
        }

        if (destroy)
        {
            Destroy(this.gameObject);
        }

    }

    void CheckPath()
    {
        //check path ahead and add positions to list of possible places to spawn key
        //this way key is always within reach of door

        int num_to_check = 10; //only check 10 per update to reduce lag

        bool new_tiles = false;

        //check adjacent tiles and all their adjacent tiles
        if (possibile_locations_to_check.Count > 0)
        {
            for (int i = 0; i < num_to_check; i++)
            {
                if (i < possibile_locations_to_check.Count)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (possibile_locations_to_check[i].adjacent_tiles[j])
                        {
                            if (possibile_locations_to_check[i].adjacent_tiles[j].tag == "Tile")
                            {
                                if (possibile_locations_to_check[i].adjacent_tiles[j].GetComponent<SpriteRenderer>().sprite != blank)
                                {
                                    if (possibile_locations_to_check[i].adjacent_tiles[j] != floor.gameObject)
                                    {
                                        bool already_a_potential = false;

                                        TileScript potential = possibile_locations_to_check[i].adjacent_tiles[j].GetComponent<TileScript>();

                                        //check if position already checked
                                        for (int k = 0; k < possibile_locations_to_check.Count; k++)
                                        {
                                            if (possibile_locations_to_check[k] == potential)
                                            {
                                                already_a_potential = true;
                                            }
                                        }

                                        for (int l = 0; l < possibile_locations.Count; l++)
                                        {
                                            if (possibile_locations.Count != 0)
                                            {
                                                if (possibile_locations[l])
                                                {
                                                    if (possibile_locations[l] == potential)
                                                    {
                                                        already_a_potential = true;
                                                    }
                                                }
                                            }
                                        }

                                        //add new possible position
                                        if (!already_a_potential)
                                        {
                                            possibile_locations_to_check.Add(potential);

                                            new_tiles = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //check 10 potential positions
            if (new_tiles)
            {
                bool already_in_list = false;

                for (int i = 0; i < num_to_check; i++)
                {
                    if (i < possibile_locations_to_check.Count)
                    {
                        for (int j = 0; j < possibile_locations.Count; j++)
                        {
                            already_in_list = false;

                            if (possibile_locations[j] == possibile_locations_to_check[i])
                            {
                                already_in_list = true;
                            }
                        }

                        //potential spawn positions
                        if (!already_in_list)
                        {
                            possibile_locations.Add(possibile_locations_to_check[i]);
                        }                        
                    }
                }                             
            }

            //remove already checked positions
            for (int i = 0; i < num_to_check; i++)
            {
                if (i < possibile_locations_to_check.Count)
                {
                    possibile_locations_to_check.RemoveAt(i);
                }
            }            

            //if no more positions to check, stop checking
            if (possibile_locations_to_check.Count == 0)
            {
                checking = false;
            }
        }
    }

    public void SpawnKey()
    {
        //when all potential positions checked, spawn key
        if (!key_spawned)
        {
            //spawn in last 10% to ensure key is far away
            int ratio = (int)(possibile_locations.Count / 1.1f);
            if (possibile_locations.Count > 0)
            {
                int random_num = Random.Range(ratio, possibile_locations.Count);

                Vector3 spawn_pos = possibile_locations[random_num].gameObject.transform.position;

                map_creator_ref.AddKey(possibile_locations[random_num].id);

                spawn_pos.z = -1;

                //spawn key
                GameObject key = Instantiate(key_prefab, spawn_pos, transform.rotation);
                key.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;

                key_ref = key;

                key_spawned = true;
            }

            //if key didnt spawn, try again but anywhere in the possible positions list
            if (!key_spawned)
            {
                if (possibile_locations.Count > 0)
                {
                    int random_num = Random.Range(0, possibile_locations.Count);

                    Vector3 spawn_pos = possibile_locations[random_num].gameObject.transform.position;

                    map_creator_ref.AddKey(possibile_locations[random_num].id);

                    spawn_pos.z = -1;

                    GameObject key = Instantiate(key_prefab, spawn_pos, transform.rotation);
                    key.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;

                    key_ref = key;

                    key_spawned = true;
                }
            }
        }
    }

    public void DestroyDoorAndKey()
    {
        Destroy(key_ref);
        Destroy(this.gameObject);
    }
}
