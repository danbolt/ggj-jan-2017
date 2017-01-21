// --------------------------------------------------------------------
// <copyright file="StateMachine.cs" company="TODO: NAME">
//   Copyright (c) 2016 TODO: NAME. All rights reserved.
// </copyright>
// <summary>
//   Defines the implementation of the State Machine.
// </summary>
// --------------------------------------------------------------------

using System.Collections.Generic;

using UnityEngine;

/// <summary>Defines the implementation of the State Machine.</summary>
/// <typeparam name="T">First generic type parameter.</typeparam>
/// <typeparam name="TK">Second generic type parameter.</typeparam>
public class StateMachine<T, TK>
{
	#region  Fields

	private T currentState;

	private float currentStateTime = 0.0f;

	private T initialState;

	private bool isRunning = false;

	private bool isStarted = false;

	private T lastState;

	private Dictionary<T, StateDefinition> stateData;

	private Dictionary<T, List<TransitionDefinition>> transitionData;

	private ValueStore valueStore;

	#endregion

	#region  Constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="StateMachine{T,TK}"/> class.
	/// </summary>
	public StateMachine()
	{
		this.stateData = new Dictionary<T, StateDefinition>();
		this.transitionData = new Dictionary<T, List<TransitionDefinition>>();
		this.valueStore = new ValueStore();
		this.isStarted = false;
		this.isRunning = false;
		this.lastState = default(T);
		this.currentState = default(T);
		this.initialState = default(T);
	}

	#endregion

	#region Delegates

	/// <summary>
	///	OnEnter state callback delegate.
	/// </summary>
	/// <param name="stateMachine"> The StateMachine that triggered the Callback</param>
	public delegate void CallbackStateOnEnter(StateMachine<T, TK> stateMachine);

	/// <summary>
	///	OnExit state callback delegate.
	/// </summary>
	/// <param name="stateMachine"> The StateMachine that triggered the Callback</param>
	public delegate void CallbackStateOnExit(StateMachine<T, TK> stateMachine);

	/// <summary>
	///	OnUpdate state callback delegate.
	/// </summary>
	/// <param name="stateMachine"> The StateMachine that triggered the Callback</param>
	public delegate void CallbackStateOnUpdate(StateMachine<T, TK> stateMachine);

	/// <summary>
	/// Check if a transition can be triggered between two states. 
	/// </summary>
	/// <param name="stateMachine">The StateMachine that triggered the transition check.</param>
	/// <param name="to">The state we want to transition to.</param>
	/// <param name="from">The state we are transitioning from.</param>
	/// <returns>True if a transition can be triggered between the states.</returns>
	public delegate bool CallbackTransitionCanTrigger(StateMachine<T, TK> stateMachine, T to, T from);

	/// <summary>
	/// Post transition event callback.
	/// </summary>
	/// <param name="stateMachine">The StateMachine that triggered the transition</param>
	/// <param name="to">The state we transitioned to.</param>
	/// <param name="from">The state we transitioned from.</param>
	public delegate void CallbackTransitionPost(StateMachine<T, TK> stateMachine, T to, T from);

	/// <summary>
	/// Pre transition event callback.
	/// </summary>
	/// <param name="stateMachine">The StateMachine that triggered the Transition.</param>
	/// <param name="to">The state we transitioned to.</param>
	/// <param name="from">The state we transitioned from.</param>
	public delegate void CallbackTransitionPre(StateMachine<T, TK> stateMachine, T to, T from);

	/// <summary>
	/// Check if a transition should be triggered between two states. 
	/// </summary>
	/// <param name="stateMachine">The StateMachine that triggered the should trigger transition check.</param>
	/// <param name="to">The state we want to transitioned to.</param>
	/// <param name="from">The state we are transitioning from.</param>
	/// <returns>True if the transition should be triggered.</returns>
	public delegate bool CallbackTransitionShouldTrigger(StateMachine<T, TK> stateMachine, T to, T from);

	#endregion

	#region  Methods - Public

	/// <summary>Start the State Machine.</summary>
	/// <param name="startingState">The initial state of the State Machine.</param>
	public void Start(T startingState)
	{
		if (!this.isStarted)
		{
			StateDefinition initialStateData;
			bool validState = this.stateData.TryGetValue(startingState, out initialStateData);
			if (validState)
			{
				this.isStarted = true;
				this.isRunning = true;
				this.lastState = default(T);
				this.initialState = startingState;
				this.currentState = this.initialState;

				this.valueStore.Reset();
				if (initialStateData.OnEnter != null)
				{
					initialStateData.OnEnter(this);
				}
			}
			else
			{
				Debug.Log("Invalid start state data when starting run");
			}
		}
		else
		{
			Debug.Log("Statemachine has already been started");
		}
	}

	/// <summary>
	/// Start the StateMachine.
	/// </summary>
	public void Start()
	{
		this.Start(this.initialState);
	}

	/// <summary>Set the initial state of the statemachine.</summary>
	/// <param name="startingState">The Initial state to load in.</param>
	public void SetInitialState(T startingState)
	{
		if (!this.isStarted)
		{
			if (this.stateData.ContainsKey(startingState))
			{
				this.initialState = startingState;
			}
			else
			{
				Debug.Log("Invalid start state data specified");
			}
		}
		else
		{
			Debug.Log("Statemachine has already been started");
		}
	}

	/// <summary>
	/// Resume running the StateMachine.
	/// </summary>
	public void Resume()
	{
		if (this.isStarted)
		{
			this.isRunning = true;
		}
		else
		{
			Debug.Log("State machine has not yet been started!");
		}
	}

	/// <summary>
	/// Stop the StateMachine.
	/// </summary>
	public void Stop()
	{
		this.isRunning = false;
	}

	/// <summary>Handle events</summary>
	/// <param name="eventId">the Id of the event to handle.</param>
	public void HandleEvent(TK eventId)
	{
		if (this.isRunning)
		{
			List<TransitionDefinition> transitionList;
			bool validTransitionList = this.transitionData.TryGetValue(this.currentState, out transitionList);
			if (validTransitionList)
			{
				bool isEventInTransitionList = false;
				for (int i = 0; i < transitionList.Count; ++i)
				{
					if (eventId.Equals(transitionList[i].TriggerEventId))
					{
						isEventInTransitionList = true;
						bool canTrigger = (transitionList[i].CanTrigger == null) || transitionList[i].CanTrigger(this, transitionList[i].StateFrom, transitionList[i].StateTo);
						if (canTrigger)
						{
							if (transitionList[i].OnPre != null)
							{
								transitionList[i].OnPre(this, transitionList[i].StateFrom, transitionList[i].StateTo);
							}

							T nextState = transitionList[i].StateTo;

							// Debug.Log(currentState + " -> " + nextState + " ( Event: " + eventId.ToString() + " )"); 
							this.ChangeState(nextState);

							if (transitionList[i].OnPost != null)
							{
								transitionList[i].OnPost(this, transitionList[i].StateFrom, transitionList[i].StateTo);
							}

							break;
						}
					}
				}

				if (!isEventInTransitionList)
				{
					Debug.LogWarning("Event - (" + eventId.ToString() + ") not handled by the current state (" + this.currentState.ToString() + ")");
				}
			}
		}
	}

	/// <summary>
	/// Gets the current state of the StateMachine
	/// </summary>
	/// <returns>the current state value.</returns>
	public T GetState()
	{
		return this.currentState;
	}

	/// <summary>
	/// Gets the previous state of the StateMachine.
	/// </summary>
	/// <returns>the previous state value.</returns>
	public T GetLastState()
	{
		return this.lastState;
	}

	/// <summary>
	/// Get the current state time.
	/// </summary>
	/// <returns>The current state time value.</returns>
	public float GetStateTime()
	{
		return this.currentStateTime;
	}

	/// <summary>
	/// Gets the ValueStore associated with this StateMachine.
	/// </summary>
	/// <returns>the ValueStore value</returns>
	public ValueStore GetValueStore()
	{
		return this.valueStore;
	}

	/// <summary>Add a new state to the StateMachine</summary>
	/// <param name="stateName">the name of the state</param>
	/// <param name="onEnter">the callback to trigger on state enter</param>
	/// <param name="onUpdate">the callback to trigger while on the state</param>
	/// <param name="onExit">the callback to trigger on state exit</param>
	public void AddState(T stateName, CallbackStateOnEnter onEnter, CallbackStateOnUpdate onUpdate, CallbackStateOnExit onExit)
	{
		if (!this.stateData.ContainsKey(stateName))
		{
			this.stateData[stateName] = new StateDefinition(stateName, onEnter, onUpdate, onExit);
		}
		else
		{
			Debug.Log("State already exists!");
		}
	}

	/// <summary>Add a transition between states to the State Machine.</summary>
	/// <param name="stateFrom">The state we are transitioning from.</param>
	/// <param name="stateTo">The state we are transitioning to.</param>
	/// <param name="onPre">The callback to be triggered before transitioning.</param>
	/// <param name="onPost">The callback to be triggered after transitioning.</param>
	/// <param name="canTrigger">The callback to check if we can transition between ttwo states.</param>
	/// <param name="shouldTrigger">The callback that should be triggered if we are transitioning between two states.</param>
	/// <param name="triggerEventId">The id of the event that triggers the transition.</param>
	public void AddTransition(T stateFrom, T stateTo, CallbackTransitionPre onPre, CallbackTransitionPost onPost, CallbackTransitionCanTrigger canTrigger, CallbackTransitionShouldTrigger shouldTrigger, TK triggerEventId)
	{
		bool sourceStateExists = this.stateData.ContainsKey(stateFrom);
		bool destinationStateExists = this.stateData.ContainsKey(stateTo);

		if (sourceStateExists && destinationStateExists)
		{
			List<TransitionDefinition> transitionList;
			if (this.transitionData.ContainsKey(stateFrom))
			{
				transitionList = this.transitionData[stateFrom];
			}
			else
			{
				transitionList = new List<TransitionDefinition>();
			}

			transitionList.Add(new TransitionDefinition(stateFrom, stateTo, onPre, onPost, canTrigger, shouldTrigger, triggerEventId));
			this.transitionData[stateFrom] = transitionList;
		}
		else
		{
			Debug.Log("One of the transition states doesn't not exist");
		}
	}

	/// <summary>Add a global transition to the StateMachine.</summary>
	/// <param name="stateTo">The state to transition to.</param>
	/// <param name="onPre">The callback to be triggered before transitioning.</param>
	/// <param name="onPost">The callback to be triggered after transitioning.</param>
	/// <param name="canTrigger">The callback to check if we can transition between ttwo states.</param>
	/// <param name="shouldTrigger">The callback that should be triggered if we are transitioning between two states.</param>
	/// <param name="triggerEventId">The id of the event that triggers the transition.</param>
	public void AddGlobalTransition(T stateTo, CallbackTransitionPre onPre, CallbackTransitionPost onPost, CallbackTransitionCanTrigger canTrigger, CallbackTransitionShouldTrigger shouldTrigger, TK triggerEventId)
	{
		bool destinationStateExists = this.stateData.ContainsKey(stateTo);
		if (destinationStateExists)
		{
			foreach (KeyValuePair<T, StateDefinition> kvp in this.stateData)
			{
				this.AddTransition(kvp.Value.StateName, stateTo, onPre, onPost, canTrigger, shouldTrigger, triggerEventId);
			}
		}
		else
		{
			Debug.Log("Destination of the transition states doesn't not exist");
		}
	}

	/// <summary>StateMachine's Update loop.</summary>
	/// <param name="elapsedDeltaTime">the amount of time elapsed since the previous update.</param>
	public void Update(float elapsedDeltaTime)
	{
		if (this.isRunning)
		{
			StateDefinition currentStateData;
			bool validState = this.stateData.TryGetValue(this.currentState, out currentStateData);

			if (validState)
			{
				this.currentStateTime += elapsedDeltaTime;

				if (currentStateData.OnUpdate != null)
				{
					currentStateData.OnUpdate(this);
				}

				List<TransitionDefinition> transitionList;
				bool validTransitionList = this.transitionData.TryGetValue(this.currentState, out transitionList);
				if (validTransitionList)
				{
					for (int i = 0; i < transitionList.Count; ++i)
					{
						bool canTrigger = (transitionList[i].CanTrigger == null) || transitionList[i].CanTrigger(this, transitionList[i].StateFrom, transitionList[i].StateTo);
						if (canTrigger)
						{
							if (transitionList[i].ShouldTrigger != null)
							{
								bool shouldTrigger = transitionList[i].ShouldTrigger(this, transitionList[i].StateFrom, transitionList[i].StateTo);
								if (shouldTrigger)
								{
									if (transitionList[i].OnPre != null)
									{
										transitionList[i].OnPre(this, transitionList[i].StateFrom, transitionList[i].StateTo);
									}

									T nextState = transitionList[i].StateTo;

									// Debug.Log(currentState + " -> " + nextState + "(Condition)"); 
									this.ChangeState(nextState);

									if (transitionList[i].OnPost != null)
									{
										transitionList[i].OnPost(this, transitionList[i].StateFrom, transitionList[i].StateTo);
									}

									break;
								}
							}
						}
					}
				}
			}
			else
			{
				Debug.Log("Invalid state data");
			}
		}
	}

	/// <summary>Gets the transitions available for a given state.</summary>
	/// <param name="state">The state to cull its available transitions.</param>
	/// <returns>The list of available transitions.</returns>
	public List<TransitionDefinition> GetTransitionsForState(T state)
	{
		List<TransitionDefinition> transitionList;
		this.transitionData.TryGetValue(state, out transitionList);
		return transitionList;
	}

	#endregion

	#region  Methods - Private

	/// <summary>The implementation that occurs when changing states.</summary>
	/// <param name="nextState">The state to change to.</param>
	private void ChangeState(T nextState)
	{
		StateDefinition currentStateData;
		StateDefinition nextStateData;
		bool validCurrentState = this.stateData.TryGetValue(this.currentState, out currentStateData);
		bool validNextState = this.stateData.TryGetValue(nextState, out nextStateData);

		if (validCurrentState && validNextState)
		{
			if (currentStateData.OnExit != null)
			{
				currentStateData.OnExit(this);
			}

			this.lastState = this.currentState;
			this.currentState = nextState;

			this.currentStateTime = 0.0f;
			this.valueStore.Reset();

			if (nextStateData.OnEnter != null)
			{
				nextStateData.OnEnter(this);
			}
		}
		else
		{
			Debug.Log("Invalid state data for current or new state");
		}
	}

	#endregion

	/// <summary>
	/// StateDefinition Object
	/// </summary>
	public class StateDefinition
	{
		#region  Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StateDefinition"/> class with default values.
		/// </summary>
		public StateDefinition()
			: this(default(T), null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="StateDefinition"/> class.</summary>
		/// <param name="stateName">Name of the State</param>
		/// <param name="onEnter">Callback to trigger on enter.</param>
		/// <param name="onUpdate">Callback to trigger on update</param>
		/// <param name="onExit">Callback to trigger on Exit</param>
		public StateDefinition(T stateName, CallbackStateOnEnter onEnter, CallbackStateOnUpdate onUpdate, CallbackStateOnExit onExit)
		{
			this.StateName = stateName;
			this.OnEnter = onEnter;
			this.OnUpdate = onUpdate;
			this.OnExit = onExit;
		}

		#endregion

		#region  Properties - Public

		/// <summary>Gets or sets the State On Enter callback.</summary>
		public CallbackStateOnEnter OnEnter { get; set; }

		/// <summary>Gets or sets the State On Exit callback.</summary>
		public CallbackStateOnExit OnExit { get; set; }

		/// <summary>Gets or sets the State callback to be triggered on Update.</summary>
		public CallbackStateOnUpdate OnUpdate { get; set; }

		/// <summary>Gets or sets the state name.</summary>
		public T StateName { get; set; }

		#endregion
	}

	/// <summary>
	/// Transition Definition object.
	/// </summary>
	public class TransitionDefinition
	{
		#region  Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TransitionDefinition"/> class.
		/// </summary>
		public TransitionDefinition()
			: this(default(T), default(T), null, null, null, null, default(TK))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="TransitionDefinition"/> class.</summary>
		/// <param name="stateFrom">State being transitioned from.</param>
		/// <param name="stateTo">State being transitioned to.</param>
		/// <param name="onPre">Callback to trigger before the transition.</param>
		/// <param name="onPost">Callback to trigger after the transition. </param>
		/// <param name="canTrigger">Callback to check if a transition can be triggered.</param>
		/// <param name="shouldTrigger">Callback to trigger if a transition should be triggered.</param>
		/// <param name="triggerEventId">Event Id that triggers the transition</param>
		public TransitionDefinition(T stateFrom, T stateTo, CallbackTransitionPre onPre, CallbackTransitionPost onPost, CallbackTransitionCanTrigger canTrigger, CallbackTransitionShouldTrigger shouldTrigger, TK triggerEventId)
		{
			this.StateFrom = stateFrom;
			this.StateTo = stateTo;
			this.OnPre = onPre;
			this.OnPost = onPost;
			this.CanTrigger = canTrigger;
			this.ShouldTrigger = shouldTrigger;
			this.TriggerEventId = triggerEventId;
		}

		#endregion

		#region  Properties - Public

		/// <summary>Gets or sets the trigger that checks if a transition can trigger between the states</summary>
		public CallbackTransitionCanTrigger CanTrigger { get; set; }

		/// <summary>Gets or sets the callback to trigger post transition.</summary>
		public CallbackTransitionPost OnPost { get; set; }

		/// <summary>Gets or sets the callback to trigger pre transition.</summary>
		public CallbackTransitionPre OnPre { get; set; }

		/// <summary>Gets or sets the calback that should trigger when transitioning.</summary>
		public CallbackTransitionShouldTrigger ShouldTrigger { get; set; }

		/// <summary>Gets or sets the state to transition from.</summary>
		public T StateFrom { get; set; }

		/// <summary>Gets or sets the state to transition to.</summary>
		public T StateTo { get; set; }

		/// <summary>Gets or sets the trigger event id.</summary>
		public TK TriggerEventId { get; set; }

		#endregion
	}

	/// <summary>
	/// Global Transition definition Object
	/// </summary>
	public class GlobalTransitionDefinition
	{
		#region  Constructors

		/// <summary>Initializes a new instance of the <see cref="GlobalTransitionDefinition"/> class.</summary>
		/// <param name="stateTo">State being transitioned to.</param>
		/// <param name="onPre">Callback to trigger before the transition.</param>
		/// <param name="onPost">Callback to trigger after the transition. </param>
		/// <param name="canTrigger">Callback to check if a transition can be triggered.</param>
		/// <param name="shouldTrigger">Callback to trigger if a transition should be triggered.</param>
		/// <param name="triggerEventId">Event Id that triggers the transition</param>
		public GlobalTransitionDefinition(T stateTo, CallbackTransitionPre onPre, CallbackTransitionPost onPost, CallbackTransitionCanTrigger canTrigger, CallbackTransitionShouldTrigger shouldTrigger, TK triggerEventId)
		{
			this.StateTo = stateTo;
			this.OnPre = onPre;
			this.OnPost = onPost;
			this.CanTrigger = canTrigger;
			this.ShouldTrigger = shouldTrigger;
			this.TriggerEventId = triggerEventId;
		}

		#endregion

		#region  Properties - Public

		/// <summary>Gets or sets the trigger that checks if a transition can trigger between the states</summary>
		public CallbackTransitionCanTrigger CanTrigger { get; set; }

		/// <summary>Gets or sets the callback to trigger post transition.</summary>
		public CallbackTransitionPost OnPost { get; set; }

		/// <summary>Gets or sets the callback to trigger pre transition.</summary>
		public CallbackTransitionPre OnPre { get; set; }

		/// <summary>Gets or sets the calback that should trigger when transitioning.</summary>
		public CallbackTransitionShouldTrigger ShouldTrigger { get; set; }

		/// <summary>Gets or sets the state to transition to.</summary>
		public T StateTo { get; set; }

		/// <summary>Gets or sets the trigger event id.</summary>
		public TK TriggerEventId { get; set; }

		#endregion
	}
}