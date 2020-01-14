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
        
    }
}
