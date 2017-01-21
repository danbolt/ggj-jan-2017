using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class WaveResultDef
{
    public List<WaveDef> waves;
}

[Serializable]
public class WaveDef
{
    public float time;
    public List<SpawnInstruction> content;
}

[Serializable]
public class SpawnInstruction
{
    public string spawnPoint;
    public string objectToSpawn;
}