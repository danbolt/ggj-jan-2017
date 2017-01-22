using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	[SerializeField]
	private UILabel scoreLabel;

	[SerializeField]
	private UILabel highScoreLabel;

	[SerializeField]
	private UILabel timeLabel;

	private GameManager gameManager;
	private GuiManager guiManager;

	// Use this for initialization
	void Start () {
		this.gameManager = GameManager.Instance;
		this.guiManager = GuiManager.Instance;
		UpdateScoreLabel();
		UpdateHighScoreLabel();
		UpdateTimeLabel();
	}

	void UpdateScoreLabel()
	{
		if (this.gameManager != null) 
		{
			scoreLabel.text = (Mathf.Floor(this.gameManager.CurrentElapsedTime) + this.gameManager.CurrentScore).ToString();
		}	
	}

	void UpdateHighScoreLabel()
	{
		if (this.gameManager != null)
		{
			highScoreLabel.text = this.gameManager.HighScore.ToString();
		}
	}

	void UpdateTimeLabel()
	{
		if (this.gameManager != null)
		{
			this.timeLabel.text = Mathf.Floor(this.gameManager.CurrentElapsedTime).ToString();
		}
	}


	void Update()
	{
		UpdateScoreLabel();
		UpdateTimeLabel();
	}

	public void PauseGame() {
		if (this.guiManager != null)
		{
			this.guiManager.TriggerGameplayPause();
		}
//		this.gameManager.Pause();
	}
}
