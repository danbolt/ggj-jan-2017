
using UnityEngine;

public class Jump : MonoBehaviour
{
	public Vector3 Force = new Vector3(0f, 1f, 0f);
	public string Button = "Jump";

	private Rigidbody _Rigidbody;
	private CapsuleCollider _Collider;
	private float ColliderHeight;

	private void Start()
	{
		_Rigidbody = GetComponent<Rigidbody>();
		_Collider = GetComponent<CapsuleCollider>();
		ColliderHeight = _Collider.bounds.extents.y;
	}

	private void FixedUpdate()
	{
		if (Input.GetButtonDown(Button) && IsGrounded)
		{
			_Rigidbody.AddForce(Force, ForceMode.Impulse);
		}
	}

	private bool IsGrounded
	{
		get
		{
			//Physics.CheckCapsule(collider.bounds.center,new Vector3(collider.bounds.center.x,collider.bounds.min.y-0.1f,collider.bounds.center.z),0.18f));
			return Physics.Raycast(transform.position, -Vector3.up, ColliderHeight + 0.05f);
		}
	}
}
