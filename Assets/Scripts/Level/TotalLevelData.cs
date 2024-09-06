using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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


        for(int i =0; i < loadedLevels.Length; i++)
        {
            string newname = $"Level{i}";
            string assetPath = AssetDatabase.GetAssetPath(levels[i]);
            AssetDatabase.RenameAsset(assetPath, newname);

            // Lưu lại và làm mới dự án
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
        // Kiểm tra danh sách đã load đúng các level chưa
        foreach (var level in levels)
        {
            Debug.Log("Loaded level: " + level.name);
        }
    }



    public void RemoveLevel(int idLevel)
    {
        var levelDataToRemove = levels[idLevel];
        levels.RemoveAt(idLevel);
        string path = AssetDatabase.GetAssetPath(levelDataToRemove);
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
   
    public void CreateLevels()
    {
        LevelData newLevel = ScriptableObject.CreateInstance<LevelData>();
        newLevel.name = $"Level{levels.Count + 1}";
        string assetPath = $"Assets/Resources/LevelData/LevelDataTable/{newLevel.name}.asset";
        AssetDatabase.CreateAsset(newLevel, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
     //   LoaderLevelsResource();
        levels.Add(newLevel);

    }
}
