using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour {

    private bool move = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (move)
        {
            transform.position += transform.forward * Time.deltaTime * 100;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f))
            {
                move = false;

                transform.position = transform.position + (transform.forward * hit.distance);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //move = false;
    }
}
