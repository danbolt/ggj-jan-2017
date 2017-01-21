using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    // Use this for initialization

    public GameObject target;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        var cameraTransform = this.GetComponent<Camera>().transform;
        cameraTransform.position = new Vector3(target.transform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }
}
