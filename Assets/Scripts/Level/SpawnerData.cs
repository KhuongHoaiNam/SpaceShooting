using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpawnerData", fileName = "Level/SpawnerData")]
public class SpawnerData : ScriptableObject
{
    [HideInInspector]public int gridWidth;
    [HideInInspector] public int gridHeight;
   [HideInInspector] public EnemyIndex[] WidthEnemy;
    [HideInInspector] public EnemyDataConfigTable EnemyDataConfigTable;

    // Initializes the grid with the correct size
    public void InitializeGrid()
    {
        WidthEnemy = new EnemyIndex[gridWidth * gridHeight];
    }
}

[Serializable]
public class EnemyIndex
{
    public Item item;
    public int indexLine;
}


