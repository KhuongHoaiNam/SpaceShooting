using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[CustomEditor(typeof(LevelControler))]
public class LevelControlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Gọi phương thức gốc để hiển thị các trường khác trong Inspector
        DrawDefaultInspector();

        LevelControler levelControler = (LevelControler)target;

        // Thêm một nút vào Inspector
        if (GUILayout.Button("Generate Level"))
        {
            levelControler.GenerateLevel();
        }
    }
}