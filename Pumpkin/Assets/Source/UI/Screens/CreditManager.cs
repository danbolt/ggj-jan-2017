using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour {

	public void GoBackToMainMenu()
	{
		if (GuiManager.Instance)
		{
			GuiManager.Instance.ReturnToMainMenu();
		}	
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
