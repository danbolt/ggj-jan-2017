﻿
using UnityEngine;

public class Pickup : MonoBehaviour
{
	public string ActorName = "Player";

	private void OnTriggerEnter(Collider otherCollider)
	{
		if (otherCollider.gameObject.name == ActorName)
		{
			if (GameManager.Instance)
			{
				GameManager.Instance.IncreaseScore();
			}

			gameObject.SetActive(false);
		}
	}
}
