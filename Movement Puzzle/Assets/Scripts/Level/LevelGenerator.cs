using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float tileSize = 0.9f;
    public float tileSizeSmall = 0.2f;
    public float colorIntensity = 0.75f;

    public ColorScheme colorScheme;
    public Material tileMaterial;
    
    public GameObject tilePrefab;
    public GameObject playerPrefab;
    public GameObject goalPrefab;

    public LevelData levelData;

    [HideInInspector] public TileManager tileManager;
    [HideInInspector] public PlayerManager playerManager;
    
    Material[] materials;

    void Awake()
    {
        LoadLevel(LevelInfo.currentLevel);
    }

    public void LoadLevel(string levelName)
    {
        // Setup
        levelData = LoadSystem.LoadLevel(levelName);
        LevelInfo.colorScheme = colorScheme;
        LevelInfo.levelGenerator = this;
        LevelInfo.levelData = levelData;

        ColorManager.colorCounts = new int[LevelInfo.colorScheme.colors.Count];
        ColorManager.colorStates = new bool[LevelInfo.colorScheme.colors.Count];

        materials = new Material[colorScheme.colors.Count];
        for (int i = 0; i < colorScheme.colors.Count; i++)
        {
            materials[i] = new Material(colorScheme.shader);
            materials[i].name = "Tile (Color " + i + ")";
            materials[i].CopyPropertiesFromMaterial(tileMaterial);
            materials[i].color = Color.Lerp(tileMaterial.color, colorScheme.colors[i].material.color, 0.75f);
        }

        // Objects
        GameObject tileParent = new GameObject("Tiles");
        tileParent.transform.parent = gameObject.transform;

        GameObject playerParent = new GameObject("Players");
        playerParent.transform.SetParent(gameObject.transform);

        // Scripts
        tileManager = tileParent.AddComponent<TileManager>();
        playerManager = playerParent.AddComponent<PlayerManager>();
        LevelInfo.playerManager = playerManager;

        // Tiles
        for (int x = 0; x < levelData.sizeX; x++)
        {
            for (int y = 0; y < levelData.sizeY; y++)
            {
                Tiles.Tile tile = levelData.tileArray[x, y];

                GameObject tileObject;

                switch (tile.objectID)
                {
                    case 1:
                        GameObject goalParent = new GameObject("Goal");
                        goalParent.transform.SetParent(tileParent.transform);
                        goalParent.transform.position = new Vector3(x, 2.5f, y);
                        
                        GameObject goal = Instantiate(goalPrefab, goalParent.transform);
                        goal.GetComponent<Renderer>().material = colorScheme.goalColor.material;

                        tileObject = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), goalParent.transform);
                        tileObject.transform.localScale *= tileSize;
                        tileObject.GetComponent<Renderer>().material = colorScheme.goalColor.material;
                        tileObject.isStatic = true;

                        break;
                    case 2:
                        // Tile
                        tileObject = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), tileParent.transform);
                        tileObject.transform.localScale *= tileSize;
                        tileObject.isStatic = true;

                        tile.gameObject = tileObject;
                        break;
                    case 3:
                        // Color Tile
                        tileObject = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), tileParent.transform);
                        tileObject.transform.localScale *= tileSize;
                        tileObject.GetComponent<Renderer>().material = materials[tile.colorIndex];

                        tile.gameObject = tileObject;
                        tileManager.colorGroups[tile.colorIndex].Add(tile);
                        break;
                    case 4:
                        // Switch
                        break;
                }
            }
        }

        // Players
        foreach (LevelData.PlayerInfo playerInfo in levelData.players)
        {
            GameObject playerObject = Instantiate(playerPrefab, new Vector3(playerInfo.posX, 0.5f, playerInfo.posY), Quaternion.identity, playerParent.transform);
            Player playerScript = playerObject.GetComponent<Player>();
            
            playerScript.LoadState(playerInfo, true);

            playerManager.players.Add(playerScript);
        }

        UndoSystem.ClearStates();
        ColorManager.CalculateColors();
    }
}
