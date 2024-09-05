using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "TotalLevelData", menuName = "Level/LevelTotal")]
public class TotalLevelData : ScriptableObject
{
    public List<LevelData> levels;
    public void LoaderLevelsResource()
    {
        LevelData[] loadedLevels = Resources.LoadAll<LevelData>("LevelData/LevelDataTable");

        // Chuyển đổi mảng thành List và gán vào biến levels
        levels = new List<LevelData>(loadedLevels);

        // Kiểm tra danh sách đã load đúng các level chưa
        foreach (var level in levels)
        {
            Debug.Log("Loaded level: " + level.name);
        }
    }
}
