using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyhole : MonoBehaviour {

    private bool open = false; //if locked
    private GameManager game_manager;
    [SerializeField] GameObject keyhole;
    private Vector3 original_pos;
    private float open_speed = 4.0f;

    // Use this for initialization
    void Start ()
    {
        game_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        original_pos = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if opened then destroy when moved
		if (open)
        {
            transform.Translate((-transform.up * Time.deltaTime) * open_speed);

            if (transform.position.y < (original_pos.y - 2.7f))
            {
                Destroy(this);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //check if player had correct key when near
        if (other.tag == "Player")
        {
            if (game_manager)
            {
                if (game_manager.CheckKey(keyhole.GetComponent<Renderer>().material.color))
                {
                    open = true;
                }
            }
        }
    }
}
