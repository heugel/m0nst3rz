using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyBook : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0, .003f * Mathf.Sin(Time.time), 0);
    }
}
