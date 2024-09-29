using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu (fileName = "TotalLevelData", menuName = "Level/LevelTotal")]
public class TotalLevelData : ScriptableObject
{
    public List<LevelData> levels;
 
    public int totalLevel()
    {
        var leveltotal = levels.Count;
        return leveltotal;
    }
    public int totalWaveInlevel(int id)
    {
        var totalwave = levels[id].waveData.Count;
        return totalwave;
    }

    public int totalSpawnerInWave(int idLevel, int idWave) { 
        var totalSpawner = levels[idLevel].waveData[idWave].spawner.Count;
        return totalSpawner;
    }
}
