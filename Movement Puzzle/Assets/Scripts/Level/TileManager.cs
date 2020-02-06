using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<Tiles.BaseTile>[] colorGroups;

    void Awake()
    {
        Events.LevelUpdate += LevelUpdate;

        colorGroups = new List<Tiles.BaseTile>[LevelInfo.colorScheme.colors.Count];

        for (int i = 0; i < LevelInfo.colorScheme.colors.Count; i++)
        {
            colorGroups[i] = new List<Tiles.BaseTile>();
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
                foreach (Tiles.BaseTile tile in colorGroups[i])
                {
                    tile.gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSize;
                    LevelInfo.levelData.tileArray[tile.x, tile.y].traversable = true;
                }
            } else
            {
                foreach (Tiles.BaseTile tile in colorGroups[i])
                {
                    tile.gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSizeSmall;
                    LevelInfo.levelData.tileArray[tile.x, tile.y].traversable = false;
                }
            }
        }
    }
}
