using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveControllerTool : EditorWindow
{
    [MenuItem("Window/Wave Controller Config")]
    public static void ShowWindows()
    {
        GetWindow<WaveControllerTool>("Wave Controller Nam 10 diem");
    }

    private void OnGUI()
    {
        GUILayout.Label("LevelList", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Levels", GUILayout.Height(30), GUILayout.Width(100)))
        {
                
        }

    }
}
