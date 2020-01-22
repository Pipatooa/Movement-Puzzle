using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FunctionTestWindow : EditorWindow
{
    [MenuItem("Window/Function Tester")]
    public static void ShowWindow()
    {
        GetWindow<FunctionTestWindow>("Function Tester");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Save Test Level"))
        {
            LevelData levelData = LoadSystem.CreateTestLevel();

            LoadSystem.SaveLevel(levelData, "testLevel.level");
        }

        if (GUILayout.Button("Load Test Level"))
        {
            LevelData levelData = LoadSystem.LoadLevel("testLevel.level");
        }
    }
}
