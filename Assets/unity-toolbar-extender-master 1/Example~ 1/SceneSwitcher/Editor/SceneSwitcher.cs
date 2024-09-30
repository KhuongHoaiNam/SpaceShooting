using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace UnityToolbarExtender.Examples
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
            commandButtonStyle.fontSize = 12;
        }
	}

    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            GUIStyle largerButtonStyle = new GUIStyle(ToolbarStyles.commandButtonStyle);
            largerButtonStyle.fixedWidth = 70f; // Set the height of the button
            largerButtonStyle.fixedHeight = 20f;
            largerButtonStyle.normal.textColor = new Color(0f / 255f, 190f / 255f, 190f / 255f);
           
            if (GUILayout.Button(new GUIContent("Splash", "Switch to Splash Scene"), largerButtonStyle))
            {
				SceneHelper.OnSwichSence("LoadingSence");
				//SceneHelper.SwitchToScene("GameScence");
                
            }
            if (GUILayout.Button(new GUIContent("MennuView", "Start Scene 2"), largerButtonStyle))
            {
                SceneHelper.OnSwichSence("MennuView");
            }

            if (GUILayout.Button(new GUIContent("Game", "Start Scene 2"), largerButtonStyle))
            {
                SceneHelper.OnSwichSence("GameSence");
            }

            if (GUILayout.Button(new GUIContent("Clear Data", "Start Scene 2"), largerButtonStyle))
            {
                Datamanager.Instance.DeleteUserData();
            }
        }
    }

    [InitializeOnLoad]
    public class SceneSwitchRightButton
    {
        static SceneSwitchRightButton()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUIStyle largerButtonStyle = new GUIStyle(ToolbarStyles.commandButtonStyle);
            largerButtonStyle.fixedWidth = 70f;
            largerButtonStyle.fixedHeight = 20f;
            largerButtonStyle.normal.textColor = new Color(0f / 255f, 190f / 255f, 190f / 255f);

            if (GUILayout.Button(new GUIContent("Play", "Start Scene 2"), largerButtonStyle))
            {
                SceneHelper.StartScene("LoadingSence");
            }
            if (GUILayout.Button(new GUIContent("Test", "Open Settings Scene"), largerButtonStyle))
            {
                SceneHelper.OnSwichSence("Test");
            }

            // Add more buttons as needed
        }
    }
    static class SceneHelper
	{
		static string sceneToOpen;

		public static void StartScene(string sceneName)
		{
			if (EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}

			sceneToOpen = sceneName;
			EditorApplication.update += OnUpdate;
		}

		public static void OnSwichSence(string sceneName)
		{

            sceneToOpen = sceneName;
            EditorApplication.update += OnSwich;
        }
		static void OnUpdate()
		{
			if (sceneToOpen == null ||
				EditorApplication.isPlaying || EditorApplication.isPaused ||
				EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			EditorApplication.update -= OnUpdate;

			if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				// need to get scene via search because the path to the scene
				// file contains the package version so it'll change over time
				string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
				if (guids.Length == 0)
				{
					Debug.LogWarning("Couldn't find scene file");
				}
				else
				{
					string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
					EditorSceneManager.OpenScene(scenePath); 
				    EditorApplication.isPlaying = true;
				}
			}
			sceneToOpen = null;
		}

		static void OnSwich()
		{
            EditorApplication.update -= OnSwich;
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // need to get scene via search because the path to the scene
                // file contains the package version so it'll change over time
                string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
            sceneToOpen = null;
        }
 
    }
}