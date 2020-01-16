using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour
{
    public List<LevelData.Tile>[] colorGroups;

    void Awake()
    {
        Events.LevelUpdate += LevelUpdate;
        SceneManager.sceneUnloaded += delegate { OnSceneUnloaded(); };
    }

    void OnSceneUnloaded()
    {
        Events.LevelUpdate -= LevelUpdate;
    }

    void Start()
    {
        colorGroups = new List<LevelData.Tile>[LevelInfo.colorScheme.colors.Count];

        for (int i = 0; i < LevelInfo.colorScheme.colors.Count; i++)
        {
            colorGroups[i] = new List<LevelData.Tile>();
        }
    }

    void LevelUpdate()
    {
        for (int i = 0; i < colorGroups.Length; i++)
        {
            if (LevelInfo.playerManager.colorStatuses[i])
            {
                foreach (LevelData.Tile tile in colorGroups[i])
                {
                    tile.gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSize;
                    LevelInfo.levelData.tileArray[tile.x, tile.y].traversable = true;
                }
            } else
            {
                foreach (LevelData.Tile tile in colorGroups[i])
                {
                    tile.gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSizeSmall;
                    LevelInfo.levelData.tileArray[tile.x, tile.y].traversable = false;
                }
            }
        }
    }
}
