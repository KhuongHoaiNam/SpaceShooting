using UnityEditor;
using UnityEngine;
using System.IO;


public static class ViewLevelTool
{
    private static GUIStyle colLevelNav;
    private static Vector2 positiones;
    public static GUIStyle GetWhiteColumnStyle()
    {
        if (colLevelNav == null)
        {
            colLevelNav = new GUIStyle();
            colLevelNav.normal.background = GUIViewCotroller.MakeTexture2D(2, 2, Color.white);
        }
        return colLevelNav;
    }

    public static void OnDrawLevelNav()
    {
        var totalLevelData = ModelLevelTool.LoadTotalLeveData();
        EditorGUILayout.BeginVertical(colLevelNav, GUILayout.MaxWidth(300f), GUILayout.ExpandHeight(true));
        GUILayout.Label("Level", colLevelNav, GUILayout.Height(40));
        positiones = GUILayout.BeginScrollView(positiones, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        SetUpView();
        GUILayout.EndScrollView();
        if (GUILayout.Button("Create Level New", GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
        {
            totalLevelData.CreateLevels();
            CreateNewFolderLevel();
        }
        if (GUILayout.Button("Reset Data", GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
        {
            totalLevelData.LoaderLevelsResource();
        }
        EditorGUILayout.EndVertical();
    }

    public static void SetUpView()
    {
        var totalLevelData = ModelLevelTool.LoadTotalLeveData();
        for (int i = 0; i < totalLevelData.levels.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            totalLevelData.levels[i] = (LevelData)EditorGUILayout.ObjectField(totalLevelData.levels[i], typeof(WaveData), true, GUILayout.MinWidth(70f), GUILayout.Height(30f));
            if (GUILayout.Button("Select", GUILayout.Height(30f), GUILayout.Width(60f)))
            {
                ModelLevelTool.curentLevel = i;
                //.LoadLevelData(ModelLevelTool.curentLevel);
                ModelLevelTool.LoadWaveData();
            }
            if (GUILayout.Button("Delete", GUILayout.Height(30f), GUILayout.Width(60f)))
            {
                totalLevelData.RemoveLevel(i);
                DeleteFolder(i);
                ModelLevelTool.curentLevel = 0;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10f);
        }

    }

    public static void InitializeStyles()
    {
        ModelLevelTool.LoadTotalLeveData().LoaderLevelsResource();

        colLevelNav = GUIViewCotroller.CreateStyle(20, FontStyle.Bold, Color.magenta, TextAnchor.MiddleCenter);
    }
    private static void CreateNewFolderLevel()
    {
        // Đường dẫn tới thư mục Resources/Level
        string folderPath = $"Assets/Resources/LevelData/LevelTotal";

        // Tạo folder nếu nó chưa tồn tại
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("Created folder: " + folderPath);
        }

        // Tạo một folder mới với tên "New Folder" và số tăng dần nếu cần thiết
        string newFolderPath = folderPath + $"/Level{0}";
        int folderNumber = 1;
        while (Directory.Exists(newFolderPath))
        {
            newFolderPath = folderPath + $"/Level{folderNumber}";
            folderNumber++;
        }

        Directory.CreateDirectory(newFolderPath);
        Debug.Log("Created new folder: " + newFolderPath);

        // Refresh lại Asset Database để Unity nhận diện được folder mới
        AssetDatabase.Refresh();
    }
    /*   private static void DeleteFolder(int id)
       {
           // Đường dẫn tới folder cần xóa, với id cụ thể
           string folderPathToDelete = $"Assets/Resources/LevelData/LevelTotal/Level{id}";

           // Kiểm tra xem folder có tồn tại không
           if (Directory.Exists(folderPathToDelete))
           {
               // Xóa folder và tất cả nội dung bên trong nó
               Directory.Delete(folderPathToDelete, true);
               Debug.Log($"Deleted folder: {folderPathToDelete}");

               // Refresh lại Asset Database để Unity nhận diện được sự thay đổi
               AssetDatabase.Refresh();
           }
           else
           {
               Debug.LogWarning($"Folder does not exist: {folderPathToDelete}");
           }
       }*/

    private static void DeleteFolder(int id)
    {
        // Đường dẫn tới folder cần xóa
        string folderPathToDelete = $"Assets/Resources/LevelData/LevelTotal/Level{id}";

        // Kiểm tra xem folder có tồn tại và có rỗng hay không
        if (Directory.Exists(folderPathToDelete))
        {
            if (Directory.GetFileSystemEntries(folderPathToDelete).Length == 0)
            {
             
            }
            else
            {
                Debug.LogWarning($"Folder is not empty and cannot be deleted: {folderPathToDelete}");
            }
        }
        else
        {
            Debug.LogWarning($"Folder does not exist: {folderPathToDelete}");
        }
        Directory.Delete(folderPathToDelete, false);
        Debug.Log($"Deleted empty folder: {folderPathToDelete}");
        // Refresh lại Asset Database để Unity nhận diện được sự thay đổi
        AssetDatabase.Refresh();
    }


}
