using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds information about the current level
public static class LevelInfo
{
    public static ColorScheme colorScheme;
    public static LevelAssets levelAssets;

    public static Material[] tileMaterials;

    public static string currentLevel = "testLevel.level";

    public static LevelData levelData;

    public static LevelGenerator levelGenerator;
    public static PlayerManager playerManager;

    public static List<LevelObjects.BaseLevelObject> levelObjects;
}
