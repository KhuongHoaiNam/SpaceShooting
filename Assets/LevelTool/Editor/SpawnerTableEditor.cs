using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnerData))]
public class SpawnerTableEditor : Editor
{
    private SpawnerData spawner;

    private void OnEnable()
    {
        spawner = (SpawnerData)target;

        // Khởi tạo lưới nếu cần thiết
        if (spawner.WidthEnemy == null || spawner.WidthEnemy.Length != spawner.gridWidth * spawner.gridHeight)
        {
            spawner.InitializeGrid();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Vẽ inspector mặc định cho các thuộc tính cơ bản
        DrawDefaultInspector();

        EditorGUILayout.Space();

        // Xử lý thay đổi kích thước grid
        HandleGridDimensionChange();

        // Vẽ lưới để chọn kẻ địch
        DrawGrid();

        // Áp dụng các thuộc tính đã thay đổi
        serializedObject.ApplyModifiedProperties();

        // Đánh dấu rằng dữ liệu đã thay đổi và lưu lại
        if (GUI.changed)
        {
            EditorUtility.SetDirty(spawner); // Đánh dấu spawner là đã thay đổi
            AssetDatabase.SaveAssets();        // Lưu lại sự thay đổi vào tệp
        }
    }

    private void HandleGridDimensionChange()
    {
        // Lưu kích thước trước đó để phát hiện thay đổi
        int previousWidth = spawner.gridWidth;
        int previousHeight = spawner.gridHeight;

        // Trường nhập cho kích thước lưới
        spawner.gridWidth = Mathf.Max(1, EditorGUILayout.IntField("Grid Width", spawner.gridWidth));
        spawner.gridHeight = Mathf.Max(1, EditorGUILayout.IntField("Grid Height", spawner.gridHeight));

        // Thay đổi kích thước lưới nếu kích thước đã thay đổi
        if (spawner.gridWidth != previousWidth || spawner.gridHeight != previousHeight)
        {
            ResizeEnemyGrid();
        }
    }

    private void ResizeEnemyGrid()
    {
        int newSize = spawner.gridWidth * spawner.gridHeight;
        EnemyIndex[] newGrid = new EnemyIndex[newSize];

        // Sao chép dữ liệu hiện có vào lưới mới
        for (int i = 0; i < Mathf.Min(newSize, spawner.WidthEnemy.Length); i++)
        {
            newGrid[i] = spawner.WidthEnemy[i];
        }

        spawner.WidthEnemy = newGrid;
    }

    private void DrawGrid()
    {
        int cellSize = 50;
        int spriteSize = 40;

        GUILayout.Space(20);

        for (int y = 0; y < spawner.gridHeight; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < spawner.gridWidth; x++)
            {
                int index = y * spawner.gridWidth + x;

                EditorGUILayout.BeginVertical(GUILayout.Width(cellSize));

                // Dropdown cho item (kẻ địch)
                spawner.WidthEnemy[index].item = (Item)EditorGUILayout.EnumPopup(spawner.WidthEnemy[index].item, GUILayout.Width(cellSize), GUILayout.Height(cellSize - 30));

                GUILayout.Space(5);

                // Trường nhập cho indexLine (số nguyên)
                spawner.WidthEnemy[index].indexLine = EditorGUILayout.IntField("", spawner.WidthEnemy[index].indexLine, GUILayout.Width(cellSize), GUILayout.Height(20));

                GUILayout.Space(5);

                // Hiển thị sprite tương ứng
                Sprite enemySprite = GetEnemySprite(spawner.WidthEnemy[index].item);
                if (enemySprite != null)
                {
                    GUILayout.Label(enemySprite.texture, GUILayout.Width(spriteSize), GUILayout.Height(spriteSize));
                }
                else
                {
                    GUILayout.Box("", GUILayout.Width(spriteSize), GUILayout.Height(spriteSize));
                }

                EditorGUILayout.EndVertical();

                GUILayout.Space(20);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private Sprite GetEnemySprite(Item enemy)
    {
        if (spawner.EnemyDataConfigTable != null)
        {
            // Tìm sprite của kẻ địch tương ứng
            EnemyData matchedData = spawner.EnemyDataConfigTable.DataTable.Find(sub => sub.enemyId == enemy);
            if (matchedData != null && matchedData.enemyIndexInfos.Count > 0)
            {
                return matchedData.enemyIndexInfos[0].iconAvt; // Sử dụng avatar icon
            }
        }
        return null;
    }
}
