using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTheBoatTest : MonoBehaviour {

    private string dirction = "Up";
    private WaveDef def;

	// Use this for initialization
	void Start () {
		
	}

    public void SetRocking(WaveDef def)
    {
        this.def = def;

    }

    // Update is called once per frame
    void Update () {

        Debug.Log(transform.rotation);
        if (this.def != null)
        {
            if (dirction == "Up")
            {
                if (transform.rotation.x > -def.boatMaxAngle)
                {
                    transform.RotateAround(Vector3.zero, new Vector3(0, 0, 1), def.boatRockingSpeed * Time.deltaTime);
                }
                else
                {
                    dirction = "Down";
                }
            }
            if (dirction == "Down")
            {
                if (transform.rotation.x < def.boatMaxAngle)
                {
                    transform.RotateAround(Vector3.zero, new Vector3(0, 0, -1), def.boatRockingSpeed * Time.deltaTime);
                }
                else
                {
                    dirction = "Up";
                }
            }
        }
    }
}
