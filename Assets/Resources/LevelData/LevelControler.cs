using AAGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelControler : SingletonMono<LevelControler>
{
    public TotalLevelData levelData; // Reference to your LevelData ScriptableObject
    public Transform parentObj;
    public List<Vector3> transTarget;
    public PathCreator pathCreator;
    public EnemyBase enemyBases;

    // Đối tượng PathCreator để lấy danh sách các điểm

    // Thời gian giữa các lần sinh ra enemy
    public int wave;
    [SerializeField] private float spawnInterval;
    public List<EnemyBase> lstEnemyInWave;
    public bool checkWave;
    public EnemyDataConfigTable enemyDataConfigTable;
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
        for (int y = 0; y < levelData.levels[0].waveData[wave].spawner[0].girdHeight; y++) // Duyệt theo chiều cao trước
        {
            for (int x = 0; x < levelData.levels[0].waveData[wave].spawner[0].girdWidth; x++) // Duyệt theo chiều rộng sau
            {
                if (levelData.levels[0].waveData[wave].spawner[0].WidthEnemy[y * levelData.levels[0].waveData[wave].spawner[0].girdWidth + x] != Item.none)
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
        /*if (pathCreator == null || enemyBases == null)
        {
            Debug.LogError("PathCreator or EnemyPrefab is not assigned.");
            yield break;
        }
*/
        // Sinh ra enemy và bắt đầu di chuyển
         SetUpTarget();
        yield return new WaitForSeconds(spawnInterval);
        for (int i = 0; i < lstEnemyInWave.Count; i++)
        {
            for(int j = 0; j < transTarget.Count; j++)
            {
                lstEnemyInWave[i].endPos = transTarget[j];

            }
        }

    }
    // di chuyen theo duong di 
    private void SpawnEnemyAtPoint(Vector3 pos)
    {
        // Lấy danh sách enemy từ level và wave cụ thể
        var enemyDatas = levelData.levels[0].waveData[wave].spawner[0].WidthEnemy;
        var enemyspawner = enemyDataConfigTable.DataTable;

        EnemyBase enemy = Instantiate(enemyBases, pathCreator.Line[levelData.levels[0].waveData[wave].spawner[0].indexLine].List_Points[0], Quaternion.identity, parentObj);

        enemy.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.SetPathCreator(pathCreator, levelData.levels[0].waveData[wave].spawner[0].indexLine);
            enemy.endPos = pos;
            lstEnemyInWave.Add(enemy);

            // enemy.MovingEndPos(pos);
        }
        else
        {
            Debug.LogError("EnemyPrefab must have an EnemyMovement script.");
        }
      /*  for (int i = 0; i < enemyDatas.Length; i++)
        {
            if (enemyDatas[i] != Item.none) {
                var enemyData = enemyDataConfigTable.DataTable.FirstOrDefault(e => e.enemyId == enemyDatas[i]);

                EnemyBase enemy = Instantiate(enemyData.enemyIndexInfos[0].enemy, pathCreator.Line[levelData.levels[0].waveData[wave].spawner[0].indexLine].List_Points[0], Quaternion.identity, parentObj);

                enemy.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.SetPathCreator(pathCreator, levelData.levels[0].waveData[wave].spawner[0].indexLine);
                    enemy.endPos = pos;
                    lstEnemyInWave.Add(enemy);

                    // enemy.MovingEndPos(pos);
                }
                else
                {
                    Debug.LogError("EnemyPrefab must have an EnemyMovement script.");
                }
            }
        }*/
        /*
                for (int i =0; i < enemyData.Length; i++)
                {
                    if (enemyData[i] != Item.none)
                    {

                        EnemyBase enemy= Instantiate(enemyspawner[x].enemyIndexInfos[0].enemy, pathCreator.Line[levelData.levels[0].waveData[wave].spawner[0].indexLine].List_Points[0], Quaternion.identity, parentObj);
                            enemy.GetComponent<EnemyBase>();
                            if (enemy != null)
                            {
                                enemy.SetPathCreator(pathCreator, levelData.levels[0].waveData[wave].spawner[0].indexLine);
                                enemy.endPos = pos;
                                lstEnemyInWave.Add(enemy);

                                // enemy.MovingEndPos(pos);
                            }
                            else
                            {
                                Debug.LogError("EnemyPrefab must have an EnemyMovement script.");
                            }

                    }
                }*/


        // EnemyBase enemy = Instantiate(enemyBase, pathCreator.Line[levelData.levels[0].waveData[wave].spawner[0].indexLine].List_Points[0], Quaternion.identity, parentObj);

    }

    public void SetUpTarget()
    {
        // Kiểm tra xem số lượng vị trí spawn có khớp với số lượng lưới không
        var spawner = levelData.levels[0].waveData[wave].spawner[0];

        int index = 0;

        // Duyệt qua từng ô trong lưới (grid)
        for (int y = 0; y < spawner.girdHeight; y++)
        {
            for (int x = 0; x < spawner.girdWidth; x++)
            {
                // Lấy enemy info tại vị trí (x, y) trong lưới
                Item currentEnemy = spawner.WidthEnemy[index];

                // Kiểm tra nếu enemy info là "none", thì bỏ qua (không tạo enemy)
                if (currentEnemy != Item.none)
                {

                   CreatEnemy(currentEnemy);

                }

                index++;  // Tăng chỉ số index để duyệt qua các vị trí trong lưới
            }
        }
    }
    private void CreatEnemy(Item idEnemy)
    {
        var enemyData = enemyDataConfigTable.DataTable.FirstOrDefault(e => e.enemyId == idEnemy);
        if (enemyData != null) {

            var enemybase = enemyData.enemyIndexInfos[0].enemy;
            if (enemybase != null)
            {
                // Sinh ra enemy ở vị trí ngẫu nhiên
              EnemyBase thisEnemy=    Instantiate(enemybase, pathCreator.Line[levelData.levels[0].waveData[wave].spawner[0].indexLine].List_Points[0], Quaternion.identity, parentObj);
                thisEnemy.GetComponent<EnemyBase>();
                if (thisEnemy != null)
                {
                    thisEnemy.SetPathCreator(pathCreator, levelData.levels[0].waveData[wave].spawner[0].indexLine);
                    lstEnemyInWave.Add(thisEnemy);

                    // enemy.MovingEndPos(pos);
                }
                else
                {
                    Debug.LogError("EnemyPrefab must have an EnemyMovement script.");
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy EnemyBase hợp lệ cho enemy có ID: ");
            }
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
