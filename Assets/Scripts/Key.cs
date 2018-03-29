using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    [SerializeField] float spin_speed = 5.0f;

    private GameManager game_manager;

    // Use this for initialization
    void Start ()
    {
        game_manager = GameObject.Find("GameManager").GetComponent<GameManager>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate((Vector3.up * Time.deltaTime) * spin_speed);
    }

    void OnTriggerEnter(Collider other)
    {
        game_manager.CollectKey(this.gameObject.GetComponent<Renderer>().material.color);

        Destroy(this.gameObject);
    }
}
