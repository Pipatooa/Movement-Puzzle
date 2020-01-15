using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public struct Tile
    {
        public int x;
        public int y;

        public bool exists;
        public int colorIndex;

        public bool traversable;

        public GameObject gameObject;

        public Tile(int x, int y, bool exists, int colorIndex)
        {
            this.x = x;
            this.y = y;
            
            this.exists = exists;
            this.colorIndex = colorIndex;
            
            traversable = true;

            gameObject = null;
        }
    }

    public struct PlayerInfo
    {
        public int posX, posY;

        public int facingDir;
        public int lastMoveDir;

        public bool reachedGoal;

        public int colorIndex;
        public int colorIndexUp;
        public int colorIndexRight;
        public int colorIndexDown;
        public int colorIndexLeft;

        public PlayerInfo(Player player)
        {
            posX = player.posX;
            posY = player.posY;

            facingDir = player.facingDir;
            lastMoveDir = player.lastMoveDir;

            reachedGoal = player.reachedGoal;

            colorIndex = player.colorIndex;
            colorIndexUp = player.colorIndexUp;
            colorIndexRight = player.colorIndexRight;
            colorIndexDown = player.colorIndexDown;
            colorIndexLeft = player.colorIndexLeft;
        }
    }

    public string levelName;
    public int sizeX, sizeY;

    public int goalX, goalY;

    public List<PlayerInfo> players;
    public Tile[,] tileArray;

    public LevelData(string levelName, int sizeX, int sizeY)
    {
        this.levelName = levelName;
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        players = new List<PlayerInfo>();
        tileArray = new Tile[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                tileArray[x, y] = new Tile(x, y, false, -1);
            }
        }
    }

    public override string ToString()
    {
        return "Name: " + levelName + "\nSize: " + tileArray.GetLength(0).ToString() + ", " + tileArray.GetLength(1).ToString();
    }
}
