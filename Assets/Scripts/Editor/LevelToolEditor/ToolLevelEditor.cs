using log4net.Core;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEditor;
using UnityEngine;

public class ToolLevelEditor : EditorWindow
{
    public TotalLevelData totalLevelData;
    private int selectedLevel = 0;
    private GUIStyle headerStyle, navStyle, waveStyle, contentStyle;
    private Vector2 scrollPosition;
    private int idWaveSpawner;
    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<ToolLevelEditor>("Level Editor");
    }

    private void OnEnable()
    {
        totalLevelData = Resources.Load<TotalLevelData>("TotalLevelData");
        InitializeStyles();
    }

    private void InitializeStyles()
    {
        headerStyle = CreateStyle(20, FontStyle.Bold, Color.magenta, TextAnchor.MiddleCenter);
        navStyle = CreateStyle(15, FontStyle.Normal, Color.red, TextAnchor.MiddleCenter);
        waveStyle = CreateStyle(15, FontStyle.Normal, Color.blue, TextAnchor.MiddleCenter);
        contentStyle = CreateStyle(0, FontStyle.Normal, Color.clear, TextAnchor.MiddleCenter);
    }

    private GUIStyle CreateStyle(int fontSize, FontStyle fontStyle, Color bgColor, TextAnchor alignment)
    {
        return new GUIStyle
        {
            fontSize = fontSize,
            fontStyle = fontStyle,
            alignment = alignment,
            normal = { background = MakeTexture(2, 2, bgColor) }
        };
    }

    private void OnGUI()
    {
        if (totalLevelData == null)
        {
            EditorGUILayout.HelpBox("TotalLevelData not found. Please ensure it is in the Resources folder.", MessageType.Error);
            return;
        }

        EditorGUILayout.BeginVertical();
        DrawHeader();
        DrawBody();

        EditorGUILayout.EndVertical();
    }

    private void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Level Editor", headerStyle, GUILayout.Height(position.height * 0.1f), GUILayout.Width(position.width));
        EditorGUILayout.EndHorizontal();
    }

    private void DrawBody()
    {
        EditorGUILayout.BeginHorizontal();
        DrawNav();
        DrawWaves();
        DrawContent(idWaveSpawner);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawNav()
    {
        EditorGUILayout.BeginVertical(navStyle, GUILayout.Width(position.width * 0.2f), GUILayout.Height(position.height * 0.9f));
        GUILayout.Label("Level", navStyle, GUILayout.Height(40));
        EditorGUILayout.Space();

        if (totalLevelData.levels != null)
        {
            for (int i = 0; i < totalLevelData.levels.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button($"Level {i}", GUILayout.Height(40)))
                    selectedLevel = i;

                if (GUILayout.Button("Remove", GUILayout.Height(40)))
                {
                    RemoveLevel(i);
                    break;
                }

                if (GUILayout.Button("Load Wave", GUILayout.Height(40)))
                {
                    //totalLevelData.levels[selectedLevel].LoaderWavesDataResource(selectedLevel);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No levels available.", MessageType.Info);
        }

        if (GUILayout.Button("Load Level", GUILayout.Height(40)))
        {
            totalLevelData.levels.Clear();
            totalLevelData.LoaderLevelsResource();
        }

        if (GUILayout.Button("Save", GUILayout.Height(40)))
        {
            SaveData();
        }

        EditorGUILayout.EndVertical();
    }

    private void RemoveLevel(int index)
    {
        if (index < 0 || index >= totalLevelData.levels.Count) return;

        var levelDataToRemove = totalLevelData.levels[index];
        totalLevelData.levels.RemoveAt(index);

        string path = AssetDatabase.GetAssetPath(levelDataToRemove);
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void SaveData()
    {
        EditorUtility.SetDirty(totalLevelData);
        AssetDatabase.SaveAssets();
        Debug.Log("TotalLevelData saved.");
    }

    private void DrawWaves()
    {
        EditorGUILayout.BeginVertical(waveStyle, GUILayout.Width(position.width * 0.3f), GUILayout.Height(position.height * 0.9f));
        GUILayout.Label("Waves", waveStyle, GUILayout.Height(40));
        EditorGUILayout.Space(10);

        if (selectedLevel >= 0 && selectedLevel < totalLevelData.levels.Count)
        {
            var waves = totalLevelData.levels[selectedLevel].waveData;
            for (int i = 0; i < waves.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                waves[i] = (WaveData)EditorGUILayout.ObjectField(waves[i], typeof(WaveData), true, GUILayout.Width(position.width * 0.18f), GUILayout.Height(30));

                if (GUILayout.Button("Load Data", GUILayout.Width(80), GUILayout.Height(30)))
                {
                    idWaveSpawner = i;

                }
                if (GUILayout.Button("Clear", GUILayout.Width(80), GUILayout.Height(30)))
                    RemoveWave(i);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(5);
            }
        }

        if (GUILayout.Button("Create Wave"))
        {
            CreateNewWave(selectedLevel);
        }

        EditorGUILayout.EndVertical();
    }

    private void RemoveWave(int waveIndex)
    {
        if (selectedLevel < 0 || selectedLevel >= totalLevelData.levels.Count || waveIndex < 0 || waveIndex >= totalLevelData.levels[selectedLevel].waveData.Count)
            return;

        var waveDataToRemove = totalLevelData.levels[selectedLevel].waveData[waveIndex];
        totalLevelData.levels[selectedLevel].waveData.RemoveAt(waveIndex);

        string path = AssetDatabase.GetAssetPath(waveDataToRemove);
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

     //   totalLevelData.levels[selectedLevel].LoaderWavesDataResource(selectedLevel);
    }

    private void CreateNewWave(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= totalLevelData.levels.Count) return;

        var waves = totalLevelData.levels[levelIndex].waveData;
        WaveData newWave = ScriptableObject.CreateInstance<WaveData>();
        newWave.name = $"Wave{waves.Count + 1}";

        string assetPath = $"Assets/Resources/LevelData/LevelTotal/Level{levelIndex}/Wave{waves.Count + 1}.asset";
        AssetDatabase.CreateAsset(newWave, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        waves.Add(newWave);
     //   totalLevelData.levels[levelIndex].LoaderWavesDataResource(levelIndex);
    }

    private void DrawContent(int waveIndex)
    {
        if (selectedLevel < 0 || selectedLevel >= totalLevelData.levels.Count || waveIndex < 0 || waveIndex >= totalLevelData.levels[selectedLevel].waveData.Count)
            return;

        // Sử dụng GUILayout để điều chỉnh kích thước và vị trí của các thành phần
        EditorGUILayout.BeginVertical(contentStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.Label("Spawner Content", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        var slotSpawner = totalLevelData.levels[selectedLevel].waveData[waveIndex].spawner;

        if (slotSpawner != null && slotSpawner.Count > 0)
        {
            for (int i = 0; i < slotSpawner.Count; i++)
            {
                // Thêm chi tiết hoặc các nút chức năng tại đây
                GUILayout.Label($"Spawner {waveIndex} Details", EditorStyles.boldLabel);
                slotSpawner[i].girdWidth = EditorGUILayout.IntField("Width", slotSpawner[i].girdWidth);
                slotSpawner[i].girdHeight = EditorGUILayout.IntField("Height", slotSpawner[i].girdHeight);
                slotSpawner[i].indexLine = EditorGUILayout.IntField("index line", slotSpawner[i].indexLine);
                InitializeGrid(slotSpawner[i]);
                for (int y = 0; y < slotSpawner[i].girdHeight; y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < slotSpawner[i].girdWidth; x++)
                    {
                        // Tạo GUIStyle để điều chỉnh màu sắc
                        GUIStyle toggleStyle = new GUIStyle(GUI.skin.button);
                        toggleStyle.normal.background = MakeTex(2, 2, grid[x, y] ? Color.green : Color.gray);

                        // Thay đổi trạng thái của ô vuông và cập nhật lại lưới
                        grid[x, y] = GUILayout.Toggle(grid[x, y], "", toggleStyle, GUILayout.Width(50), GUILayout.Height(50));
                    }
                    EditorGUILayout.EndHorizontal();
                }

                // Cập nhật lại dữ liệu của slotSpawner
                for (int y = 0; y < slotSpawner[i].girdHeight; y++)
                {
                    for (int x = 0; x < slotSpawner[i].girdWidth; x++)
                    {
                        // Lưu giá trị từ grid vào slotSpawner.tiles
                        slotSpawner[i].tiles[y * slotSpawner[i].girdWidth + x] = grid[x, y] ? TileType.Active : TileType.Inactive;
                    }
                }
                if (GUILayout.Button("Save Grid"))
                {
                    // Cập nhật mảng tiles của SpawnerData
                    slotSpawner[i].tiles = new TileType[slotSpawner[i].girdWidth * slotSpawner[i].girdHeight];
                    for (int y = 0; y < slotSpawner[i].girdHeight; y++)
                    {
                        for (int x = 0; x < slotSpawner[i].girdWidth; x++)
                        {
                            slotSpawner[i].tiles[y * slotSpawner[i].girdWidth + x] = grid[x, y] ? TileType.Active : TileType.Inactive;
                        }
                    }

                    // Đánh dấu SpawnerData là đã bị thay đổi để Unity lưu lại
                    EditorUtility.SetDirty(slotSpawner[i]);

                    // Thông báo đã lưu thành công
                    Debug.Log("Grid has been saved to the asset.");
                }
                // Đánh dấu đối tượng slotSpawner[i] đã bị thay đổi
                EditorUtility.SetDirty(slotSpawner[i]);
                GUILayout.Space(10);
            }
        }

        if (GUILayout.Button("Creat Spawner"))
        {

        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    private bool[,] grid; // Thêm mảng lưới để chỉnh sửa


    private void InitializeGrid(SpawnerData spawner)
    {
        // Kiểm tra và khởi tạo lưới nếu cần
        if (grid == null || grid.GetLength(0) != spawner.girdWidth || grid.GetLength(1) != spawner.girdHeight)
        {
            grid = new bool[spawner.girdWidth, spawner.girdHeight];

            // Khởi tạo trạng thái của lưới từ tiles của SpawnerData
            if (spawner.tiles != null)
            {
                for (int y = 0; y < spawner.girdHeight; y++)
                {
                    for (int x = 0; x < spawner.girdWidth; x++)
                    {
                        grid[x, y] = spawner.tiles[y * spawner.girdWidth + x] == TileType.Active;
                    }
                }
            }
        }
    }

    private void DrawEditableGrid(SpawnerData spawner)
    {

        /* // Nút Save sẽ lưu trạng thái của lưới vào asset
        */
    }
    // Hàm tạo texture màu cho GUIStyle
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pixels);
        result.Apply();
        return result;
    }
}