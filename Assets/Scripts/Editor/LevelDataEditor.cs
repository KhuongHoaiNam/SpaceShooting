using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor // Sửa lỗi chính tả ở đây
{
   /* public override void OnInspectorGUI()
    {
        LevelData levelData = (LevelData)target;

        // Hiển thị biến level
        //levelData.level = EditorGUILayout.IntField("Level", levelData.level);

        // Nút Load sẽ tải các dữ liệu wave vào asset
        if (GUILayout.Button("Load"))
        {
            levelData.OnLoad();
            EditorUtility.SetDirty(levelData); // Đánh dấu LevelData là đã thay đổi
        }

        // Hiển thị danh sách các WaveData
        if (levelData.waveData != null)
        {
            foreach (var wave in levelData.waveData)
            {
                EditorGUILayout.ObjectField("Wave Data", wave, typeof(SpawnerData), false);
            }
        }

        // Cập nhật giao diện người dùng
        serializedObject.Update();
    }*/
}