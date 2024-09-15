using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "EnemyData", fileName = "EnemyDataConfig")]
public class EnemyDataConfigTable : ScriptableObject
{
    public List<EnemyData> DataTable = new List<EnemyData>();
}
[Serializable]
public class EnemyData
{
    public Item enemyId;
    public List<EnemyIndexInfo> enemyIndexInfos;

}
[Serializable]
public class EnemyIndexInfo
{
    public int level;
    public float hp;
    public float mp;
    public float damge;
    public Sprite iconAvt;
    public EnemyBase enemy;


}