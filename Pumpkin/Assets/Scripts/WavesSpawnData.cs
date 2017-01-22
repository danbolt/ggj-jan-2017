using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesSpawnData : MonoBehaviour {

    private WaveResultDef def;
    public int currentIndex = 0;

	// Use this for initialization
	void Start () {
        string filePath = "WavesData";

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        def = Newtonsoft.Json.JsonConvert.DeserializeObject<WaveResultDef>(targetFile.text);
    }

    public WaveDef GetNextWave()
    {
        if (def.waves.Count > currentIndex)
        {
            var result = def.waves[currentIndex];
            currentIndex++;
            return result;
        }
        else
        {
            // Return a random wave from the end
            int randomIndex = (Random.Range(def.waves.Count - 3, def.waves.Count)) - 1;
            return def.waves[randomIndex];
        }
    }
}
