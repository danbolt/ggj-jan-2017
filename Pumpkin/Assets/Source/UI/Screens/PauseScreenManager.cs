using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenManager : MonoBehaviour {

	private GuiManager guiManager;

	public void ResumeGameplay()
	{
		if (guiManager != null) 
		{
			this.guiManager.TriggerGameplayResume();
		}
	}

	private void Start()
	{
		this.guiManager = GuiManager.Instance;
	}

}
