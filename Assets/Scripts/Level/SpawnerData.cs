using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SpawnerData", fileName = "Level/SpawnerData")]
public class SpawnerData : ScriptableObject
{
    public int width;
    public int height;
    public int indexLine;

    public TileType[] tiles; // Mảng để lưu trữ trạng thái của từng ô trong lưới



}
public enum TileType
{
    Inactive, // Màu xám
    Active    // Màu xanh lá
}

