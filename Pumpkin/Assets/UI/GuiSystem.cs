// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GUISystem.cs" company="TODO: NAME">
//   Copyright (c) 2016 TODO: NAME. All rights reserved.
// </copyright>
// <summary>
//   Defines the GUISystem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

/// <summary>The gui system.</summary>
public class GuiSystem : MonoBehaviour
{
	#region Static Fields and Constants

	private static GuiSystem instance;

	#endregion

	#region  Fields

	[SerializeField]
	private Camera systemCamera;

	[SerializeField]
	private UIPanel systemPanel;

	[SerializeField]
	private UIRoot systemRoot;

	#endregion

	#region  Properties - Public

	/// <summary>Gets the instance.</summary>
	public static GuiSystem Instance
	{
		get
		{
			return instance;
		} 
	}

	/// <summary>Gets the gui camera.</summary>
	public Camera GuiCamera
	{
		get
		{
			return this.systemCamera;
		}
	}

	/// <summary>Gets the gui root.</summary>
	public UIRoot GuiRoot
	{
		get
		{
			return this.systemRoot;
		}
	}

	/// <summary>Gets the gui panel.</summary>
	public UIPanel GuiPanel
	{
		get
		{
			return this.systemPanel;
		}
	}

	#endregion

	#region  Methods - Private

	private void Awake()
	{
		instance = this;
	}

	#endregion
}