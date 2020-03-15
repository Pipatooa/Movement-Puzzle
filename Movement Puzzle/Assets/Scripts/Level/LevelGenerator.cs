using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public bool editorMode;
    
    public float tileSize = 0.9f;
    public float tileSizeSmall = 0.2f;
    public float colorIntensity = 0.75f;

    public ColorScheme colorScheme;
    public LevelAssets levelAssets;

    [HideInInspector] public GameObject tileParent;
    [HideInInspector] public GameObject levelObjectParent;

    [HideInInspector] public PlayerManager playerManager;

    void Awake()
    {
        // Setup
        LevelInfo.levelGenerator = this;
        LevelInfo.colorScheme = colorScheme;
        LevelInfo.levelAssets = levelAssets;

        // Create all color materials for tiles
        LevelInfo.tileMaterials = new Material[colorScheme.colors.Count];
        for (int i = 0; i < colorScheme.colors.Count; i++)
        {
            LevelInfo.tileMaterials[i] = new Material(colorScheme.shader);
            LevelInfo.tileMaterials[i].name = "Tile (Color " + i + ")";
            LevelInfo.tileMaterials[i].CopyPropertiesFromMaterial(colorScheme.defaultTileColor.material);
            LevelInfo.tileMaterials[i].color = Color.Lerp(colorScheme.defaultTileColor.material.color, colorScheme.colors[i].material.color, 0.75f);
        }

        if (editorMode)
        {
            // Use an empty level if in editor mode
            LevelEditor.CreateNewLevel();
        }
        else
        {
            // Load level data from file
            LevelInfo.levelData = LoadSystem.LoadLevel(LevelInfo.currentLevelName);
        }

        // Generate level
        LoadLevel();
    }

    // Loads in the current level
    public void LoadLevel()
    {
        // Create empty parent objects to group objects
        tileParent = new GameObject("Tiles");
        tileParent.transform.parent = gameObject.transform;

        levelObjectParent = new GameObject("Level Objects");
        levelObjectParent.transform.SetParent(gameObject.transform);

        // Load in player manager
        if (!editorMode)
        {
            playerManager = levelObjectParent.AddComponent<PlayerManager>();
            LevelInfo.playerManager = playerManager;
        }

        // Load tiles
        for (int x = 0; x < LevelInfo.levelData.sizeX; x++)
        {
            for (int y = 0; y < LevelInfo.levelData.sizeY; y++)
            {
                // Create game objects for each tile
                if (editorMode)
                {
                    LevelInfo.levelData.tileArray[x, y].LevelEditorCreateGameObjects(tileParent.transform);
                } else
                {
                    LevelInfo.levelData.tileArray[x, y].CreateGameObjects(tileParent.transform);
                }
            }
        }

        // Load level objects
        foreach (LevelObjects.BaseLevelObject levelObject in LevelInfo.levelData.levelObjects)
        {
            // Create game objects for each level object
            if (editorMode)
            {
                levelObject.CreateGameObjects(levelObjectParent.transform);
            }
            else
            {
                levelObject.CreateGameObjects(levelObjectParent.transform);
            }
        }

        // Once level has finished loading, calculate initial level state
        if (!editorMode)
        {
            ColorManager.ResetColorCounts();
            Events.LevelInit?.Invoke();
            ColorManager.CalculateColors();
            Events.LevelUpdate?.Invoke();

            UndoSystem.ClearStates();
        }
    }

    // Destroys all game objects for the level
    public void DestroyLevelObjects()
    {
        for (int x=0; x < LevelInfo.levelData.sizeX; x++)
        {
            for (int y=0; y < LevelInfo.levelData.sizeY; y++)
            {
                LevelInfo.levelData.tileArray[x, y].DestroyGameObjects();
            }
        }

        foreach (LevelObjects.BaseLevelObject levelObject in LevelInfo.levelData.levelObjects)
        {
            levelObject.DestroyGameObjects();
        }

        GameObject.Destroy(tileParent);
        GameObject.Destroy(levelObjectParent);
    }
}
