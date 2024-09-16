using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpawnerData", fileName = "Level/SpawnerData")]
public class SpawnerData : ScriptableObject
{
    public int gridWidth;
    public int gridHeight;
    public EnemyIndex[] WidthEnemy;
    public EnemyDataConfigTable EnemyDataConfigTable;

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


