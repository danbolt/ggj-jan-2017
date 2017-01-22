
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	public float Speed = 1f;
	public string Axis = "Horizontal";

    /// <summary>
    /// Reference to the <see cref="UnityEngine.Transform"/> used for the player's 3D model. Set this in the editor.
    /// </summary>
    public Transform modelTransform = null;

    #pragma warning disable 0414
	private Rigidbody _Rigidbody;
    #pragma warning restore 0414

	private void Start()
	{
        #pragma warning disable 0414
		_Rigidbody = GetComponent<Rigidbody>();
        #pragma warning restore 0414
	}

	private void FixedUpdate()
	{
		transform.position = transform.position + MoveDirection;
		if (MoveDirection != Vector3.zero)
		{
			modelTransform.forward = Vector3.Normalize(MoveDirection);
		}
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
