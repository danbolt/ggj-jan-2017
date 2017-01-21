using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class WaveResultDef
{
    public List<List<SpawnInstruction>> waves;
}

[Serializable]
public class SpawnInstruction
{
    public string spawnPoint;
    public string objectToSpawn;
}