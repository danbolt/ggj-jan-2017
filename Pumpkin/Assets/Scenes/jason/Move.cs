
using UnityEngine;

public class Move : MonoBehaviour
{
	public float Speed = 1f;
	public string Axis = "Horizontal";

	private Rigidbody _Rigidbody;

	private void Start()
	{
		_Rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		transform.position = transform.position + MoveDirection;
	}

	private Vector3 MoveDirection
	{
		get
		{
			var scale = Speed + Time.deltaTime;
			return new Vector3(
				Input.GetAxis(Axis) * scale,
				0f,
				0f);
		}
	}
}
