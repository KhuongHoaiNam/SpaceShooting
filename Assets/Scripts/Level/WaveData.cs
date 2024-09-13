using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu (fileName ="Wave Data", menuName = "Level/Wave Data")]
public class WaveData : ScriptableObject
{
    public List<SpawnerData> spawner = new List<SpawnerData>();
    public void CreatSpawner(int level, int wave)
    {

        SpawnerData newSpawnerData = ScriptableObject.CreateInstance<SpawnerData>();
        newSpawnerData.name = $"Spawner_{spawner.Count + 1}_wave{wave}_Level{level}";
        string assetPath = $"Assets/Resources/LevelData/LevelTotal/Level{level}/Wave{wave}/{newSpawnerData.name}.asset";

        AssetDatabase.CreateAsset(newSpawnerData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //   LoaderLevelsResource();
        spawner.Add(newSpawnerData);
    }

    public void RemoveSpawner(int level, int wave)
    {
        var levelDataToRemove = spawner[level];
        spawner.RemoveAt(level);

        string path = AssetDatabase.GetAssetPath(levelDataToRemove);
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
  
    public void LoadSpawner(int level , int wave)
    {
        SpawnerData[] loaderSpawner = Resources.LoadAll<SpawnerData>($"LevelData/LevelTotal/Level{level}/Wave{wave}");
        spawner = new List<SpawnerData>(loaderSpawner);
        for(int i =0; i < loaderSpawner.Length; i++)
        {
            string name = $"Spawner_{i}_wave{wave}_Level{level}";
            spawner[i].name = name;
            string asssetPath = AssetDatabase.GetAssetPath(spawner[i]);
            AssetDatabase.RenameAsset(asssetPath, name);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            }
        Debug.Log($"{level}--{wave}");
    }

    
}
