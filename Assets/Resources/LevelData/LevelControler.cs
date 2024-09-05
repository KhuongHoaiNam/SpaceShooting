using AAGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelControler : SingletonMono<LevelControler>
{
    public LevelData levelData; // Reference to your LevelData ScriptableObject
    public Transform parentObj;
    public List<Vector3> transTarget;
    public PathCreator pathCreator;
    public EnemyBase enemyBase;

    // Đối tượng PathCreator để lấy danh sách các điểm

    // Thời gian giữa các lần sinh ra enemy
    public int wave;
    [SerializeField] private float spawnInterval;
    public List<EnemyBase> lstEnemyInWave;
    public bool checkWave;
    void Start()
    {
        GenerateLevel();
        StartCoroutine(SpawnEnemies());
    }

    public void GenerateLevel()
    {
        transTarget.Clear();

        // Ensure the LevelData is set
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned.");
            return;
        }
        for (int y = 0; y < levelData.waveData[wave].spawner[0].height; y++) // Duyệt theo chiều cao trước
        {
            for (int x = 0; x < levelData.waveData[wave].spawner[0].width; x++) // Duyệt theo chiều rộng sau
            {
                if (levelData.waveData[wave].spawner[0].tiles[y * levelData.waveData[wave].spawner[0].width + x] == TileType.Active)
                {
                    // Instantiate a GameObject at the position of the active tile
                    Vector3 position = new Vector3(x, -y, 0) * 5f + parentObj.position;
                    //Instantiate(enemyBase, position, Quaternion.identity, parentObj);
                   
                    transTarget.Add(position);
                }
            }
        }
        checkWave = true;
    }


 
    private IEnumerator SpawnEnemies()
    {
        if (pathCreator == null || enemyBase == null)
        {
            Debug.LogError("PathCreator or EnemyPrefab is not assigned.");
            yield break;
        }

        // Sinh ra enemy và bắt đầu di chuyển
        for (int i = 0; i < transTarget.Count; i++)
        {
            SpawnEnemyAtPoint(transTarget[i]);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    // di chuyen theo duong di 
    private void SpawnEnemyAtPoint(Vector3 pos)
    {
        EnemyBase enemy = Instantiate(enemyBase, pathCreator.Line[levelData.waveData[wave].spawner[0].indexLine].List_Points[0], Quaternion.identity, parentObj);
        enemy.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.SetPathCreator(pathCreator, levelData.waveData[wave].spawner[0].indexLine);
            enemy.endPos = pos;
            lstEnemyInWave.Add(enemy);

            // enemy.MovingEndPos(pos);
        }
        else
        {
            Debug.LogError("EnemyPrefab must have an EnemyMovement script.");
        }
    }

    public void CheckEndWave()
    {
        if (!lstEnemyInWave.Any())
        {
            StartCoroutine(SwitchWave());
        }
    }
    public IEnumerator SwitchWave()
    {
        wave++;
        checkWave = false; // Đặt cờ trước khi bắt đầu
        GenerateLevel(); // Gọi hàm GenerateLevel

        // Chờ đến khi cờ isLevelGenerated trở thành true
        yield return new WaitUntil(() => checkWave);
        StartCoroutine(SpawnEnemies());
    }
}
