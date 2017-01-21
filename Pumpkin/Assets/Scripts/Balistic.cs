using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balistic : MonoBehaviour {

    public string Direction = "Left";
    public float forceMultiplier = 0.00000005f;
    private Rigidbody body;
    private int timesApplied = 0;

    // Use this for initialization
    void Start () {
        body = this.GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        if (timesApplied < 10)
        {
            if (Direction == "Left")
            {
                body.AddForce(new Vector3(-1, 0, 0) * forceMultiplier, ForceMode.VelocityChange);
            }
            else if (Direction == "Right")
            {
                body.AddForce(new Vector3(1, 0, 0) * forceMultiplier, ForceMode.VelocityChange);
            }
            timesApplied++;

        }
       
    }
}
