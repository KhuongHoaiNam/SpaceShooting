using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public static class ModelLevelTool 
{
/*    public static int curentLevel = 0;
    public static int waveIndex = 0;

    public static TotalLevelData LoadTotalLeveData()
    {
        TotalLevelData totalLevelData = Resources.Load<TotalLevelData>("TotalLevelData");
        return totalLevelData;
    }

    public static LevelData LoadLevelData(int isLevl)
    {
        TotalLevelData totalLevelData = LoadTotalLeveData();
        if (totalLevelData == null || curentLevel < 0 || isLevl >= totalLevelData.levels.Count)
        {
            Debug.LogError("TotalLevelData not found or invalid level index.");
            return null;
        }
        return totalLevelData.levels[isLevl];
    }

    public static WaveData LoadWaveData()
    {
        LevelData levelDAta = LoadLevelData(curentLevel);
        return levelDAta.waveData[waveIndex];
    }
    

    public static SpawnerData LoaderSpawner( int Sawnerindex)
    {
        WaveData wavedata = LoadWaveData();
        return wavedata.spawner[Sawnerindex];
    }*/
}
