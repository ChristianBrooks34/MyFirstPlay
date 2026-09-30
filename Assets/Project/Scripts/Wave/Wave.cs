using System;
using System.Collections.Generic;

[Serializable]
public class Wave
{
    public int WaveSpawnDelayInMilliseconds;
    public List<WaveEnemy> Enemies = new List<WaveEnemy>();

    public bool IsWaveActive { get; set; }
    public int CurrentAliveEnemies { get; set; }
}

