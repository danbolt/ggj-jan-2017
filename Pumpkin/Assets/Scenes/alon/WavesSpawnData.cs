using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesSpawnData : MonoBehaviour {

    private WaveResultDef def;
    private int currentIndex;

	// Use this for initialization
	void Start () {
        string filePath = "WavesData";

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        def = Newtonsoft.Json.JsonConvert.DeserializeObject<WaveResultDef>(targetFile.text);

        currentIndex = 0;
    }

    public List<SpawnInstruction> GetNextWave()
    {
        if (def.waves.Count > currentIndex)
        {
            var result = def.waves[currentIndex];
            currentIndex++;
            return result;
        }

        return null;
    }
}
