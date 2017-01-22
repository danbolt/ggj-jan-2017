using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	[SerializeField]
	private UILabel scoreLabel;

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		this.gameManager = GameManager.Instance;
		UpdateScoreLabel();
	}

	void UpdateScoreLabel()
	{
		if (this.gameManager != null) 
		{
			scoreLabel.text = this.gameManager.CurrentScore.ToString();
		}	
	}

	void Update()
	{
		UpdateScoreLabel();
	}

	public void PauseGame() {
		this.gameManager.Pause();
	}
}
