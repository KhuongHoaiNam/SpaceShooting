using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName ="Wave Data", menuName = "Level/Wave Data")]
public class WaveData : ScriptableObject
{
    public List<SpawnerData> spawner;
}
