
using UnityEngine;

public class Pickup : MonoBehaviour
{
    #pragma warning disable 0414
	private BoxCollider _Collider;
    #pragma warning restore 0414

	private void Start()
	{
        #pragma warning disable 0414
		_Collider = GetComponent<BoxCollider>();
        #pragma warning restore 0414
	}

	private void OnTriggerEnter(Collider c)
	{
        if (c.gameObject.name == "Player")
        {
            gameObject.SetActive(false);
        }
	}
}
