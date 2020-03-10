using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public string levelName;
    public int sizeX, sizeY;

    public Tiles.BaseTile[,] tileArray;
    public List<LevelObjects.BaseLevelObject> levelObjects = new List<LevelObjects.BaseLevelObject>();
    
    public LevelData(string levelName, int sizeX, int sizeY)
    {
        this.levelName = levelName;
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        tileArray = new Tiles.BaseTile[sizeX, sizeY];
    }
}
