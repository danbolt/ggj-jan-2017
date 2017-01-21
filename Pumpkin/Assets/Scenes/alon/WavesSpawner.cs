using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesSpawner : MonoBehaviour {

    private const int TIMER_DEFAULT = 2;

    private float timeLeft = 0;
    private Dictionary<string, GameObject> spawnPoints;
    private WavesSpawnData spawnData;

	// Use this for initialization
	void Start () {
        timeLeft = TIMER_DEFAULT;

        // Get all spawn points and keep a reference to them
        var spawnPointsSearch = GameObject.FindGameObjectsWithTag("SpawnPoints");
        spawnPoints = new Dictionary<string, GameObject>();
        foreach (GameObject spawnPoint in spawnPointsSearch)
        {
            spawnPoints[spawnPoint.name] = spawnPoint;
        }

        spawnData = this.GetComponent<WavesSpawnData>();
    }
	
	// Update is called once per frame
	void Update () {

        timeLeft -= Time.deltaTime;
        Debug.Log(timeLeft);
        if (timeLeft < 0)
        {
            Spawn();
            Debug.Log("I have finished counting down");
            timeLeft = TIMER_DEFAULT;
        }
    }

    private void Spawn() {
        // Get assigned spawn points
        // And create objects

        List<SpawnInstruction> spawnInstructions = spawnData.GetNextWave();
        if (spawnInstructions != null)
        {
            foreach (SpawnInstruction spawnInstruction in spawnInstructions)
            {
                Object foundObject = Resources.Load(spawnInstruction.objectToSpawn);

                GameObject targetObject = Instantiate(foundObject as GameObject, spawnPoints[spawnInstruction.spawnPoint].transform.position, new Quaternion());
            }

        }
     
    }

}
