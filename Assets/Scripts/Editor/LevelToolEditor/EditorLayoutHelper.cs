using UnityEditor;
using UnityEngine;

public static class EditorLayoutHelper
{
    public static void DrawNav(TotalLevelData totalLevelData, ref int selectedLevel, ref int idWaveSpawner, System.Action saveData, System.Action<int> removeLevel)
    {
        EditorGUILayout.BeginVertical(EditorHelper.CreateStyle(15, FontStyle.Normal, Color.red, TextAnchor.MiddleCenter), GUILayout.Width(200), GUILayout.Height(500));
        GUILayout.Label("Level", EditorHelper.CreateStyle(15, FontStyle.Normal, Color.red, TextAnchor.MiddleCenter), GUILayout.Height(40));
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
                    removeLevel(i);
                    break;
                }

                if (GUILayout.Button("Load Wave", GUILayout.Height(40)))
                {
                   // totalLevelData.levels[selectedLevel].LoaderWavesDataResource(selectedLevel);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }

        if (GUILayout.Button("Load Level", GUILayout.Height(40)))
        {
            totalLevelData.levels.Clear();
            totalLevelData.LoaderLevelsResource();
        }

        if (GUILayout.Button("Save", GUILayout.Height(40)))
        {
            saveData();
        }

        EditorGUILayout.EndVertical();
    }

    public static void DrawWaves(TotalLevelData totalLevelData, int selectedLevel, ref int idWaveSpawner, System.Action<int> createNewWave, System.Action<int> removeWave)
    {
        EditorGUILayout.BeginVertical(EditorHelper.CreateStyle(15, FontStyle.Normal, Color.blue, TextAnchor.MiddleCenter), GUILayout.Width(300), GUILayout.Height(500));
        GUILayout.Label("Waves", EditorHelper.CreateStyle(15, FontStyle.Normal, Color.blue, TextAnchor.MiddleCenter), GUILayout.Height(40));

        if (selectedLevel >= 0 && selectedLevel < totalLevelData.levels.Count)
        {
            var waves = totalLevelData.levels[selectedLevel].waveData;
            for (int i = 0; i < waves.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                waves[i] = (WaveData)EditorGUILayout.ObjectField(waves[i], typeof(WaveData), true, GUILayout.Width(180), GUILayout.Height(30));

                if (GUILayout.Button("Load Data", GUILayout.Width(80), GUILayout.Height(30)))
                    idWaveSpawner = i;

                if (GUILayout.Button("Clear", GUILayout.Width(80), GUILayout.Height(30)))
                    removeWave(i);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }

        if (GUILayout.Button("Create Wave"))
            createNewWave(selectedLevel);

        EditorGUILayout.EndVertical();
    }

    public static void DrawContent(TotalLevelData totalLevelData, int selectedLevel, int waveIndex, ref Vector2 scrollPosition)
    {
        if (selectedLevel < 0 || selectedLevel >= totalLevelData.levels.Count || waveIndex < 0 || waveIndex >= totalLevelData.levels[selectedLevel].waveData.Count)
            return;

        EditorGUILayout.BeginVertical(EditorHelper.CreateStyle(0, FontStyle.Normal, Color.clear, TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.Label("Spawner Content", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        var slotSpawner = totalLevelData.levels[selectedLevel].waveData[waveIndex].spawner;

        if (slotSpawner != null)
        {
            for (int i = 0; i < slotSpawner.Count; i++)
            {
                GUILayout.Label($"Spawner {i} Details", EditorStyles.boldLabel);
                slotSpawner[i].girdWidth = EditorGUILayout.IntField("Width", slotSpawner[i].girdWidth);
                slotSpawner[i].girdHeight = EditorGUILayout.IntField("Height", slotSpawner[i].girdHeight);
                slotSpawner[i].indexLine = EditorGUILayout.IntField("Index Line", slotSpawner[i].indexLine);

                EditorGUILayout.Space();
                // Các thao tác chỉnh sửa lưới (grid)
                DrawGrid(slotSpawner[i]);
                EditorGUILayout.Space();

                if (GUILayout.Button("Save Grid"))
                {
                    SaveSpawnerGrid(slotSpawner[i]);
                    Debug.Log("Grid has been saved.");
                }
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private static void DrawGrid(SpawnerData spawner)
    {
        // Thêm logic vẽ lưới (grid) tại đây
    }

    private static void SaveSpawnerGrid(SpawnerData spawner)
    {
        // Thêm logic lưu trữ grid tại đây
    }
}
