using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu (fileName = "TotalLevelData", menuName = "Level/LevelTotal")]
public class TotalLevelData : ScriptableObject
{
    public List<LevelData> levels;
 
}
