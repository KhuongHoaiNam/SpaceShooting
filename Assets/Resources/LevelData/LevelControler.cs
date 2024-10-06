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
    [SerializeField] private int currentSpawner = 0 ;
    [SerializeField] private int thisLevel;

    // Bảng dữ liệu cấu hình enemy, chứa thông tin về các loại enemy
    public EnemyDataConfigTable enemyDataConfigTable;


    public bool checkStatusChange;

    public ToolTipStateGame txtToolTip;
    public int idlv => Datamanager.Instance.user.levelPlaying;

    // Hàm Start được gọi khi bắt đầu game
    void Start()
    {
        StartCoroutine(OnShowToolTip(wave+1, currentSpawner+1));
        Debug.Log(idlv);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            OnWin();
        }
    }
    public void OnWin()
    {
        ViewManager.SwitchView(ViewIndex.PopupWinView);
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
        var spawner = levelData.maps[0].levels[idlv].waveData[wave].spawner[currentSpawner];

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
        SetUpTarget();
        yield return new WaitForSeconds(spawnInterval);

        for (int i = 0; i < lstEnemySpawner.Count; i++)
        {
            var Setupline = levelData.maps[0].levels[idlv].waveData[wave].spawner[currentSpawner].WidthEnemy[i].indexLine;
            lstEnemySpawner[i].endPos = transTarget[i];
            lstEnemySpawner[i].SetPathCreator(pathCreator,Setupline ); // Đặt đường đi cho enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Hàm thiết lập target cho từng enemy
    public void SetUpTarget()
    {

        lstEnemySpawner.Clear();
        var spawner = levelData.maps[0].levels[idlv].waveData[wave].spawner[currentSpawner];
        int index = 0;
        Debug.Log($"{wave}++++++{currentSpawner}");
        for (int y = 0; y < spawner.gridHeight; y++)
        {
            for (int x = 0; x < spawner.gridWidth; x++)
            {
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
        var dataLevel = levelData.maps[0].levels[idlv].waveData[wave].spawner[0].WidthEnemy[line].indexLine;
        var enemyData = enemyDataConfigTable.DataTable.FirstOrDefault(e => e.enemyId == idEnemy);
        var pointIndex = pathCreator.Line[dataLevel].List_Points;
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

    public void SwitchWave()
    {
        var totalWave = levelData.maps[0].levels[idlv].waveData.Count-1;

        var totalSpawner = levelData.maps[0].levels[idlv].waveData[wave].spawner.Count - 1;

        if (!lstEnemySpawner.Any())
        {
            if (wave < totalWave)
            {
                if (currentSpawner < totalSpawner)
                {
                    currentSpawner++;
                    StartCoroutine(OnShowToolTip(totalWave, totalSpawner));
                }
                else if (currentSpawner >= totalSpawner)
                {
                    wave++;
                    currentSpawner = 0;
                    checkStatusChange = false;
                    StartCoroutine(OnShowToolTip(totalWave, totalSpawner));
                }
            }
            else {
                ViewManager.SwitchView(ViewIndex.PopupWinView);
            }
        }
    }
    public IEnumerator OnShowToolTip(int totalWave, int totalSpawner)
    {
        txtToolTip.gameObject.SetActive(true);
        txtToolTip.SetUpToolTip($"{idlv}==Wave {wave.ToString()}", totalWave);
        yield return new WaitForSeconds(3f);
        txtToolTip.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        txtToolTip.gameObject.SetActive(true);
        txtToolTip.SetUpToolTip($" Spawner {currentSpawner.ToString()}", totalSpawner);
        yield return new WaitForSeconds(2f);
        txtToolTip.gameObject.SetActive(false);
        GenerateLevel(); // Gọi hàm để khởi tạo level
        StartCoroutine(SpawnEnemies()); // Bắt đầu quá trình sinh enemy
    }
}
