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
    public Material tileMaterial;

    public LevelData levelData;

    GameObject tileParent;
    GameObject playerParent;

    [HideInInspector] public TileManager tileManager;
    [HideInInspector] public PlayerManager playerManager;
    
    Material[] materials;

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
            LevelInfo.tileMaterials[i].CopyPropertiesFromMaterial(tileMaterial);
            LevelInfo.tileMaterials[i].color = Color.Lerp(tileMaterial.color, colorScheme.colors[i].material.color, 0.75f);
        }

        // Create empty parent objects to group objects
        tileParent = new GameObject("Tiles");
        tileParent.transform.parent = gameObject.transform;

        playerParent = new GameObject("Players");
        playerParent.transform.SetParent(gameObject.transform);

        // Load in level objects
        LoadScripts();
        LoadTiles();
        LoadPlayers();

        // Once level has finished loading, calculate initial level state
        UndoSystem.ClearStates();

        ColorManager.ResetColorCounts();
        ColorManager.CalculateColors();

        Events.LevelUpdate();
    }

    // Loads in all manager scripts
    void LoadScripts()
    {
        tileManager = tileParent.AddComponent<TileManager>();
        playerManager = playerParent.AddComponent<PlayerManager>();

        LevelInfo.tileManager = tileManager;
        LevelInfo.playerManager = playerManager;
    }

    // Loads tiles of level
    void LoadTiles()
    {
        // Iterate through tiles
        for (int x = 0; x < levelData.sizeX; x++)
        {
            for (int y = 0; y < levelData.sizeY; y++)
            {
                // Create game objects for each tile
                levelData.tileArray[x, y].CreateGameObjects(tileParent.transform);
            }
        }
    }

    // Loads all players and player info into level
    void LoadPlayers()
    {
        // Load each player
        foreach (LevelData.PlayerInfo playerInfo in levelData.players)
        {
            GameObject playerObject = Instantiate(levelAssets.player, new Vector3(playerInfo.posX, 0.5f, playerInfo.posY), Quaternion.identity, playerParent.transform);
            Player playerScript = playerObject.GetComponent<Player>();

            playerScript.LoadInfo(playerInfo);

            playerManager.players.Add(playerScript);
        }
    }
}
