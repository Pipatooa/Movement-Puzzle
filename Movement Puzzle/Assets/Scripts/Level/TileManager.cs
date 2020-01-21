using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<Tiles.Tile>[] colorGroups;

    void Awake()
    {
        Events.LevelUpdate += LevelUpdate;

        colorGroups = new List<Tiles.Tile>[LevelInfo.colorScheme.colors.Count];

        for (int i = 0; i < LevelInfo.colorScheme.colors.Count; i++)
        {
            colorGroups[i] = new List<Tiles.Tile>();
        }
    }

    void OnDestroy()
    {
        Events.LevelUpdate -= LevelUpdate;
    }

    void LevelUpdate()
    {
        for (int i = 0; i < colorGroups.Length; i++)
        {
            if (ColorManager.colorStates[i])
            {
                foreach (Tiles.Tile tile in colorGroups[i])
                {
                    tile.gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSize;
                    LevelInfo.levelData.tileArray[tile.x, tile.y].traversable = true;
                }
            } else
            {
                foreach (Tiles.Tile tile in colorGroups[i])
                {
                    tile.gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSizeSmall;
                    LevelInfo.levelData.tileArray[tile.x, tile.y].traversable = false;
                }
            }
        }
    }
}
