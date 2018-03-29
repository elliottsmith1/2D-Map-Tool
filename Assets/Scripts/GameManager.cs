using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject wall_prefab;
    [SerializeField] GameObject player;

    private int[] map = new int[3];
    private MapCreator map_creator;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        map_creator = GameObject.Find("Map Creator").GetComponent<MapCreator>();
        map_creator.CreateGame(this);

        RenderSettings.ambientLight = Color.black;
    }

    public void CreateMap(int[] _map, int _size, int _spawn)
    {
        map = _map;

        float width = 0;
        float height = 0;

        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                Vector3 spawn_pos = new Vector3(width, 1.5f, height);

                if (map[i + _size * j] == 25)
                {
                    GameObject wall = Instantiate(wall_prefab, spawn_pos, Quaternion.identity);                    
                }

                if ((i + _size * j) == _spawn)
                {
                    player.transform.position = spawn_pos;
                }

                width += 2.0f;
            }
            height += 2.0f;
            width = 0.0f;
        }

        
    }
}
