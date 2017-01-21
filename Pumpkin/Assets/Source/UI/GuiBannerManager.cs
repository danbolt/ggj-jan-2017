// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuiBannerManager.cs" company="TODO: NAME">
//   Copyright (c) 2016 TODO: NAME. All rights reserved.
// </copyright>
// <summary>
//   The gui banner manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using UnityEngine;

/// <summary>The gui banner manager.</summary>
public class GuiBannerManager : MonoBehaviour
{
	#region Static Fields and Constants

	private static GuiBannerManager instance;

	#endregion

	#region  Fields

	private Queue<BannerItem> bannerQueue = new Queue<BannerItem>();

	[SerializeField]
	private GuiBanner[] banners;

	[SerializeField]
	private UILabel preCacheFontLabel;

	#endregion

	#region Enums

	/// <summary>The banner Type.</summary>
	public enum BannerType
	{
		/// <summary>The test banner.</summary>
		TestBanner1,
	}

	#endregion

	#region  Properties - Public

	/// <summary>Gets the instance of the GuiBannerManager.</summary>
	public static GuiBannerManager Instance
	{
		get
		{
			return instance;
		}
	}

	#endregion

	#region  Methods - Public

	/// <summary>The show banner.</summary>
	/// <param name="banner">The banner.</param>
	/// <param name="description">The description.</param>
	public void ShowBanner(BannerType banner, string description)
	{
		BannerItem item = new BannerItem() { Type = banner, Description = description };
		this.bannerQueue.Enqueue(item);

		int curBannerIndex = (int)this.bannerQueue.Peek().Type;
		this.banners[curBannerIndex].ShowBanner(description);
	}

	#endregion

	#region  Methods - Private

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		foreach (GuiBanner banner in this.banners)
		{
			banner.Callback = this.RemoveBanner;
		}
	}

	private void RemoveBanner()
	{
		if (this.bannerQueue.Count > 0)
		{
			this.bannerQueue.Dequeue();

			if (this.bannerQueue.Count > 0)
			{
				BannerItem bannerItem = this.bannerQueue.Peek();

				int curBannerIndex = (int)bannerItem.Type;
				string description = bannerItem.Description;
				this.banners[curBannerIndex].ShowBanner(description);
			}
		}
	}

	#endregion

	private class BannerItem
	{
		#region  Fields

		public string Description { get; set; }

		public BannerType Type { get; set; }

		#endregion
	}
}