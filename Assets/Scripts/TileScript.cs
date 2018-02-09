using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public int id;
    public int value;

    public bool open_up = false;
    public bool open_down = false;
    public bool open_left = false;
    public bool open_right = false;

    public bool tile_already_set = false;

    private SpriteRenderer sprite_rend;
    private int difficulty;

    [SerializeField] Sprite bottom_left;
    [SerializeField] Sprite bottom_right;
    [SerializeField] Sprite top_left;
    [SerializeField] Sprite top_right;

    [SerializeField] Sprite vertical;
    [SerializeField] Sprite horizontal;

    [SerializeField] Sprite vertical_gap_left;
    [SerializeField] Sprite vertical_gap_right;

    [SerializeField] Sprite horizontal_gap_top;
    [SerializeField] Sprite horizontal_gap_bottom;

    [SerializeField] Sprite junction;

    [SerializeField] Sprite blank;

    [SerializeField] GameObject[] adjacent_tiles;

    // Use this for initialization
    void Start ()
    {
        adjacent_tiles = new GameObject[4];

        sprite_rend = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        SetTile();
	}

    public void SetDifficulty(int _dif)
    {
        difficulty = _dif;
    }

    public void ResetTile()
    {
        open_down = false;
        open_left = false;
        open_right = false;
        open_up = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Tile")
        {
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

        SetTile();
    }

    public void SetTile()
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

    void ChangeTile()
    {
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

        if (!tile_already_set)
        {
            if (open_down)
            {
                int tile_rand = Random.Range(0, difficulty);

                if (tile_rand <= difficulty - 4)
                {
                    sprite_rend.sprite = vertical;
                    open_up = true;
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

                if (tile_rand <= difficulty - 4)
                {
                    sprite_rend.sprite = vertical;
                    open_down = true;
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

                if (tile_rand <= difficulty - 4)
                {
                    sprite_rend.sprite = horizontal;
                    open_right = true;
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

                if (tile_rand <= difficulty - 4)
                {
                    sprite_rend.sprite = horizontal;
                    open_left = true;
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
