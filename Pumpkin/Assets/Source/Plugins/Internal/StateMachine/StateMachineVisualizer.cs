// --------------------------------------------------------------------
// <copyright file="StateMachineVisualizer.cs" company="TODO: NAME">
//   Copyright (c) 2016 TODO: NAME. All rights reserved.
// </copyright>
// <summary>
//   StateMachine Visualizer editor window object.
// </summary>
// --------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

using UnityEngine;

/// <summary>     
/// StateMachine Visualizer editor window object.
/// </summary>    
public class StateMachineVisualizer : EditorWindow
{
	#region  Fields

	[SerializeField]
	private int selectedIndex = -1;

	[SerializeField]
	private Dictionary<string, object> stateMachineList;

	[SerializeField]
	private readonly List<StateNode> stateNodeList = new List<StateNode>();

	#endregion

	#region  Methods - Public

	/// <summary> Opens a new StateVisualizer Window. </summary>
	[MenuItem("Tools/State Machine Visualizer")]
	public static void ShowStateVisualizer()
	{
		// Creates the wizard for display
		StateMachineVisualizer visualizer = GetWindow(typeof(StateMachineVisualizer), false, "Show State Visualizer", true) as StateMachineVisualizer;
		if (visualizer != null)
		{
			visualizer.Refresh();
		}
	}

	#endregion

	#region  Methods - Private

	private void OnGUI()
	{
		EditorGUILayout.HelpBox("Select which state machine to visualize:", MessageType.Info, true);
		EditorGUILayout.Space();

		string selectedStateMachineName = null;
		if (this.stateMachineList != null && this.stateMachineList.Count > 0)
		{
			string[] names = new string[this.stateMachineList.Count];
			this.stateMachineList.Keys.CopyTo(names, 0);

			int newSelectedIndex = EditorGUILayout.Popup("State Machine:", Mathf.Min(this.stateMachineList.Count - 1, Mathf.Max(this.selectedIndex, 0)), names);
			if (newSelectedIndex != this.selectedIndex)
			{
				this.selectedIndex = newSelectedIndex;
			}

			selectedStateMachineName = names[this.selectedIndex];
		}
		else
		{
			EditorGUILayout.Popup("State Machine:", 0, new[] { "none" });
			this.selectedIndex = -1;
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		if (selectedStateMachineName != null)
		{
			object selectedStateMachine = this.stateMachineList[selectedStateMachineName];
			if (selectedStateMachine != null)
			{
				Type stateMachineType = selectedStateMachine.GetType();
				if (stateMachineType.IsGenericType)
				{
					Type[] genericTypeArguments = stateMachineType.GetGenericArguments();
					MethodInfo method = typeof(StateMachineVisualizer).GetMethod("LayoutStateMachineVisualizer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
					MethodInfo generic = method.MakeGenericMethod(genericTypeArguments);
					generic.Invoke(this, new[] { selectedStateMachine });
				}
			}
			else
			{
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("State Machine not instantiated!");
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
		}
	}

	private void AddStateMachinesToMasterList(object instance)
	{
		if (instance == null)
		{
			return;
		}

		Type classType = instance.GetType();
		FieldInfo[] fields = classType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

		foreach (FieldInfo fieldInfo in fields)
		{
			Type fieldType = fieldInfo.FieldType;
			if (fieldType.IsGenericType)
			{
				if (fieldType.GetGenericTypeDefinition() == typeof(StateMachine<,>))
				{
					string name = this.stateMachineList.Count.ToString() + ". " + instance.ToString() + " -> " + fieldInfo.Name;
					object stateMachineInstance = fieldInfo.GetValue(instance);
					this.stateMachineList.Add(name, stateMachineInstance);
				}
			}
		}
	}

	private void Refresh()
	{
		if (this.stateMachineList == null)
		{
			this.stateMachineList = new Dictionary<string, object>();
		}
		else
		{
			this.stateMachineList.Clear();
		}

		if (Selection.activeObject != null)
		{
			GameObject gameObj = Selection.activeObject as GameObject;
			if (gameObj)
			{
				foreach (Component comp in gameObj.GetComponentsInChildren<Component>(true))
				{
					this.AddStateMachinesToMasterList(comp);
				}
			}
			else
			{
				this.AddStateMachinesToMasterList(Selection.activeObject);
			}
		}
	}

	private void OnSelectionChange()
	{
		this.Refresh();
		this.Repaint();
	}

	private void OnInspectorUpdate()
	{
		this.Refresh();
		this.Repaint();
	}

	private void Update()
	{
		this.Repaint();
	}

	private void LayoutStateMachineVisualizer<T, K>(object target)
	{
		StateMachine<T, K> stateMachine = target as StateMachine<T, K>;

		this.stateNodeList.Clear();

		StateNode newStateNode = new StateNode();
		newStateNode.X = this.position.width * 0.25f;
		newStateNode.Y = (this.position.height * 0.5f) + 50.0f;
		newStateNode.Width = 125.0f;
		newStateNode.Height = 50.0f;
		newStateNode.Color = Color.red;
		newStateNode.Title = stateMachine != null ? stateMachine.GetState().ToString() : string.Empty;
		newStateNode.Text = string.Format("{0:0.00}", stateMachine.GetStateTime() + " sec");
		newStateNode.Action = null;
		newStateNode.Parent = stateMachine;
		this.stateNodeList.Add(newStateNode);

		List<StateMachine<T, K>.TransitionDefinition> transitionList = stateMachine.GetTransitionsForState(stateMachine.GetState());
		if (transitionList.Count > 0)
		{
			float perHeight = (this.position.height - 50.0f) / (transitionList.Count + 1);
			float currentPosY = 50.0f + perHeight;
			foreach (StateMachine<T, K>.TransitionDefinition transDef in transitionList)
			{
				StateNode newTransitionStateNode = new StateNode();
				newTransitionStateNode.X = this.position.width * 0.75f;
				newTransitionStateNode.Y = currentPosY;
				newTransitionStateNode.Width = 125.0f;
				newTransitionStateNode.Height = 50.0f;

				newTransitionStateNode.Title = transDef.StateTo.ToString();
				newTransitionStateNode.Text = null;
				newTransitionStateNode.Action = transDef.TriggerEventId;
				newTransitionStateNode.Color = Color.cyan;
				newTransitionStateNode.Parent = stateMachine;
				this.stateNodeList.Add(newTransitionStateNode);

				currentPosY += perHeight;
			}
		}

		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		Color oldColor = GUI.color;
		this.BeginWindows();

		for (int i = 0; i < this.stateNodeList.Count; ++i)
		{
			GUI.color = this.stateNodeList[i].Color;
			GUILayout.Window(i, new Rect(this.stateNodeList[i].X - (this.stateNodeList[i].Width * 0.5f), this.stateNodeList[i].Y - (this.stateNodeList[i].Height * 0.5f), this.stateNodeList[i].Width, this.stateNodeList[i].Height), this.DrawStateNodeWindow<T, K>, this.stateNodeList[i].Title);
		}

		this.EndWindows();
		GUI.color = oldColor;
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	private void DrawStateNodeWindow<T, K>(int windowId)
	{
		StateNode stateNode = this.stateNodeList[windowId];

		GUILayout.FlexibleSpace();
		Color oldColor = GUI.color;
		GUI.color = Color.white;
		GUILayout.BeginVertical();

		if (stateNode.Text != null)
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(stateNode.Text);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		if (stateNode.Action != null)
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(stateNode.Action.ToString()))
			{
				K eventId = (K)stateNode.Action;
				StateMachine<T, K> stateMachine = (StateMachine<T, K>)stateNode.Parent;
				if (stateMachine != null && eventId != null)
				{
					stateMachine.HandleEvent(eventId);
				}
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
		GUI.color = oldColor;
		GUILayout.FlexibleSpace();
	}

	#endregion

	/// <summary>
	/// Represents a single state node object.
	/// </summary>
	public class StateNode
	{
		#region  Properties - Public

		/// <summary>
		/// Gets or sets the X position value of the State Node.
		/// </summary>
		public float X { get; set; }

		/// <summary>
		///  Gets or sets the Y position of the State Node.
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		///  Gets or sets the width size of the State Node.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		///  Gets or sets the height size of the State Node.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		///  Gets or sets the text field of State Node.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the action object of State node.
		/// </summary>
		public object Action { get; set; }

		/// <summary>
		///  Gets or sets the color field of State Node.
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		///  Gets or sets the title field of State Node.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		///  Gets or sets the parent object of state Node.
		/// </summary>
		public object Parent { get; set; }

		#endregion
	}
}