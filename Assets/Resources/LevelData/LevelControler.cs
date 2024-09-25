using AAGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelControler : SingletonMono<LevelControler>
{
    public TotalLevelData levelData;
    public Transform parentObj;
    public List<Vector3> transTarget;
    public PathCreator pathCreator;
    public List<EnemyBase> lstEnemySpawner;
    
    [SerializeField] private float spawnInterval;
    [SerializeField] private int wave = 0;
    [SerializeField] private int currentSpawner = 0;
    [SerializeField] private int thisLevel;

    // Bảng dữ liệu cấu hình enemy, chứa thông tin về các loại enemy
    public EnemyDataConfigTable enemyDataConfigTable;


    public bool checkStatusChange;

    public ToolTipStateGame txtToolTip;
    public int idlv;

    // Hàm Start được gọi khi bắt đầu game
    void Start()
    {
        StartCoroutine(OnShowToolTip()); 
    }

    public void GenerateLevel()
    {
        transTarget.Clear(); // Xóa các target cũ trước khi tạo mới

        // Kiểm tra xem dữ liệu level đã được thiết lập chưa
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned."); // Thông báo lỗi nếu chưa có dữ liệu
            return;
        }

        // Lấy thông tin spawner của wave hiện tại
        var spawner = levelData.levels[idlv].waveData[wave].spawner[currentSpawner];

        // Duyệt qua lưới để tạo các vị trí spawn
        for (int y = 0; y < spawner.gridHeight; y++)
        {
            for (int x = 0; x < spawner.gridWidth; x++)
            {
                int index = y * spawner.gridWidth + x;

                // Nếu ô hiện tại không phải là Item.none, tức là có enemy cần spawn
                if (spawner.WidthEnemy[index].item != Item.none)
                {
                    // Tính toán vị trí spawn của enemy
                    Vector3 position = new Vector3(x, -y, 0) * 5f + parentObj.position;

                    // Thêm vị trí này vào danh sách transTarget
                    transTarget.Add(position);
                }
            }
        }

        checkStatusChange = true; // Đặt cờ báo hiệu level đã được tạo
    }

    // Coroutine để sinh enemy theo thời gian
    private IEnumerator SpawnEnemies()
    {
        SetUpTarget(); // Thiết lập target cho mỗi enemy
        yield return new WaitForSeconds(spawnInterval); // Chờ khoảng thời gian giữa mỗi lần sinh

        // Lặp qua danh sách enemy để đặt các vị trí kết thúc (endPos) và đường di chuyển cho chúng
        for (int i = 0; i < lstEnemySpawner.Count; i++)
        {
            lstEnemySpawner[i].endPos = transTarget[i]; // Đặt vị trí đích cho mỗi enemy
            lstEnemySpawner[i].SetPathCreator(pathCreator, levelData.levels[idlv].waveData[wave].spawner[currentSpawner].WidthEnemy[i].indexLine); // Đặt đường đi cho enemy
            yield return new WaitForSeconds(spawnInterval); // Chờ trước khi sinh enemy tiếp theo
        }
    }

    // Hàm thiết lập target cho từng enemy
    public void SetUpTarget()
    {
        lstEnemySpawner.Clear();
        var spawner = levelData.levels[idlv].waveData[wave].spawner[currentSpawner];
        int index = 0;
        Debug.Log($"{wave}++++++{currentSpawner}");
        // Duyệt qua lưới (grid) để tìm các vị trí có enemy cần spawn
        for (int y = 0; y < spawner.gridHeight; y++)
        {
            for (int x = 0; x < spawner.gridWidth; x++)
            {
                // Nếu ô hiện tại có enemy (không phải Item.none), tạo enemy ở vị trí đó
                if (spawner.WidthEnemy[index].item != Item.none)
                {
                    CreateEnemy(spawner.WidthEnemy[index].item, index); // Gọi hàm tạo enemy
                }
                index++;
            }
        }
    }

    private void CreateEnemy(Item idEnemy, int line)
    {
      
        var enemyData = enemyDataConfigTable.DataTable.FirstOrDefault(e => e.enemyId == idEnemy);
        var pointIndex = pathCreator.Line[levelData.levels[idlv].waveData[wave].spawner[0].WidthEnemy[line].indexLine].List_Points;
        if (enemyData == null || enemyData.enemyIndexInfos[0].enemy == null)
        {
            Debug.LogWarning($"Không tìm thấy EnemyBase hợp lệ cho enemy có ID: {idEnemy}");
            return;
        }
        EnemyBase newEnemy = Instantiate(
            enemyData.enemyIndexInfos[0].enemy, pointIndex[0]
           ,
            Quaternion.identity,
            parentObj
        );
        if (newEnemy != null)
        {
            lstEnemySpawner.Add(newEnemy);
        }
        else
        {
            Debug.LogError("EnemyPrefab must have an EnemyMovement script.");
        }
    }


    public void CheckSpawner()
    {
        var indexSpawner = levelData.levels[idlv].waveData[wave].spawner.Count;
        if (!lstEnemySpawner.Any())
        {
            currentSpawner++;
            if (currentSpawner < indexSpawner)
            {
                StartCoroutine(OnShowToolTip());
                Debug.Log("wave");
            }
            else if (currentSpawner >= indexSpawner)
            {
                StartCoroutine(SwitchWave());
                StartCoroutine(OnShowToolTip());
                Debug.Log("Chuyen Wave");
            }
            Debug.Log(indexSpawner);
        }
    }
    public IEnumerator SwitchWave()
    {
        wave++; // Tăng chỉ số wave
        currentSpawner = 0;

        checkStatusChange = false; // Đặt cờ báo hiệu wave mới chưa bắt đầu
        GenerateLevel(); // Gọi hàm để tạo level mới

        // Chờ cho đến khi level mới được tạo hoàn chỉnh
        yield return new WaitUntil(() => checkStatusChange);

        // Bắt đầu sinh enemy cho wave mới
        StartCoroutine(SpawnEnemies());
    }


    public IEnumerator OnShowToolTip()
    {
        var totalWave = levelData.levels[idlv].waveData.Count;
        var totalSpawner = levelData.levels[idlv].waveData[wave].spawner.Count;
        txtToolTip.gameObject.SetActive(true);
        txtToolTip.SetUpToolTip( $"{idlv}==Wave {wave.ToString()}", totalWave);
        yield return new WaitForSeconds(3f);
        txtToolTip.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        txtToolTip.gameObject.SetActive(true);
        txtToolTip.SetUpToolTip($" Spawner { currentSpawner.ToString()}", totalSpawner);
        yield return new WaitForSeconds(2f);
        txtToolTip.gameObject.SetActive(false);
        GenerateLevel(); // Gọi hàm để khởi tạo level
        StartCoroutine(SpawnEnemies()); // Bắt đầu quá trình sinh enemy
    }

    /*public void CheckEndWave()
    {
        // Nếu danh sách enemy trống, chuyển sang wave tiếp theo
        if (!lstEnemyInWave.Any())
        {
            StartCoroutine(SwitchWave());
        }
    }

 */
}
