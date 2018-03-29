using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Door_Position
{
    top, left, right, bottom
}

public class Door : MonoBehaviour {

    public Door_Position door_pos = Door_Position.bottom;
    public TileScript floor;
    public List<TileScript> possibile_locations = new List<TileScript>();
    public List<TileScript> possibile_locations_to_check = new List<TileScript>();

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

        float offset = 0.24f;

        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
        pos.z = -1;
        pos.x = gameObject.transform.position.x;
        pos.y = gameObject.transform.position.y;

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
        int num_to_check = 10;

        bool new_tiles = false;

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
                                        //if (possibile_locations.Count < 100)
                                        {
                                            bool already_a_potential = false;

                                            TileScript potential = possibile_locations_to_check[i].adjacent_tiles[j].GetComponent<TileScript>();

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
            }


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

                        if (!already_in_list)
                        {
                            possibile_locations.Add(possibile_locations_to_check[i]);
                        }                        
                    }
                }                             
            }

            for (int i = 0; i < num_to_check; i++)
            {
                if (i < possibile_locations_to_check.Count)
                {
                    possibile_locations_to_check.RemoveAt(i);
                }
            }            

            if (possibile_locations_to_check.Count < 1)
            {
                checking = false;
            }
        }
    }

    public void SpawnKey()
    {
        if (!key_spawned)
        {
            int ratio = (int)(possibile_locations.Count / 1.1f);
            if (possibile_locations.Count > 0)
            {
                int random_num = Random.Range(ratio, possibile_locations.Count);

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

    public void DestroyDoorAndKey()
    {
        Destroy(key_ref);
        Destroy(this.gameObject);
    }
}
