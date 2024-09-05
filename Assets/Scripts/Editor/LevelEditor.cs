    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(SpawnerData))]
    public class LevelEditor : Editor
    {
        private bool[,] grid;

        public override void OnInspectorGUI()
        {
            SpawnerData level = (SpawnerData)target;

            // Điều chỉnh kích thước của lưới
            level.width = EditorGUILayout.IntField("Width", level.width);
            level.height = EditorGUILayout.IntField("Height", level.height);
            level.indexLine = EditorGUILayout.IntField("index line",level.indexLine);
            // Khởi tạo lưới nếu cần
            if (grid == null || grid.GetLength(0) != level.width || grid.GetLength(1) != level.height)
            {
                grid = new bool[level.width, level.height];

                // Khởi tạo trạng thái của lưới từ tiles của LevelData
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
            }

            // Hiển thị lưới và cho phép chỉnh sửa
            for (int y = 0; y < level.height; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < level.width; x++)
                {
                    // Tạo GUIStyle để điều chỉnh màu sắc
                    GUIStyle toggleStyle = new GUIStyle(GUI.skin.button);
                    toggleStyle.normal.background = MakeTex(2, 2, grid[x, y] ? Color.green : Color.gray);

                    // Thay đổi trạng thái của ô vuông và cập nhật lại lưới
                    grid[x, y] = GUILayout.Toggle(grid[x, y], "", toggleStyle, GUILayout.Width(50), GUILayout.Height(50));
                }
                EditorGUILayout.EndHorizontal();
            }

            // Nút Save sẽ lưu trạng thái của lưới vào asset
            if (GUILayout.Button("Save"))
            {
                // Cập nhật mảng tiles của LevelData
                level.tiles = new TileType[level.width * level.height];
                for (int y = 0; y < level.height; y++)
                {
                    for (int x = 0; x < level.width; x++)
                    {
                        level.tiles[y * level.width + x] = grid[x, y] ? TileType.Active : TileType.Inactive;
                    }
                }

                // Đánh dấu LevelData là đã bị thay đổi để Unity lưu lại
                EditorUtility.SetDirty(level);

                // Thông báo đã lưu thành công
                Debug.Log("Lưới đã được lưu vào asset.");
            }



        }

        // Hàm tạo texture màu cho GUIStyle
        private Texture2D MakeTex(int width, int height, Color col)
        {
            // Tạo một mảng các đối tượng Color với kích thước là width * height
            // Mỗi phần tử trong mảng đại diện cho một pixel trong texture
            Color[] pix = new Color[width * height];

            // Vòng lặp này chạy qua tất cả các pixel và đặt màu cho từng pixel
            for (int i = 0; i < pix.Length; i++)
            {
                // Đặt màu cho mỗi pixel trong mảng là màu col được truyền vào
                pix[i] = col;
            }

            // Tạo một đối tượng Texture2D mới với chiều rộng và chiều cao được chỉ định
            Texture2D result = new Texture2D(width, height);

            // Áp dụng mảng màu pix vào texture, tức là texture sẽ được điền đầy màu sắc từ mảng pix
            result.SetPixels(pix);

            // Áp dụng tất cả các thay đổi đã thực hiện trên texture (đặt màu cho các pixel)
            result.Apply();

            // Trả về đối tượng Texture2D đã được tạo và điền màu sắc, sẵn sàng để sử dụng
            return result;
        }

    }
