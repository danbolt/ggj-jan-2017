using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	private GuiManager uiManagerInstance;

	// Use this for initialization
	void Start () {
		uiManagerInstance = GuiManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnStartGameClicked()
	{
		if (uiManagerInstance)
		{
			uiManagerInstance.TriggerGameplayStart();
		}
	}

	public void OnShowCreditsClicked()
	{
		if (uiManagerInstance)
		{
			uiManagerInstance.ShowSettings();
		}
	}

	public void OnShowSettingsClicked()
	{
		if (uiManagerInstance)
		{
			uiManagerInstance.ShowSettings();
		}
	}
}
