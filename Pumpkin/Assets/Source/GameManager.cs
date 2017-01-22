﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private int highScore = 0;
	private int playerScore = 0;
	private bool isGameRunning;

	private static GameManager instance;

	public static GameManager Instance {
		get 
		{
			return instance;
		}
	}

	public int CurrentScore 
	{
		get 
		{
			return this.playerScore;
		}
	}

	public bool BeatHighScore
	{
		get
		{
			return this.playerScore > this.highScore;
		}
	}

	private void Awake() {
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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncreaseScore()
	{
		this.playerScore++;
	}

	public void ResetScore()
	{
		this.playerScore = 0;
	}

	public void Pause()
	{
		Time.timeScale = 0.0f;
	}

	public void UnPause()
	{
		Time.timeScale = 1.0f;
	}

	public void UpdateHighScore()
	{
		this.highScore = this.playerScore;
	}

}
