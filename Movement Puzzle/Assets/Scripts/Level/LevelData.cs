using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
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

    public List<PlayerInfo> players;
    public Tiles.Tile[,] tileArray;

    public LevelData(string levelName, int sizeX, int sizeY)
    {
        this.levelName = levelName;
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        players = new List<PlayerInfo>();
        tileArray = new Tiles.Tile[sizeX, sizeY];
    }
}
