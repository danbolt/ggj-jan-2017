// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundManager.cs" company="TODO: NAME">
//   Copyright (c) 2016 TODO: NAME. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

/// <summary>The sound manager.</summary>
public class SoundManager : MonoBehaviour
{
	#region Static Fields and Constants

	private static SoundManager instance;

	#endregion

	#region  Fields

	[SerializeField]
	private AudioClip[] backgroundMusic;

	[SerializeField]
	private AudioClip bannerNotificationSfx;

	// UI SFXs
	[SerializeField]
	private AudioClip buttonPressSfx;

	[SerializeField]
	private AudioSource loopingSfxSource;

	[SerializeField]
	private AudioSource musicAudioSource;

	[SerializeField]
	private AudioClip scoreCalulationSfx;

	[SerializeField]
	private AudioSource sfxSource;

	#endregion

	#region  Properties - Public

	/// <summary>Gets the instance.</summary>
	public static SoundManager Instance
	{
		get
		{
			return instance;
		}
	}

	#endregion

	#region  Methods - Public

	/// <summary>The play button click.</summary>
	public void PlayButtonClick()
	{
		this.sfxSource.PlayOneShot(this.buttonPressSfx);
	}

	/// <summary>The play banner notification sfx.</summary>
	public void PlayBannerNotificationSfx()
	{
		this.sfxSource.PlayOneShot(this.bannerNotificationSfx);
	}

	#endregion

	#region  Methods - Private

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		this.musicAudioSource.clip = this.backgroundMusic[0];
		this.musicAudioSource.Play();
	}

	#endregion
}