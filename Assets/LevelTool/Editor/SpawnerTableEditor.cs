using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnerData))]
public class SpawnerTableEditor : Editor
{
    SpawnerData spawner;

    private void OnEnable()
    {
        spawner = (SpawnerData)target;

        // Load the SpriteManager asset (make sure you assign it in the Inspector)
        spawner.EnemyDataConfigTable = AssetDatabase.LoadAssetAtPath<EnemyDataConfigTable>("Assets/SpriteManager.asset");

        if (spawner.WidthEnemy == null || spawner.WidthEnemy.Length != spawner.girdWidth * spawner.girdHeight)
        {
            spawner.InitializeGrid();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Let Unity handle the default inspector properties (grid width/height)
        DrawDefaultInspector();

        EditorGUILayout.Space();

        // Handle changes to grid width and height
        HandleGridDimensionChange();

        // Create a grid layout for selecting enemies
        DrawGrid();

        serializedObject.ApplyModifiedProperties();
    }

    // Function to handle grid dimension changes
    private void HandleGridDimensionChange()
    {
        // Track if the grid dimensions changed
        int previousWidth = spawner.girdWidth;
        int previousHeight = spawner.girdHeight;

        // Draw editable fields for grid width and height
        spawner.girdWidth = EditorGUILayout.IntField("Grid Width", spawner.girdWidth);
        spawner.girdHeight = EditorGUILayout.IntField("Grid Height", spawner.girdHeight);

        // Ensure the grid dimensions stay positive
        spawner.girdWidth = Mathf.Max(1, spawner.girdWidth);
        spawner.girdHeight = Mathf.Max(1, spawner.girdHeight);

        // If the dimensions have changed, reinitialize the WidthEnemy array
        if (spawner.girdWidth != previousWidth || spawner.girdHeight != previousHeight)
        {
            ResizeEnemyGrid();
        }
    }

    // Function to resize the enemy grid
    private void ResizeEnemyGrid()
    {
        int newSize = spawner.girdWidth * spawner.girdHeight;
        Item[] newGrid = new Item[newSize];

        // Copy over existing data to the new grid (up to the new size)
        for (int i = 0; i < Mathf.Min(newSize, spawner.WidthEnemy.Length); i++)
        {
            newGrid[i] = spawner.WidthEnemy[i];
        }

        spawner.WidthEnemy = newGrid;
    }

    private void DrawGrid()
    {
        // Kích thước của mỗi ô trong lưới
        int cellSize = 50;
        int spriteSize = 40; // Kích thước của hình ảnh bên dưới mỗi enum
        GUILayout.Space(20);
        // Bắt đầu vẽ lưới
        for (int y = 0; y < spawner.girdHeight; y++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < spawner.girdWidth; x++)
            {
                int index = y * spawner.girdWidth + x;

                // Bắt đầu khối dọc cho mỗi ô trong lưới
                EditorGUILayout.BeginVertical(GUILayout.Width(cellSize));

                // Tạo một menu thả xuống (dropdown) cho mỗi ô trong lưới
                spawner.WidthEnemy[index] = (Item)EditorGUILayout.EnumPopup(spawner.WidthEnemy[index], GUILayout.Width(cellSize), GUILayout.Height(cellSize - 30));

                // Tạo khoảng cách 10 đơn vị giữa EnumPopup và sprite
                GUILayout.Space(5);

                // Lấy hình ảnh tương ứng với loại enemy hiện tại
                Sprite enemySprite = GetEnemySprite(spawner.WidthEnemy[index]);

                // Vẽ hình ảnh bên dưới menu thả xuống
                if (enemySprite != null)
                {
                    GUILayout.Label(enemySprite.texture, GUILayout.Width(spriteSize), GUILayout.Height(spriteSize));
                }
                else
                {
                    // Nếu không có hình ảnh, vẽ một ô trống hoặc hình mặc định
                    GUILayout.Box("", GUILayout.Width(spriteSize), GUILayout.Height(spriteSize));
                }

                EditorGUILayout.EndVertical(); // Kết thúc khối dọc

                // Tạo khoảng cách 20 đơn vị giữa các khối với nhau
                GUILayout.Space(20);
            }
            EditorGUILayout.EndHorizontal(); // Kết thúc hàng hiện tại
        }
    }

    // Function to return the corresponding sprite from the SpriteManager
    private Sprite GetEnemySprite(Item enemy)
    {
        if (spawner.EnemyDataConfigTable != null)
        {
            // Sửa lỗi thừa dấu chấm
            EnemyData matchedSubSprite = spawner.EnemyDataConfigTable.DataTable.Find(sub => sub.enemyId == enemy);
            if (matchedSubSprite != null && matchedSubSprite.enemyIndexInfos.Count > 0)
            {
                return matchedSubSprite.enemyIndexInfos[0].iconAvt; // Sử dụng iconAvt
            }
        }
        return null;
    }
}