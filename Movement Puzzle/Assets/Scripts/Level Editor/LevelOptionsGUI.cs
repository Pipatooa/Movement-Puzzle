using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;

namespace LevelEditorGUI
{
    public class LevelOptionsGUI : MonoBehaviour
    {
        public GameObject guiParent;
        
        // Sets whether this GUI is visible
        public void SetVisibility(bool value)
        {
            guiParent.SetActive(value);
        }

        // New level button
        public void NewLevel()
        {
            LevelEditor.CreateNewLevel();
            LevelEditor.ReloadLevel();

            LevelEditor.selectionGUI.ResetSelection();
        }

        // Load level button
        public void LoadLevel()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open Level", "", "level", false);

            // If no file selected, ignore
            if (paths.Length == 0) return;

            LevelEditor.LoadLevel(paths[0]);

            LevelEditor.selectionGUI.ResetSelection();
        }

        // Save level button
        public void SaveLevel()
        {
            string path = StandaloneFileBrowser.SaveFilePanel("Save Level", "", LevelInfo.currentLevelName + ".level", "level");

            // If no filepath chose, ignore
            if (path.Length == 0) return;

            LevelEditor.filePath = path;

            LoadSystem.SaveLevel(LevelInfo.levelData, path);
        }

    }
}