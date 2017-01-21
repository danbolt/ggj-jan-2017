
using UnityEngine;

public class WaveHitDetection : MonoBehaviour
{
	public GameObject Player;

	private void OnTriggerEnter()
	{
		if (Player)
		{
			Player.SendMessage("HitByWave", null, SendMessageOptions.RequireReceiver);
		}
		else
		{
			Debug.Log("wave prefab has no player set");
		}
	}
}
