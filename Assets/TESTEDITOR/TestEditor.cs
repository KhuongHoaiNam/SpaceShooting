/*using UnityEditor;
using UnityEngine;

public class TestEditor : EditorWindow
{
  *//*  private SOobject soObject;

    [MenuItem("Window/SOobject Editor")]
    public static void ShowWindow()
    {
        GetWindow<TestEditor>("SOobject Editor");
    }

    private void OnEnable()
    {
        // Load a SOobject asset or create a new one
        string[] guids = AssetDatabase.FindAssets("t:SOobject");
        if (guids.Length > 0)
        {
            soObject = AssetDatabase.LoadAssetAtPath<SOobject>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }
        else
        {
            soObject = ScriptableObject.CreateInstance<SOobject>();
            AssetDatabase.CreateAsset(soObject, "Assets/SOobject.asset");
            AssetDatabase.SaveAssets();
        }
    }

    private void OnGUI()
    {
        if (soObject == null)
        {
            EditorGUILayout.LabelField("No SOobject found.");
            return;
        }

        EditorGUILayout.LabelField("GameObject List", EditorStyles.boldLabel);

        // Display list of GameObjects
        for (int i = 0; i < soObject.gameObjects.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            soObject.gameObjects[i] = (GameObject)EditorGUILayout.ObjectField(soObject.gameObjects[i], typeof(GameObject), true);

            if (GUILayout.Button("Remove"))
            {
                RemoveGameObject(i);
                break; // Break to avoid modifying the list while iterating
            }
            EditorGUILayout.EndHorizontal();
        }

        // Add button
        if (GUILayout.Button("Add GameObject"))
        {
            AddGameObject();
        }

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(soObject);
            AssetDatabase.SaveAssets();
        }
    }

    private void AddGameObject()
    {
        soObject.gameObjects.Add(null); // Thêm phần tử null vào danh sách
    }

    private void RemoveGameObject(int index)
    {
        if (index >= 0 && index < soObject.gameObjects.Count)
        {
            soObject.gameObjects.RemoveAt(index); // Xóa phần tử tại index
        }
    }
}

*//*


    [MenuItem("Window/TestEditor")]
    public static void ShowWindow()
    {
        GetWindow<TestEditor>("TestEditor");
    }

    private SOobject soObject;
    private SpawnerData selectedWaveData;
    private bool[,] grid; // Thêm mảng lưới để chỉnh sửa

    private void OnGUI()
    {
        // Hiển thị trường chọn SOobject
        soObject = (SOobject)EditorGUILayout.ObjectField("SOobject", soObject, typeof(SOobject), false);

        if (soObject != null)
        {
            // Hiển thị các nút cho từng WaveData
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Select Wave Data", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            for (int i = 0; i < soObject.Wave.Count; i++)
            {
                if (GUILayout.Button($"Level {i}", GUILayout.Height(30), GUILayout.Width(100)))
                {
                    // Khi nhấn nút, lưu WaveData được chọn
                    selectedWaveData = soObject.Wave[i];
                    InitializeGrid(selectedWaveData);
                    Repaint(); // Cập nhật giao diện để hiển thị dữ liệu
                }
            }

            EditorGUILayout.Space();

            // Hiển thị dữ liệu của WaveData nếu có
            if (selectedWaveData != null)
            {
                EditorGUILayout.LabelField("Wave Data Details", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                GUIStyle boxStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    padding = new RectOffset(10, 10, 10, 10)
                };

                EditorGUILayout.BeginVertical(boxStyle);
                EditorGUILayout.LabelField($"Width: {selectedWaveData.width}", EditorStyles.label);
                EditorGUILayout.LabelField($"Height: {selectedWaveData.height}", EditorStyles.label);
                EditorGUILayout.LabelField($"Index Line: {selectedWaveData.indexLine}", EditorStyles.label);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Tiles:", EditorStyles.boldLabel);

                // Hiển thị lưới và cho phép chỉnh sửa
                DrawEditableGrid(selectedWaveData);

                EditorGUILayout.EndVertical();
            }
        }
    }

    private void InitializeGrid(SpawnerData waveData)
    {
        // Khởi tạo lưới từ dữ liệu WaveData
        grid = new bool[waveData.width, waveData.height];
        for (int y = 0; y < waveData.height; y++)
        {
            for (int x = 0; x < waveData.width; x++)
            {
                grid[x, y] = waveData.tiles[y * waveData.width + x] == TileType.Active;
            }
        }
    }

    private void DrawEditableGrid(SpawnerData waveData)
    {
        // Hiển thị và cho phép chỉnh sửa lưới
        for (int y = 0; y < waveData.height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < waveData.width; x++)
            {
                // Tạo GUIStyle để điều chỉnh màu sắc
                GUIStyle toggleStyle = new GUIStyle(GUI.skin.button);
                toggleStyle.normal.background = MakeTex(2, 2, grid[x, y] ? Color.green : Color.gray);

                // Thay đổi trạng thái của ô vuông và cập nhật lại lưới
                grid[x, y] = GUILayout.Toggle(grid[x, y], "", toggleStyle, GUILayout.Width(20), GUILayout.Height(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        // Nút Save sẽ lưu trạng thái của lưới vào asset
        if (GUILayout.Button("Save"))
        {
            // Cập nhật mảng tiles của WaveData
            waveData.tiles = new TileType[waveData.width * waveData.height];
            for (int y = 0; y < waveData.height; y++)
            {
                for (int x = 0; x < waveData.width; x++)
                {
                    waveData.tiles[y * waveData.width + x] = grid[x, y] ? TileType.Active : TileType.Inactive;
                }
            }

            // Đánh dấu WaveData là đã bị thay đổi để Unity lưu lại
            EditorUtility.SetDirty(waveData);

            // Thông báo đã lưu thành công
            Debug.Log("Lưới đã được lưu vào asset.");
        }
    }

    // Hàm tạo texture màu cho GUIStyle
    private Texture2D MakeTex(int width, int height, Color col)
    {
        // Tạo một mảng các đối tượng Color với kích thước là width * height
        Color[] pix = new Color[width * height];

        // Đặt màu cho mỗi pixel trong mảng là màu col được truyền vào
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }

        // Tạo một đối tượng Texture2D mới với chiều rộng và chiều cao được chỉ định
        Texture2D result = new Texture2D(width, height);

        // Áp dụng mảng màu pix vào texture
        result.SetPixels(pix);

        // Áp dụng tất cả các thay đổi đã thực hiện trên texture
        result.Apply();

        // Trả về đối tượng Texture2D đã được tạo và điền màu sắc
        return result;
    }

*/