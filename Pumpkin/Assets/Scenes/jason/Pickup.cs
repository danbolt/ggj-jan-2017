
using UnityEngine;

public class Pickup : MonoBehaviour
{
	private BoxCollider _Collider;

	private void Start()
	{
		_Collider = GetComponent<BoxCollider>();
	}

	private void OnTriggerEnter()
	{
		Debug.Log("triggered");
	}
}
