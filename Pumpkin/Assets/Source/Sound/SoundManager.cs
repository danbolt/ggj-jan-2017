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
    private AudioClip[] coinPickSfx;

    [SerializeField]
    private AudioClip wavePreSplashSfx;

    [SerializeField]
    private AudioClip waveSplashSfx;

    [SerializeField]
    private AudioClip barrelShotSfx;

    [SerializeField]
	private AudioSource sfxSource;

    [SerializeField]
    private AudioSource waterSfxSource;

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
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

	private void Start()
	{
		this.musicAudioSource.clip = this.backgroundMusic[0];
        this.musicAudioSource.loop = true;
        this.musicAudioSource.Play();
	}

    public void PlayCoinSound()
    {
        int randomIndex = Random.Range(0, coinPickSfx.Length);
        this.sfxSource.clip = (this.coinPickSfx[randomIndex]);
        this.sfxSource.Play();
    }

    public void PlayWavePreSplash()
    {
        this.waterSfxSource.PlayOneShot(this.wavePreSplashSfx);
    }

    public void PlayWaveSplash()
    {
        this.waterSfxSource.PlayOneShot(this.waveSplashSfx);
    }

    public void PlayBarrelShotSound(AudioSource source)
    {
        source.PlayOneShot(this.barrelShotSfx);
    }

    #endregion
}