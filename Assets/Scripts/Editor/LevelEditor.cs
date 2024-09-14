/*using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(SpawnerData))]
public class LevelEditor : Editor
{
    private bool[,] grid;
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        // Tạo đối tượng ReorderableList cho listEnemySpawner
        SerializedProperty listProperty = serializedObject.FindProperty("listEnemySpawner");
        reorderableList = new ReorderableList(serializedObject, listProperty, true, true, true, true);

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                                    element, GUIContent.none);
        };

        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Enemy Spawner List");
        };
    }

    public override void OnInspectorGUI()
    {
        // Tạo đối tượng SerializedObject để quản lý các thay đổi trên SpawnerData
        SerializedObject serializedObject = new SerializedObject(target);
        SpawnerData level = (SpawnerData)target;

        // Tạo các property cho SerializedObject
        SerializedProperty widthProp = serializedObject.FindProperty("width");
        SerializedProperty heightProp = serializedObject.FindProperty("height");
        SerializedProperty indexLineProp = serializedObject.FindProperty("indexLine");

        // Hiển thị các trường chỉnh sửa trong Inspector
        EditorGUILayout.PropertyField(widthProp, new GUIContent("Width"));
        EditorGUILayout.PropertyField(heightProp, new GUIContent("Height"));
        EditorGUILayout.PropertyField(indexLineProp, new GUIContent("Index Line"));

        // Hiển thị danh sách kẻ thù
        reorderableList.DoLayoutList();

        // Cập nhật nếu có thay đổi kích thước của lưới
        if (grid == null || grid.GetLength(0) != level.width || grid.GetLength(1) != level.height)
        {
            InitializeGrid(level);
        }

        // Hiển thị lưới
        for (int y = 0; y < level.height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < level.width; x++)
            {
                // Tạo GUIStyle để điều chỉnh màu sắc
                GUIStyle toggleStyle = new GUIStyle(GUI.skin.button);
                toggleStyle.normal.background = MakeTex(2, 2, grid[x, y] ? Color.green : Color.gray);

                // Ghi nhận thay đổi của lưới
                bool newValue = GUILayout.Toggle(grid[x, y], "", toggleStyle, GUILayout.Width(50), GUILayout.Height(50));

                if (newValue != grid[x, y])
                {
                    Undo.RecordObject(level, "Toggle Grid State");
                    grid[x, y] = newValue;

                    EditorUtility.SetDirty(level);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        // Nút Save để lưu lưới vào asset
        if (GUILayout.Button("Save"))
        {
            SaveGrid(level);
        }

        // Áp dụng các thay đổi cho SerializedObject
        serializedObject.ApplyModifiedProperties();

        // Buộc vẽ lại giao diện
        Repaint();
    }

    private void InitializeGrid(SpawnerData level)
    {
        grid = new bool[level.width, level.height];

        // Khởi tạo trạng thái của lưới từ tiles của SpawnerData
        if (level.tiles != null)
        {
            for (int y = 0; y < level.height; y++)
            {
                for (int x = 0; x < level.width; x++)
                {
                    grid[x, y] = level.tiles[y * level.width + x] == TileType.Active;
                }
            }
        }
        else
        {
            // Nếu tiles chưa khởi tạo, tạo một mảng mới
            level.tiles = new TileType[level.width * level.height];
        }
    }

    private void SaveGrid(SpawnerData level)
    {
        level.tiles = new TileType[level.width * level.height];
        for (int y = 0; y < level.height; y++)
        {
            for (int x = 0; x < level.width; x++)
            {
                level.tiles[y * level.width + x] = grid[x, y] ? TileType.Active : TileType.Inactive;
            }
        }

        EditorUtility.SetDirty(level);
        AssetDatabase.SaveAssets();

        Debug.Log("Grid has been saved to the asset.");
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++) pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
*/