using AAGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelControler : SingletonMono<LevelControler>
{
    // Tham chiếu đến TotalLevelData, đây là một ScriptableObject chứa thông tin về các level
    public TotalLevelData levelData;

    // Đối tượng cha để chứa các enemy được sinh ra
    public Transform parentObj;

    // Danh sách các vị trí target để enemy di chuyển đến
    public List<Vector3> transTarget;

    // Đối tượng PathCreator để tạo ra đường đi cho enemy
    public PathCreator pathCreator;

    // Prefab cơ bản của enemy sẽ được sử dụng để sinh ra các enemy
    public EnemyBase enemyBasePrefab;

    // Chỉ số wave hiện tại
    public int wave;

    // Khoảng thời gian giữa mỗi lần sinh enemy
    [SerializeField] private float spawnInterval;

    // Danh sách lưu trữ các enemy đã sinh ra trong mỗi wave
    public List<EnemyBase> lstEnemyInWave;

    // Biến kiểm tra xem wave đã kết thúc hay chưa
    public bool checkWave;

    // Bảng dữ liệu cấu hình enemy, chứa thông tin về các loại enemy
    public EnemyDataConfigTable enemyDataConfigTable;

    // Hàm Start được gọi khi bắt đầu game
    void Start()
    {
        GenerateLevel(); // Gọi hàm để khởi tạo level
        StartCoroutine(SpawnEnemies()); // Bắt đầu quá trình sinh enemy
    }

    // Hàm để tạo các vị trí spawn enemy cho level hiện tại
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
        var currentSpawner = levelData.levels[0].waveData[wave].spawner[0];

        // Duyệt qua lưới để tạo các vị trí spawn
        for (int y = 0; y < currentSpawner.girdHeight; y++)
        {
            for (int x = 0; x < currentSpawner.girdWidth; x++)
            {
                int index = y * currentSpawner.girdWidth + x;

                // Nếu ô hiện tại không phải là Item.none, tức là có enemy cần spawn
                if (currentSpawner.WidthEnemy[index] != Item.none)
                {
                    // Tính toán vị trí spawn của enemy
                    Vector3 position = new Vector3(x, -y, 0) * 5f + parentObj.position;

                    // Thêm vị trí này vào danh sách transTarget
                    transTarget.Add(position);
                }
            }
        }

        checkWave = true; // Đặt cờ báo hiệu level đã được tạo
    }

    // Coroutine để sinh enemy theo thời gian
    private IEnumerator SpawnEnemies()
    {
        SetUpTarget(); // Thiết lập target cho mỗi enemy
        yield return new WaitForSeconds(spawnInterval); // Chờ khoảng thời gian giữa mỗi lần sinh

        // Lặp qua danh sách enemy để đặt các vị trí kết thúc (endPos) và đường di chuyển cho chúng
        for (int i = 0; i < lstEnemyInWave.Count; i++)
        {
            lstEnemyInWave[i].endPos = transTarget[i]; // Đặt vị trí đích cho mỗi enemy
            lstEnemyInWave[i].SetPathCreator(pathCreator, levelData.levels[0].waveData[wave].spawner[0].indexLine); // Đặt đường đi cho enemy
            yield return new WaitForSeconds(spawnInterval); // Chờ trước khi sinh enemy tiếp theo
        }
    }

    // Hàm thiết lập target cho từng enemy
    public void SetUpTarget()
    {
        var spawner = levelData.levels[0].waveData[wave].spawner[0];
        int index = 0;

        // Duyệt qua lưới (grid) để tìm các vị trí có enemy cần spawn
        for (int y = 0; y < spawner.girdHeight; y++)
        {
            for (int x = 0; x < spawner.girdWidth; x++)
            {
                // Nếu ô hiện tại có enemy (không phải Item.none), tạo enemy ở vị trí đó
                if (spawner.WidthEnemy[index] != Item.none)
                {
                    CreateEnemy(spawner.WidthEnemy[index]); // Gọi hàm tạo enemy
                }
                index++;
            }
        }
    }

    // Hàm để sinh enemy dựa trên ID của enemy (Item)
    private void CreateEnemy(Item idEnemy)
    {
        // Tìm kiếm dữ liệu của enemy trong bảng dữ liệu cấu hình dựa trên ID
        var enemyData = enemyDataConfigTable.DataTable.FirstOrDefault(e => e.enemyId == idEnemy);

        // Nếu không tìm thấy dữ liệu enemy hoặc không có prefab cho enemy, in cảnh báo
        if (enemyData == null || enemyData.enemyIndexInfos[0].enemy == null)
        {
            Debug.LogWarning($"Không tìm thấy EnemyBase hợp lệ cho enemy có ID: {idEnemy}");
            return;
        }

        // Tạo một instance của enemy tại vị trí bắt đầu của đường đi (path)
        EnemyBase newEnemy = Instantiate(
            enemyData.enemyIndexInfos[0].enemy,
            pathCreator.Line[levelData.levels[0].waveData[wave].spawner[0].indexLine].List_Points[0],
            Quaternion.identity,
            parentObj
        );

        // Nếu enemy được tạo thành công, thêm nó vào danh sách lstEnemyInWave
        if (newEnemy != null)
        {
            lstEnemyInWave.Add(newEnemy);
        }
        else
        {
            Debug.LogError("EnemyPrefab must have an EnemyMovement script."); // Thông báo lỗi nếu prefab không có script điều khiển
        }
    }

    // Hàm kiểm tra xem wave hiện tại đã kết thúc chưa (tức là tất cả enemy đã bị tiêu diệt)
    public void CheckEndWave()
    {
        // Nếu danh sách enemy trống, chuyển sang wave tiếp theo
        if (!lstEnemyInWave.Any())
        {
            StartCoroutine(SwitchWave());
        }
    }

    // Coroutine để chuyển sang wave tiếp theo
    public IEnumerator SwitchWave()
    {
        wave++; // Tăng chỉ số wave
        checkWave = false; // Đặt cờ báo hiệu wave mới chưa bắt đầu
        GenerateLevel(); // Gọi hàm để tạo level mới

        // Chờ cho đến khi level mới được tạo hoàn chỉnh
        yield return new WaitUntil(() => checkWave);

        // Bắt đầu sinh enemy cho wave mới
        StartCoroutine(SpawnEnemies());
    }
}
