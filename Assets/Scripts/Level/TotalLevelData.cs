using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu (fileName = "TotalLevelData", menuName = "Level/LevelTotal")]
public class TotalLevelData : ScriptableObject
{
    public List<DataMap> maps;
 
}
[Serializable]
public class DataMap
{
    public List <LevelData> levels;
}