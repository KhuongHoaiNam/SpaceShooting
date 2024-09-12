using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSpawnerConfig : MonoBehaviour
{
    public List<DataEnemyConfig> dataEnemyConfigs;

}

[Serializable]
public class DataEnemyConfig 
{
    public EnemyInfo enemyInfor;
    public GameObject prefebEnemy;
}

