using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balistic : MonoBehaviour {

    public string Direction = "Left";
    public float forceMultiplier = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Direction == "Left")
        {
            this.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * forceMultiplier, ForceMode.Impulse);
        }
        else if (Direction == "Right")
        {
            this.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * forceMultiplier, ForceMode.Impulse);
        }
    }
}
