/*using UnityEditor;
using UnityEngine;
using System.IO;

public static class ViewWaveTool 
{
    private static GUIStyle viewWaveTool;
    private static Vector2 positiones;
   // private static int curentLevel = 0;
    public static GUIStyle GetWhiteColumnStyle()
    {
        if (viewWaveTool == null)
        {
            viewWaveTool = new GUIStyle();
            viewWaveTool.normal.background = GUIViewCotroller.MakeTexture2D(2, 2, Color.white);
        }
        return viewWaveTool;
    }
    public static void OnDrawWaveNav()
    {
        var waveNav = ModelLevelTool.LoadLevelData(ModelLevelTool.curentLevel);
        EditorGUILayout.BeginVertical(viewWaveTool, GUILayout.MaxWidth(300f), GUILayout.ExpandHeight(true));
        GUILayout.Label("Wave", viewWaveTool, GUILayout.Height(40));
        positiones = GUILayout.BeginScrollView(positiones, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        SetUp();
        GUILayout.EndScrollView();
        if (GUILayout.Button("Create Wave New", GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
        {
            waveNav.CreateWave(ModelLevelTool.curentLevel);

            CreateNewFolderWave();
        }
        if (GUILayout.Button("Reset Data", GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
        {
            waveNav.LoaderAllWaveInLevel(ModelLevelTool.curentLevel);

        }
        if (GUILayout.Button("Save", GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
        {
            // Lưu tất cả thay đổi trong dữ liệu WaveData
            AssetDatabase.SaveAssets();
            Debug.Log("Changes saved.");
        }
        EditorGUILayout.EndVertical();
    }

    public static void SetUp()
    {
        var waveNav = ModelLevelTool.LoadLevelData(ModelLevelTool.curentLevel);
       
        for (int i = 0; i < waveNav.waveData.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            waveNav.waveData[i] = (WaveData)EditorGUILayout.ObjectField(waveNav.waveData[i], typeof(WaveData), true, GUILayout.MinWidth(70f), GUILayout.Height(30f));
            if (GUILayout.Button("Select", GUILayout.Height(30f), GUILayout.Width(60f)))
            {
                ModelLevelTool.waveIndex = i;
                Debug.Log($"{ModelLevelTool.curentLevel} __ {ModelLevelTool.waveIndex}");
            }
            if (GUILayout.Button("Delete", GUILayout.Height(30f), GUILayout.Width(60f)))
            {
                waveNav.RemoveWave(i);
                ModelLevelTool.waveIndex = 0;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10f);
        }
    }
    public static void InitializeStyles()
    {
       // ModelLevelTool.LoadTotalLeveData().LoaderLevelsResource();

        viewWaveTool = GUIViewCotroller.CreateStyle(20, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
    }
    private static void CreateNewFolderWave()
    {
        // Đường dẫn tới thư mục Resources/Level
        string folderPath = $"Assets/Resources/LevelData/LevelTotal/Level{ModelLevelTool.curentLevel}";

        // Tạo folder nếu nó chưa tồn tại
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("Created folder: " + folderPath);
        }

        // Tạo một folder mới với tên "New Folder" và số tăng dần nếu cần thiết
        string newFolderPath = folderPath + $"/Wave{0}";
        int folderNumber = 1;
        while (Directory.Exists(newFolderPath))
        {
            newFolderPath = folderPath + $"/Wave{folderNumber}";
            folderNumber++;
        }

        Directory.CreateDirectory(newFolderPath);
        Debug.Log("Created new folder: " + newFolderPath);

        // Refresh lại Asset Database để Unity nhận diện được folder mới
        AssetDatabase.Refresh();
    }
}
*/