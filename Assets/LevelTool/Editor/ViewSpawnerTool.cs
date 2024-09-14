using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public static class ViewSpawnerTool 
{
    private static GUIStyle viewSpaner;
    private static Vector2 positiones;
    private static bool[,] grid;
    public static GUIStyle GetWhiteColumnStyle()
    {
        if (viewSpaner == null)
        {
            viewSpaner = new GUIStyle();
            viewSpaner.normal.background = GUIViewCotroller.MakeTexture2D(2, 2, Color.white);
        }
        return viewSpaner;
    }

    public static void OnDrawSpawnerNav()
    {
        var waveInLevel = ModelLevelTool.LoadWaveData();
        EditorGUILayout.BeginVertical(viewSpaner, GUILayout.MaxWidth(1000f), GUILayout.ExpandHeight(true));
        GUILayout.Label("Spawner", viewSpaner, GUILayout.Height(50f));
            positiones = GUILayout.BeginScrollView(positiones, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            SetUpView();
        GUILayout.EndScrollView();
            if (GUILayout.Button("Create Spawner New", GUILayout.Height(50f), GUILayout.ExpandWidth(true)))
            {
            waveInLevel.CreatSpawner(ModelLevelTool.curentLevel, ModelLevelTool.waveIndex);
            }
            if (GUILayout.Button("Reset Data", GUILayout.Height(50f), GUILayout.ExpandWidth(true)))
            {
                waveInLevel.LoadSpawner(ModelLevelTool.curentLevel, ModelLevelTool.waveIndex);
            }
        EditorGUILayout.EndVertical();
    }

    public static void SetUpView()
    {
        var waveInLevel = ModelLevelTool.LoadWaveData();
        for (int i = 0; i < waveInLevel.spawner.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            waveInLevel.spawner[i] = (SpawnerData)EditorGUILayout.ObjectField(waveInLevel.spawner[i], typeof(SpawnerData), true, GUILayout.MinWidth(70f), GUILayout.Height(30f));

            SetUpOnGui(i);
            
            if (GUILayout.Button("Delete", GUILayout.Height(30f), GUILayout.Width(60f)))
            {
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10f);
        }

    }

    public static void InitializeStyles()
    {

        viewSpaner = GUIViewCotroller.CreateStyle(20, FontStyle.Bold, Color.clear, TextAnchor.MiddleCenter);
    }


    public static void InitializeGrid(int SpawnerId)
    {
        if (grid == null || grid.GetLength(0) != ModelLevelTool.LoaderSpawner(SpawnerId).girdWidth || grid.GetLength(1) != ModelLevelTool.LoaderSpawner(SpawnerId).girdHeight)
        {
            grid = new bool[ModelLevelTool.LoaderSpawner(SpawnerId).girdWidth, ModelLevelTool.LoaderSpawner(SpawnerId).girdHeight];

            // Khởi tạo trạng thái của lưới từ tiles của LevelData
            if (ModelLevelTool.LoaderSpawner(SpawnerId).tiles != null)
            {
                for (int y = 0; y < ModelLevelTool.LoaderSpawner(SpawnerId).girdHeight; y++)
                {
                    for (int x = 0; x < ModelLevelTool.LoaderSpawner(SpawnerId).girdWidth; x++)
                    {
                        grid[x, y] = ModelLevelTool.LoaderSpawner(SpawnerId).tiles[y * ModelLevelTool.LoaderSpawner( SpawnerId).girdWidth + x] == TileType.Active;
                    }
                }
            }
        }
    }


    public static void DrawGrid(int SpawnerId)
    {
        for (int y = 0; y < ModelLevelTool.LoaderSpawner(SpawnerId).girdHeight; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < ModelLevelTool.LoaderSpawner(SpawnerId).girdWidth; x++)
            {
                // Tạo GUIStyle để điều chỉnh màu sắc
                GUIStyle toggleStyle = new GUIStyle(GUI.skin.button);
                toggleStyle.normal.background = GUIViewCotroller.MakeTexture2D(2, 2, grid[x, y] ? Color.green : Color.gray);

                // Thay đổi trạng thái của ô vuông và cập nhật lại lưới
                grid[x, y] = GUILayout.Toggle(grid[x, y], "", toggleStyle, GUILayout.Width(50), GUILayout.Height(50));
            }
            EditorGUILayout.EndHorizontal();
        }
    }


    public static void SaveGrid(int SpawnerId)
    {
        ModelLevelTool.LoaderSpawner(SpawnerId).tiles = new TileType[ModelLevelTool.LoaderSpawner(SpawnerId).girdWidth * ModelLevelTool.LoaderSpawner(SpawnerId).girdHeight];
        for (int y = 0; y < ModelLevelTool.LoaderSpawner(SpawnerId).girdHeight; y++)
        {
            for (int x = 0; x < ModelLevelTool.LoaderSpawner(SpawnerId).girdWidth; x++)
            {
                ModelLevelTool.LoaderSpawner(SpawnerId).tiles[y * ModelLevelTool.LoaderSpawner(SpawnerId).girdWidth + x] = grid[x, y] ? TileType.Active : TileType.Inactive;
            }
        }

        // Đánh dấu LevelData là đã bị thay đổi để Unity lưu lại
        EditorUtility.SetDirty(ModelLevelTool.LoaderSpawner(SpawnerId));

        // Thông báo đã lưu thành công
        Debug.Log("Lưới đã được lưu vào asset.");
    }

    public static void SetUpOnGui(int spawnerId)
    {
        var thisSpawner = ModelLevelTool.LoaderSpawner(spawnerId);
        
        GUILayout.FlexibleSpace();
        // Điều chỉnh kích thước của lưới
        thisSpawner.girdWidth = EditorGUILayout.IntField("Width", thisSpawner.girdWidth);
        GUILayout.Space(10f);
        thisSpawner.girdHeight = EditorGUILayout.IntField("Height", thisSpawner.girdHeight);
        GUILayout.Space(10f);
        thisSpawner.indexLine = EditorGUILayout.IntField("Index Line", thisSpawner.indexLine);

        center(spawnerId);
        GUILayout.Space(10f);

        // Nút Save sẽ lưu trạng thái của lưới vào asset
        if (GUILayout.Button("Save"))
        {
            SaveGrid(spawnerId);
        }
        GUILayout.FlexibleSpace();
    }

    public static void center(int id)
    {
        // Đặt không gian linh hoạt trước và sau nội dung để căn giữa
        GUILayout.FlexibleSpace();

        // Bắt đầu hiển thị phần nội dung
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // Nội dung sẽ được căn giữa theo chiều ngang
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();



        InitializeGrid(id);
        GUILayout.Space(10f);
        // Hiển thị lưới
        DrawGrid(id);
        GUILayout.Space(10f);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
    }
}
