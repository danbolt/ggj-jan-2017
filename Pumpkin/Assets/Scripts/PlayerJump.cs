
using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
	public Vector3 Force = new Vector3(0f, 1f, 0f);
	public string Button = "Jump";
	public AudioClip[] JumpSounds;

	private Rigidbody _Rigidbody;
	private CapsuleCollider _Collider;
	private AudioSource _AudioSource;
	private float ColliderHeight;
	private System.Random _Random = new System.Random();
	private int LastUsedSoundIndex = -1;

	private void Start()
	{
		_Rigidbody = GetComponent<Rigidbody>();
		_Collider = GetComponent<CapsuleCollider>();
		_AudioSource = GetComponent<AudioSource>();
		ColliderHeight = _Collider.bounds.extents.y;
	}

	private void FixedUpdate()
	{
		if (Input.GetButtonDown(Button) && IsGrounded)
		{
			_Rigidbody.AddForce(Force, ForceMode.Impulse);

			int soundCount = JumpSounds.Length;
			if (soundCount > 0)
			{
				int randIndex = _Random.Next(0, soundCount);
				if (randIndex == LastUsedSoundIndex)
				{
					randIndex = (randIndex + 1) % soundCount;
				}

				var sound = JumpSounds[randIndex];
				_AudioSource.PlayOneShot(sound);
			}
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
