
using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
	public Vector3 Force = new Vector3(0f, 1f, 0f);
	public Vector3 ExtraDrag = Vector3.zero;
	public string Button = "Jump";
	public string Button2 = "Fire2";
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
		if (!IsGrounded)
		{
			// kludge to account for the weird gravity scale
			_Rigidbody.velocity = _Rigidbody.velocity + ExtraDrag * Time.deltaTime;
		}
		else if (Input.GetButtonDown(Button) || Input.GetButtonDown(Button2))
		{
			Jump();
			PlayJumpSound();
		}
	}

	private bool IsGrounded
	{
		get
		{
			//Physics.CheckCapsule(collider.bounds.center,new Vector3(collider.bounds.center.x,collider.bounds.min.y-0.1f,collider.bounds.center.z),0.18f));
			return Physics.Raycast(transform.position, -Vector3.up, ColliderHeight + 0.02f);
		}
	}

	private void Jump()
	{
		_Rigidbody.velocity = Vector3.zero;
		_Rigidbody.angularVelocity = Vector3.zero;
		_Rigidbody.AddForce(Force, ForceMode.Impulse);
	}

	private void PlayJumpSound()
	{
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
