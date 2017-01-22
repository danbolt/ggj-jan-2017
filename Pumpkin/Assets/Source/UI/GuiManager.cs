// --------------------------------------------------------------------
// <copyright file="GuiManager.cs" company="TODO: NAME">
//   Copyright (c) 2016 TODO: NAME. All rights reserved.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>The gui manager.</summary>
public class GuiManager : MonoBehaviour
{
	#region Static Fields and Constants

	/// <summary>The gui system path.</summary>
	private const string GuiSystemPath = "Prefabs/UI/";

	private static GuiManager instance;


	#endregion

	#region  Fields

	private GameObject currentGuiPanel; // This will hold the root of the window we are currently in

	private GameObject currentGuiPopup;

	private float gameTimePassed = 0.0f;

	private GameObject guiGameObject; // this is the gui game object we will be pushing elements to.

	[SerializeField]
	private bool isolatedGuiFlow = false;

	private StateMachine<GuiStates, GuiStateEvents> mainGuiStateMachine;

	[SerializeField]
	private bool skipToGameplay = false;

	private bool tutorialRun;

	#endregion

	#region Enums

	private enum GuiStateEvents
	{
		/// <summary>The initialized gui.</summary>
		InitializedGui, 

		/// <summary>The display main menu.</summary>
		DisplayMainMenu, 

		/// <summary>The resume gameplay.</summary>
		ResumeGameplay, 

		/// <summary>The pause gameplay.</summary>
		PauseGameplay, 

		/// <summary>The return to menu.</summary>
		ReturnToMenu, 

		/// <summary>The dismiss popup.</summary>
		DismissPopup, 

		/// <summary>The dismiss pause settings popup.</summary>
		DismissPauseSettingsPopup, 

		/// <summary>The show settings popup.</summary>
		ShowSettingsPopup, 

		/// <summary>The show end game screen.</summary>
		ShowEndGameScreen, 

		/// <summary>The enter gameplay.</summary>
		EnterGameplay, 

		/// <summary>The skip to gameplay.</summary>
		SkipToGameplay
	}

	private enum GuiStates
	{
		/// <summary>The uninitialized.</summary>
		Uninitialized, 

		/// <summary>The initialized.</summary>
		Initialized, 

		/// <summary>The main menu.</summary>
		MainMenu, 

		/// <summary> The Loading Screen state. </summary>
		Loading, 

		/// <summary> The hardness minigame state. </summary>
		HardnessMinigame, 

		/// <summary> The resonance minigame state. </summary>
		ResonanceMinigame, 

		/// <summary> The sharpness minigame state. </summary>
		SharpnessMinigame, 

		/// <summary>The gameplay.</summary>
		Gameplay, 

		/// <summary>The starting gameplay.</summary>
		StartingGameplay, 

		/// <summary>The gameplay paused.</summary>
		GameplayPaused, 

		/// <summary>The gameplay resuming.</summary>
		GameplayResuming, 

		/// <summary>The gameplay post game.</summary>
		GameplayPostGame, 

		/// <summary>The gameplay high score.</summary>
		GameplayHighScore, 

		/// <summary>The settings.</summary>
		Settings
	}

	#endregion

	#region  Properties - Public

	/// <summary>Gets the instance.</summary>
	public static GuiManager Instance
	{
		get
		{
			return instance;
		}
	}

	/// <summary>Gets or sets a value indicating whether initial gameplay run.</summary>
	public bool TutorialRun
	{
		get
		{
			return this.tutorialRun;
		}

		set
		{
			this.tutorialRun = value;
		}
	}

	/// <summary>Gets the gameplay time passed.</summary>
	public float GameplayTimePassed
	{
		get
		{
			return this.gameTimePassed;
		}
	}

	#endregion

	#region  Methods - Public

	/// <summary>The is paused.</summary>
	/// <returns>The <see cref="bool"/>.</returns>
	public bool IsPaused()
	{
		if (this.mainGuiStateMachine != null)
		{
			return this.mainGuiStateMachine.GetState() == GuiStates.GameplayResuming;
		}

		return false;
	}

	/// <summary>The show banner.</summary>
	/// <param name="bannerType">The banner type.</param>
	/// <param name="description">The description.</param>
	public void ShowBanner(GuiBannerManager.BannerType bannerType, string description)
	{
		GuiBannerManager.Instance.ShowBanner(bannerType, description);
	}

	/// <summary>The trigger gameplay pause.</summary>
	public void TriggerGameplayPause()
	{
		if (this.mainGuiStateMachine.GetState() == GuiStates.Gameplay)
		{
			this.mainGuiStateMachine.HandleEvent(GuiStateEvents.PauseGameplay);
		}
	}

	/// <summary>The trigger gameplay start.</summary>
	public void TriggerGameplayStart()
	{
		if (this.mainGuiStateMachine.GetState() == GuiStates.StartingGameplay)
		{
			// Modify this so its an actual start and not a game unpause
			if (!this.isolatedGuiFlow)
			{
				// Trigger gameplay start hook here.
			}
		}
		else if (this.mainGuiStateMachine.GetState() == GuiStates.MainMenu || this.mainGuiStateMachine.GetState() == GuiStates.GameplayPostGame)
		{
			this.mainGuiStateMachine.HandleEvent(GuiStateEvents.EnterGameplay);
		}
	}

	/// <summary>The trigger gameplay resume.</summary>
	public void TriggerGameplayResume()
	{
		this.mainGuiStateMachine.HandleEvent(GuiStateEvents.ResumeGameplay);

		if (!this.isolatedGuiFlow)
		{
			// Trigger Gameplay resume hook here.
		}
	}

	/// <summary>The transition to gameplay countdown.</summary>
	public void TransitionToGameplayCountdown()
	{
		this.mainGuiStateMachine.HandleEvent(GuiStateEvents.ResumeGameplay);
	}

	/// <summary>
	/// The return to main menu.
	/// IMPORTANT: This event should be handled by ALL states!
	/// </summary>
	public void ReturnToMainMenu()
	{
		this.mainGuiStateMachine.HandleEvent(GuiStateEvents.ReturnToMenu);
	}

	/// <summary>The show settings popup.</summary>
	public void ShowSettings()
	{
		this.mainGuiStateMachine.HandleEvent(GuiStateEvents.ShowSettingsPopup);
	}

	/// <summary>The dismiss popup.</summary>
	public void DismissPopup()
	{
		// Special case for when the game is paused.
		if (this.mainGuiStateMachine.GetLastState() == GuiStates.GameplayPaused)
		{
			this.mainGuiStateMachine.HandleEvent(GuiStateEvents.DismissPauseSettingsPopup);
		}
		else
		{
			// All the popups should be able to handle this state
			this.mainGuiStateMachine.HandleEvent(GuiStateEvents.DismissPopup);
		}
	}

	/// <summary>The end game.</summary>
	public void EndGame()
	{
		this.mainGuiStateMachine.HandleEvent(GuiStateEvents.ShowEndGameScreen);
	}

	public void ShowCredits()
	{
		this.mainGuiStateMachine.HandleEvent(GuiStateEvents.ShowEndGameScreen);
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
		Localization.language = "English";
		this.SetupStateMachine();
	}

	private void Update()
	{
		this.gameTimePassed += Time.deltaTime;
		this.mainGuiStateMachine.Update(RealTime.deltaTime);
	}

	private void SetupStateMachine()
	{
		// Set up the main GUI state machine.
		this.mainGuiStateMachine = new StateMachine<GuiStates, GuiStateEvents>();

		// All the possible states
		this.mainGuiStateMachine.AddState(GuiStates.Uninitialized, this.Unitialized_OnEnter, null, null);
		this.mainGuiStateMachine.AddState(GuiStates.Initialized, this.Initialized_OnEnter, null, null);
		this.mainGuiStateMachine.AddState(GuiStates.MainMenu, this.MainMenu_OnEnter, null, this.MainMenu_OnExit);
		this.mainGuiStateMachine.AddState(GuiStates.Gameplay, null, null, this.Gameplay_OnExit);
		this.mainGuiStateMachine.AddState(GuiStates.GameplayPaused, this.GameplayPaused_OnEnter, null, this.GameplayPaused_OnExit);
		this.mainGuiStateMachine.AddState(GuiStates.GameplayResuming, this.InitializeGameplayHUD, null, null);
		this.mainGuiStateMachine.AddState(GuiStates.GameplayHighScore, this.GameplayHighScore_OnEnter, null, this.GameplayHighScore_OnExit);
		this.mainGuiStateMachine.AddState(GuiStates.GameplayPostGame, this.GameplayPostGame_OnEnter, null, this.GameplayPostGame_OnExit);
		this.mainGuiStateMachine.AddState(GuiStates.Settings, this.Settings_OnEnter, null, this.Settings_OnExit);
		this.mainGuiStateMachine.AddState(GuiStates.StartingGameplay, this.StartingGameplay_OnEnter, null, null);

		// Initialization state transitions
		this.mainGuiStateMachine.AddTransition(GuiStates.Uninitialized, GuiStates.Initialized, null, null, null, null, GuiStateEvents.InitializedGui);
		this.mainGuiStateMachine.AddTransition(GuiStates.Initialized, GuiStates.MainMenu, null, null, null, null, GuiStateEvents.DisplayMainMenu);

		// Main menu to other state transitions
		this.mainGuiStateMachine.AddTransition(GuiStates.MainMenu, GuiStates.Settings, null, null, null, null, GuiStateEvents.ShowSettingsPopup);
		this.mainGuiStateMachine.AddTransition(GuiStates.MainMenu, GuiStates.StartingGameplay, null, null, null, null, GuiStateEvents.EnterGameplay);

		// Return to Main Menu transitions
		this.mainGuiStateMachine.AddTransition(GuiStates.Gameplay, GuiStates.MainMenu, null, null, null, null, GuiStateEvents.ReturnToMenu);
		this.mainGuiStateMachine.AddTransition(GuiStates.GameplayPaused, GuiStates.MainMenu, null, null, null, null, GuiStateEvents.ReturnToMenu);
		this.mainGuiStateMachine.AddTransition(GuiStates.Settings, GuiStates.MainMenu, null, null, null, null, GuiStateEvents.DismissPopup);
		this.mainGuiStateMachine.AddTransition(GuiStates.GameplayPostGame, GuiStates.MainMenu, null, null, null, null, GuiStateEvents.ReturnToMenu);

		// Gameplay transitions.
		this.mainGuiStateMachine.AddTransition(GuiStates.StartingGameplay, GuiStates.Gameplay, null, null, null, null, GuiStateEvents.EnterGameplay);
		this.mainGuiStateMachine.AddTransition(GuiStates.GameplayResuming, GuiStates.Gameplay, null, null, null, null, GuiStateEvents.ResumeGameplay);
		this.mainGuiStateMachine.AddTransition(GuiStates.Gameplay, GuiStates.GameplayPaused, null, null, null, null, GuiStateEvents.PauseGameplay);
		this.mainGuiStateMachine.AddTransition(GuiStates.GameplayPaused, GuiStates.GameplayResuming, null, null, null, null, GuiStateEvents.ResumeGameplay);
		this.mainGuiStateMachine.AddTransition(GuiStates.GameplayPaused, GuiStates.Settings, null, null, null, null, GuiStateEvents.ShowSettingsPopup);
		this.mainGuiStateMachine.AddTransition(GuiStates.GameplayHighScore, GuiStates.GameplayPostGame, null, null, null, null, GuiStateEvents.ShowEndGameScreen);
		this.mainGuiStateMachine.AddTransition(GuiStates.Settings, GuiStates.GameplayPaused, null, null, null, null, GuiStateEvents.DismissPauseSettingsPopup);
		this.mainGuiStateMachine.AddTransition(GuiStates.GameplayPostGame, GuiStates.StartingGameplay, null, null, null, null, GuiStateEvents.EnterGameplay);


		// Add a state that allows us to skip to gameplay after initialization
		this.mainGuiStateMachine.AddTransition(GuiStates.Initialized, GuiStates.StartingGameplay, null, null, null, null, GuiStateEvents.SkipToGameplay);

		this.mainGuiStateMachine.Start(GuiStates.Uninitialized);
	}

	private void Unitialized_OnEnter(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		// this should be the only "Instantiate" called from the GUI side, everything else should be loaded via NGUITools.Addchild()
		this.guiGameObject = Instantiate(Resources.Load(GuiSystemPath + "GuiSystem"), new Vector3(0, 50, 0), Quaternion.identity) as GameObject;

		// Make the GUI system easier to find
		if (this.guiGameObject == null)
		{
			return;
		}

		this.guiGameObject.name = "GuiSystem";

		this.mainGuiStateMachine.HandleEvent(GuiStateEvents.InitializedGui);

		// We need to keep this object around so we don't blow out our GUI system.
		DontDestroyOnLoad(this.guiGameObject);
	}

	private void Initialized_OnEnter(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		if (!this.skipToGameplay)
		{
			this.mainGuiStateMachine.HandleEvent(GuiStateEvents.DisplayMainMenu);
		}
		else
		{
			this.mainGuiStateMachine.HandleEvent(GuiStateEvents.SkipToGameplay);
		}
	}

	private void MainMenu_OnEnter(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		// Add the main menu to the game
		if (this.currentGuiPanel == null || this.currentGuiPanel.name != "MainMenu")
		{
			Destroy(this.currentGuiPanel);
			this.currentGuiPanel = NGUITools.AddChild(this.guiGameObject, Resources.Load(GuiSystemPath + "MainMenu/MainMenu") as GameObject);
			this.currentGuiPanel.name = "MainMenu";
		}
	}

	private void MainMenu_OnExit(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
	}

	private void StartingGameplay_OnEnter(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		Destroy(this.currentGuiPanel);
		//		this.currentGuiPanel = NGUITools.AddChild(this.guiGameObject, Resources.Load(GuiSystemPath + "HUD_Panel") as GameObject);
		//
		//		this.currentGuiPanel.name = "HUD";
		this.gameTimePassed = 0.0f;

		if (!this.isolatedGuiFlow)
		{
			// Add hook to start the gameplay loop here.
		}

		// Trigger the gameplay to start immediately.
		this.TriggerGameplayStart();
		SceneManager.LoadScene("LevelDesignPrototype");
		this.mainGuiStateMachine.HandleEvent(GuiStateEvents.EnterGameplay);
	}

	private void InitializeGameplayHUD(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		Destroy(this.currentGuiPanel);
		this.currentGuiPanel = NGUITools.AddChild(this.guiGameObject, Resources.Load(GuiSystemPath + "HUD_Panel") as GameObject);
		this.currentGuiPanel.name = "HUD";
	}

	private void Gameplay_Update(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		// TODO: Listen for end game conditions... ie got caught, is stuck etc etc.
	}

	private void Gameplay_OnExit(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		if (!this.isolatedGuiFlow)
		{
			// Add hook to pause / stop the game.
		}
	}

	private void GameplayPaused_OnEnter(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		// Load the pause screen
		Destroy(this.currentGuiPanel);
		this.currentGuiPanel = NGUITools.AddChild(this.guiGameObject, Resources.Load(GuiSystemPath + "PauseMenu/PauseMenu") as GameObject);
		this.currentGuiPanel.name = "PauseScreen";
	}

	private void GameplayPaused_OnExit(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		// Do something
		// Destroy(m_CurrenGUIPanel);
	}

	private void Settings_OnEnter(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		this.currentGuiPopup = NGUITools.AddChild(this.guiGameObject, Resources.Load(GuiSystemPath + "Popups/SettingsPopup") as GameObject);
		this.currentGuiPopup.name = "SettingsPopup";
	}

	private void Settings_OnExit(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		Destroy(this.currentGuiPopup);
	}

	private void GameplayPostGame_OnEnter(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		Destroy(this.currentGuiPanel);
		this.currentGuiPanel = NGUITools.AddChild(this.guiGameObject, Resources.Load(GuiSystemPath + "PostGameScreen/ScoreScreen") as GameObject);
		this.currentGuiPanel.name = "EndGameScreen";
	}

	private void GameplayPostGame_OnExit(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		// Do something.
	}

	private void GameplayHighScore_OnEnter(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		Destroy(this.currentGuiPanel);
		this.currentGuiPanel = NGUITools.AddChild(this.guiGameObject, Resources.Load(GuiSystemPath + "PostGameScreen/HighScoreScreen") as GameObject);
		this.currentGuiPanel.name = "HighScoreScreen";
	}

	private void GameplayHighScore_OnExit(StateMachine<GuiStates, GuiStateEvents> stateMachine)
	{
		// Do something?
	}

	#endregion
}
