using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public string levelName;

    [Range(0f, 1f)] public float tileSize;
    [Range(0f, 1f)] public float tileSizeSmall;
    [Range(0f, 1f)] public float colorIntensity;

    public ColorScheme colorScheme;
    public Material tileMaterial;
    
    public GameObject tilePrefab;
    public GameObject playerPefab;
    public GameObject goalPrefab;

    public LevelData levelData;

    [HideInInspector] public TileManager tileManager;
    [HideInInspector] public PlayerManager playerManager;
    
    Material[] materials;

    private void Awake()
    {
        Level.levelGenerator = this;
        Level.colorScheme = colorScheme;
    }

    void Start()
    {
        materials = new Material[colorScheme.colors.Count];
        for (int i = 0; i < colorScheme.colors.Count; i++)
        {
            materials[i] = new Material(colorScheme.shader);
            materials[i].name = "Tile (Color " + i + ")";
            materials[i].CopyPropertiesFromMaterial(tileMaterial);
            materials[i].color = Color.Lerp(tileMaterial.color, colorScheme.colors[i].material.color, 0.75f);
        }

        levelData = LoadSystem.LoadLevel(levelName);
        Level.levelData = levelData;

        LoadLevel();
    }

    void LoadLevel()
    {
        // Objects
        GameObject tileParent = new GameObject("Tiles");
        tileParent.transform.parent = gameObject.transform;

        GameObject goalParent = new GameObject("Goal");
        goalParent.transform.SetParent(gameObject.transform);
        goalParent.transform.position = new Vector3(levelData.goalX, 2.5f, levelData.goalY);
        GameObject goal = Instantiate(goalPrefab, goalParent.transform);
        goal.GetComponent<Renderer>().material = colorScheme.goalColor.material;

        GameObject playerParent = new GameObject("Players");
        playerParent.transform.SetParent(gameObject.transform);

        // Scripts
        tileManager = tileParent.AddComponent<TileManager>();
        tileManager.colorGroups = new List<LevelData.Tile>[colorScheme.colors.Count];
        for (int i = 0; i < colorScheme.colors.Count; i++) tileManager.colorGroups[i] = new List<LevelData.Tile>();
        tileManager.levelGenerator = this;

        playerManager = playerParent.AddComponent<PlayerManager>();
        Level.playerManager = playerManager;

        // Tiles
        for (int x = 0; x < levelData.sizeX; x++)
        {
            for (int y = 0; y < levelData.sizeY; y++)
            {
                if (levelData.tileArray[x, y].exists)
                {
                    GameObject tileObject = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), tileParent.transform);
                    tileObject.transform.localScale *= tileSize;

                    levelData.tileArray[x, y].gameObject = tileObject;

                    LevelData.Tile tile = levelData.tileArray[x, y];

                    if (levelData.goalX == x && levelData.goalY == y)
                    {
                        tileObject.GetComponent<Renderer>().material = colorScheme.goalColor.material;
                    }
                    else if (tile.colorIndex == -1)
                    {
                        tileObject.isStatic = true;
                    }
                    else
                    {
                        tileObject.GetComponent<Renderer>().material = materials[tile.colorIndex];
                        tileManager.colorGroups[tile.colorIndex].Add(tile);
                    }
                }
            }
        }

        // Players
        foreach (LevelData.PlayerInfo playerInfo in levelData.players)
        {
            GameObject playerObject = Instantiate(playerPefab, new Vector3(playerInfo.posX, 0.5f, playerInfo.posY), Quaternion.identity, playerParent.transform);
            Player playerScript = playerObject.GetComponent<Player>();
            
            playerScript.LoadState(playerInfo, true);

            playerManager.players.Add(playerScript);
        }

        playerManager.UpdateColorCount();
    }
}
