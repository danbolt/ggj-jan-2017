// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuiBanner.cs" company="TODO: NAME">
//   Copyright (c) 2016 TODO: NAME. All rights reserved.
// </copyright>
// <summary>
//   Defines the Implementation of the GuiBanner Object.
//   Use gui banners to display information modally to the user.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

/// <summary>
/// Defines the Implementation of the GuiBanner Object.
/// Use gui banners to display information modally to the user.
/// </summary>
public class GuiBanner : MonoBehaviour
{
	#region Static Fields and Constants

	private const float DisplayTime = 1.5f;

	#endregion

	#region  Fields

	private EventDelegate.Callback callback;

	[SerializeField]
	private UILabel descriptionLabel;

	private bool finishedForward = false;

	[SerializeField]
	private GuiBannerManager guiBannerManager;

	private bool showingBanner = false;

	private float timeDelta = 0.0f;

	[SerializeField]
	private UIPlayTween tweener;

	[SerializeField]
	private TweenTransform tweenTransform;

	#endregion

	#region  Properties - Public

	/// <summary>Sets the callback.</summary>
	public EventDelegate.Callback Callback
	{
		set
		{
			this.callback = value;
		}
	}

	#endregion

	#region  Methods - Public

	/// <summary>TODO The show banner.</summary>
	/// <param name="description">TODO The description.</param>
	public void ShowBanner(string description)
	{
		if (!this.showingBanner)
		{
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.PlayBannerNotificationSfx();
			}

			this.timeDelta = 0.0f;
			this.showingBanner = true;
			this.finishedForward = true;
			this.descriptionLabel.text = description;
			EventDelegate.Add(this.tweener.onFinished, this.FinishedForward, true);

			this.tweener.Play(true);
		}
	}

	#endregion

	#region  Methods - Private

	private void HideBanner()
	{
		this.finishedForward = false;
		EventDelegate.Add(this.tweener.onFinished, this.RemoveBanner, true);

		this.tweener.Play(false);
	}

	private void Update()
	{
		if (this.showingBanner)
		{
			this.timeDelta += Time.deltaTime;

			if (this.timeDelta > DisplayTime && this.finishedForward)
			{
				this.HideBanner();
			}
		}
	}

	private void RemoveBanner()
	{
		this.showingBanner = false;
		this.callback();
	}

	private void FinishedForward()
	{
		this.finishedForward = true;
	}

	#endregion
}