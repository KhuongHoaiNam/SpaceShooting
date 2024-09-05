using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "testOs", fileName = "testEditor")]
public class SOobject : ScriptableObject
{
    public List<SpawnerData> gameObjects = new List<SpawnerData>(); // Khởi tạo danh sách
}
