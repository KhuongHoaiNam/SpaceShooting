
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public List<WaveData> waveData = new List<WaveData>(); // Khởi tạo danh sách
    public void LoaderWavesDataResource(int idLeves)
    {
        WaveData[] loadedLevels = Resources.LoadAll<WaveData>($"LevelData/LevelTotal/Level{idLeves}");

        // Chuyển đổi mảng thành List và gán vào biến levels
        waveData = new List<WaveData>(loadedLevels);

        // Kiểm tra danh sách đã load đúng các level chưa
        for (int i = 0; i < loadedLevels.Length; i++)
        {
            // Đặt tên mới cho wave
            string newName = $"Wave {i + 1}";

            // Đổi tên của ScriptableObject trong bộ nhớ
            waveData[i].name = newName;

            // Lấy đường dẫn hiện tại của file ScriptableObject
            string assetPath = AssetDatabase.GetAssetPath(waveData[i]);

            // Đổi tên file trong dự án
            AssetDatabase.RenameAsset(assetPath, newName);

            // Lưu lại và làm mới dự án
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Renamed and saved Wave: " + newName);
        }
    }
    public void SetupName()
    {
     
    }
}





    