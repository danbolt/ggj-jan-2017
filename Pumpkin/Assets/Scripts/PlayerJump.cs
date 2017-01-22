
using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
	public Vector3 Force = new Vector3(0f, 2f, 0f);
	public string Button = "Jump";
	public AudioClip[] JumpSounds;

	private Rigidbody _Rigidbody;
	private CapsuleCollider _Collider;
	private AudioSource _AudioSource;
	private float ColliderHeight;
	private System.Random _Random = new System.Random();
	private int LastUsedSoundIndex = -1;
    private bool inAir;


    private void Start()
	{
		_Rigidbody = GetComponent<Rigidbody>();
		_Collider = GetComponent<CapsuleCollider>();
		_AudioSource = GetComponent<AudioSource>();
		ColliderHeight = _Collider.bounds.extents.y;

        inAir = false;

    }

	private void FixedUpdate()
	{
         inAir = !IsGrounded;

        if ((Input.GetButtonDown(Button) || Input.GetButtonDown("Fire2")) && !inAir)
		{
			Jump();
			PlayJumpSound();
		}

        if (inAir)
        { 
            Vector3 vel = _Rigidbody.velocity;
            vel.y -= 20.8f * Time.deltaTime;
            _Rigidbody.velocity = vel;

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
