using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float tileSize = 0.9f;
    public float tileSizeSmall = 0.2f;
    public float colorIntensity = 0.75f;

    public ColorScheme colorScheme;
    public LevelAssets levelAssets;

    public LevelData levelData;

    GameObject tileParent;
    GameObject levelObjectParent;

    [HideInInspector] public PlayerManager playerManager;

    void Awake()
    {
        LoadLevel(LevelInfo.currentLevel);
    }

    // Loads in the current level
    public void LoadLevel(string levelName)
    {
        // Setup
        levelData = LoadSystem.LoadLevel(levelName);
        LevelInfo.colorScheme = colorScheme;
        LevelInfo.levelAssets = levelAssets;
        LevelInfo.levelGenerator = this;
        LevelInfo.levelData = levelData;

        // Create all color materials for tiles
        LevelInfo.tileMaterials = new Material[colorScheme.colors.Count];
        for (int i = 0; i < colorScheme.colors.Count; i++)
        {
            LevelInfo.tileMaterials[i] = new Material(colorScheme.shader);
            LevelInfo.tileMaterials[i].name = "Tile (Color " + i + ")";
            LevelInfo.tileMaterials[i].CopyPropertiesFromMaterial(colorScheme.defaultTileColor.material);
            LevelInfo.tileMaterials[i].color = Color.Lerp(colorScheme.defaultTileColor.material.color, colorScheme.colors[i].material.color, 0.75f);
        }

        // Create empty parent objects to group objects
        tileParent = new GameObject("Tiles");
        tileParent.transform.parent = gameObject.transform;

        levelObjectParent = new GameObject("Level Objects");
        levelObjectParent.transform.SetParent(gameObject.transform);

        // Load in player manager
        playerManager = levelObjectParent.AddComponent<PlayerManager>();
        LevelInfo.playerManager = playerManager;

        // Load tiles
        for (int x = 0; x < levelData.sizeX; x++)
        {
            for (int y = 0; y < levelData.sizeY; y++)
            {
                // Create game objects for each tile
                levelData.tileArray[x, y].CreateGameObjects(tileParent.transform);
            }
        }

        // Load level objects
        foreach (LevelObjects.BaseLevelObject levelObject in levelData.levelObjects)
        {
            levelObject.CreateGameObjects(levelObjectParent.transform);
        }

        // Once level has finished loading, calculate initial level state
        UndoSystem.ClearStates();

        ColorManager.ResetColorCounts();
        ColorManager.CalculateColors();

        Events.LevelUpdate();
    }
}
