using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public List<LevelData.Tile>[] colorGroups;

    void Awake()
    {
        Events.LevelUpdate += LevelUpdate;
    }

    void LevelUpdate()
    {
        for (int i = 0; i < colorGroups.Length; i++)
        {
            if (levelGenerator.playerManager.colorStatuses[i])
            {
                foreach (LevelData.Tile tile in colorGroups[i])
                {
                    tile.gameObject.transform.localScale = Vector3.one * levelGenerator.tileSize;
                    levelGenerator.levelData.tileArray[tile.x, tile.y].traversable = true;
                }
            } else
            {
                foreach (LevelData.Tile tile in colorGroups[i])
                {
                    tile.gameObject.transform.localScale = Vector3.one * levelGenerator.tileSizeSmall;
                    levelGenerator.levelData.tileArray[tile.x, tile.y].traversable = false;
                }
            }
        }
    }
}
