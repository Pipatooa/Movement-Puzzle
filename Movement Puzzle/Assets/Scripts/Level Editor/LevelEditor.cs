using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelEditor
{
    public static LevelEditorGUI.LevelOptionsGUI levelOptionsGUI;
    public static LevelEditorGUI.SelectionGUI selectionGUI;
    public static LevelEditorGUI.TilePlacementGUI tilePlacementGUI;
    public static LevelEditorGUI.AddObjectGUI addObjectGUI;
    public static LevelEditorGUI.ObjectSettingsGUI objectSettingsGUI;

    public static Vector2Int selectionStart;
    public static Vector2Int selectionEnd;

    public static LevelObjects.BaseLevelObject selectedLevelObject;

    public static bool levelOpen;
    public static string filePath;

    // Creates a new level
    public static void CreateNewLevel()
    {
        // Creates an empty level
        LevelInfo.levelData = new LevelData("New Level", 64, 64);

        UpdateLevelName("New Level");

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                LevelInfo.levelData.tileArray[x, y] = new Tiles.BaseTile();
                LevelInfo.levelData.tileArray[x, y].x = x;
                LevelInfo.levelData.tileArray[x, y].y = y;
            }
        }
    }

    // Loads a level into the level editor
    public static void LoadLevel(string path)
    {
        // Load level
        LevelInfo.levelData = LoadSystem.LoadLevel(path);
        filePath = path;

        // Update level name
        UpdateLevelName(LevelInfo.levelData.levelName);

        // Reload all level objects
        ReloadLevel();
    }

    // Updates the level name
    public static void UpdateLevelName(string name)
    {
        levelOptionsGUI.levelNameField.text = name;
        LevelInfo.currentLevelName = name;
        LevelInfo.levelData.levelName = name;
    }

    // Sets tile at location and recreates game objects
    public static void SetTile(int x, int y, Tiles.BaseTile tile)
    {
        // Destroys game objects for existing tile
        LevelInfo.levelData.tileArray[x, y].DestroyGameObjects();

        // Create new tile
        LevelInfo.levelData.tileArray[x, y] = tile.CreateCopy();
        LevelInfo.levelData.tileArray[x, y].x = x;
        LevelInfo.levelData.tileArray[x, y].y = y;

        // Create new game objects for tile
        LevelInfo.levelData.tileArray[x, y].LevelEditorCreateGameObjects(LevelInfo.levelGenerator.tileParent.transform);
    }

    // Reloads all game objects for current level
    public static void ReloadLevel()
    {
        LevelInfo.levelGenerator.DestroyLevelObjects();
        LevelInfo.levelGenerator.LoadLevel();
    }
}
