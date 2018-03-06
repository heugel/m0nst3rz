using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainer : MonoBehaviour {

	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y < -5)
        {
            transform.position += new Vector3(0, 15, 0);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
	}
}
