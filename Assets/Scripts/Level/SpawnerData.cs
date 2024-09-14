using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SpawnerData", fileName = "Level/SpawnerData")]
public class SpawnerData : ScriptableObject
{

    public int girdWidth;
    public int girdHeight;
    public Item[] WidthEnemy;
    public int indexLine;
    public EnemyDataConfigTable EnemyDataConfigTable;

    public void InitializeGrid()
    {
        WidthEnemy = new Item[girdWidth * girdHeight];
    }


    public TileType[] tiles; // Mảng để lưu trữ trạng thái của từng ô trong lưới


}
public enum TileType
{
    Inactive, // Màu xám
    Active    // Màu xanh lá
}


