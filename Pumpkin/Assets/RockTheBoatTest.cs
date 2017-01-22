using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTheBoatTest : MonoBehaviour {

    private string dirction = "Up";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log(transform.rotation);
        if (dirction == "Up")
        {
            if (transform.rotation.x > -0.05 )
            {
                transform.RotateAround(Vector3.zero, new Vector3(0, 0, 1), 20 * Time.deltaTime);
            }
            else
            {
                dirction = "Down";
            }
        }
        if (dirction == "Down")
        {
            if (transform.rotation.x < 0.05 )
            {
                transform.RotateAround(Vector3.zero, new Vector3(0, 0, -1), 20 * Time.deltaTime);
            }
            else
            {
                dirction = "Up";
            }
        }
    }
}
