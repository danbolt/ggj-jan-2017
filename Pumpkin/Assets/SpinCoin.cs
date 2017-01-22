using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCoin : MonoBehaviour {

    private Transform parentObjectTransform;

    public float TimeScale = 1.0f;

	/// <summary>
    /// Called once when the frame begins.
    /// </summary>
	void Start () {
        parentObjectTransform = GetComponent<Transform>();
	}
	
	/// <summary>
    /// Called once per physics update (zero or more times per frame).
    /// </summary>
	void FixedUpdate () {
        parentObjectTransform.Rotate(0.0f, Time.deltaTime * TimeScale, 0.0f);
	}
}
