using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesSpawner : MonoBehaviour {

    private const int TIMER_DEFAULT = 2;

    private float timeLeft = 0;
    private Dictionary<string, GameObject> spawnPoints;
    private WavesSpawnData spawnData;
    private List<GameObject> currentSpawns;
    private WaveDef currentWaveDef;

    // Use this for initialization
    void Start () {

        currentSpawns = new List<GameObject>();

        // Get all spawn points and keep a reference to them
        var spawnPointsSearch = GameObject.FindGameObjectsWithTag("SpawnPoints");
        spawnPoints = new Dictionary<string, GameObject>();
        foreach (GameObject spawnPoint in spawnPointsSearch)
        {
            spawnPoints[spawnPoint.name] = spawnPoint;
        }

        spawnData = this.GetComponent<WavesSpawnData>();
        SetCurrentWaveDef();
    }

    private void SetCurrentWaveDef()
    {
        currentWaveDef = spawnData.GetNextWave();
        if (currentWaveDef != null)
        {
            timeLeft = currentWaveDef.time;
        }
    }
	
	// Update is called once per frame
	void Update () {

        timeLeft -= Time.deltaTime;
        //Debug.Log(timeLeft);
        if (timeLeft < 0)
        {
            Spawn(currentWaveDef);
            //Debug.Log("I have finished counting down");
            SetCurrentWaveDef();
        }
    }

    private void Spawn(WaveDef waveDef) {
        // Get assigned spawn points
        // And create objects
        // Clear the previous wave
        if (waveDef != null)
        {
            foreach (GameObject spawn in currentSpawns)
            {
                if (spawn != null)
                {
                    Destroy(spawn);
                }
            }
            foreach (SpawnInstruction spawnInstruction in waveDef.content)
            {
                Object foundObject = Resources.Load(spawnInstruction.objectToSpawn);

                GameObject targetObject = Instantiate(foundObject as GameObject, spawnPoints[spawnInstruction.spawnPoint].transform.position, new Quaternion());

                currentSpawns.Add(targetObject);
            }

        }
     
    }

}
