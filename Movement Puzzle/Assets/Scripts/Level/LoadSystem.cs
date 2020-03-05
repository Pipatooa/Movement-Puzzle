using UnityEngine;
using System.IO;

public static class LoadSystem
{
    // Temporary function to create test level until level editor is complete
    public static LevelData CreateTestLevel()
    {
        LevelData levelData = new LevelData("Test level", 20, 20);

        LevelData.PlayerInfo player1 = new LevelData.PlayerInfo();
        player1.posX = 3;
        player1.posY = 3;

        player1.facingDir = 0;
        player1.lastMoveDir = 3;

        player1.colorIndex = 0;
        player1.colorIndexUp = 0;
        player1.colorIndexRight = 1;
        player1.colorIndexDown = 2;
        player1.colorIndexLeft = 3;

        LevelData.PlayerInfo player2 = new LevelData.PlayerInfo();
        player2.posX = 3;
        player2.posY = 7;

        player2.facingDir = 0;
        player2.lastMoveDir = 0;

        player2.colorIndex = 1;
        player2.colorIndexUp = 0;
        player2.colorIndexRight = 1;
        player2.colorIndexDown = 2;
        player2.colorIndexLeft = 3;

        levelData.players.Add(player1);
        levelData.players.Add(player2);

        for (int x=0; x < 20; x++)
        {
            for (int y=0; y < 20; y++)
            {
               levelData.tileArray[x, y] = new Tiles.PlainTile();
            }
        }

        for (int x=1; x < 10; x++)
        {
            levelData.tileArray[x, 10] = new Tiles.ColorTile();
            levelData.tileArray[x, 11] = new Tiles.ColorTile();
            levelData.tileArray[x, 12] = new Tiles.ColorTile();
            levelData.tileArray[x, 13] = new Tiles.ColorTile();

            levelData.tileArray[x, 10].colorIndex = 0;
            levelData.tileArray[x, 11].colorIndex = 1;
            levelData.tileArray[x, 12].colorIndex = 2;
            levelData.tileArray[x, 13].colorIndex = 3;
        }

        levelData.tileArray[9, 4] = new Tiles.Goal();

        return levelData;
    }
    
    // Save a level data object to file
    public static void SaveLevel(LevelData levelData, string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Levels", fileName);

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            // Write basic level details
            writer.Write(levelData.levelName);
            writer.Write((byte) levelData.sizeX);
            writer.Write((byte) levelData.sizeY);

            writer.Write((byte) levelData.players.Count);

            // Write player data
            for (int i = 0; i < levelData.players.Count; i++)
            {
                writer.Write((byte) levelData.players[i].posX);
                writer.Write((byte) levelData.players[i].posY);
                
                writer.Write((byte) levelData.players[i].facingDir);
                writer.Write((byte) levelData.players[i].lastMoveDir);

                writer.Write((sbyte) levelData.players[i].colorIndex);
                writer.Write((sbyte) levelData.players[i].colorIndexUp);
                writer.Write((sbyte) levelData.players[i].colorIndexRight);
                writer.Write((sbyte) levelData.players[i].colorIndexDown);
                writer.Write((sbyte) levelData.players[i].colorIndexLeft);
            }

            // Write tile data
            for (int x = 0; x < levelData.sizeX; x++)
            {
                for (int y = 0; y < levelData.sizeY; y++)
                {
                    Tiles.BaseTile tile = levelData.tileArray[x, y];

                    writer.Write((byte)((tile.objectID << 4 & 0xF0) | (tile.colorIndex & 0x0F)));

                    if (tile.objectID != 0)
                    {
                        writer.Write(tile.GetAdditionalInfo());
                    }
                }
            }
        }
    }

    // Load a level data object from file
    public static LevelData LoadLevel(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Levels", fileName);

        if (File.Exists(path))
        {
            LevelData levelData;

            string levelName;
            int sizeX, sizeY;

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                // Load basic level details
                levelName = reader.ReadString();
                sizeX = reader.ReadByte();
                sizeY = reader.ReadByte();

                // Create a new level object to be populated
                levelData = new LevelData(levelName, sizeX, sizeY);

                int numPlayers = reader.ReadByte();

                // Load player info
                for (int i = 0; i < numPlayers; i++)
                {
                    LevelData.PlayerInfo player = new LevelData.PlayerInfo();

                    player.posX = reader.ReadByte();
                    player.posY = reader.ReadByte();
                    
                    player.facingDir = reader.ReadByte();
                    player.lastMoveDir = reader.ReadByte();
                    
                    player.colorIndex = reader.ReadSByte();
                    player.colorIndexUp = reader.ReadSByte();
                    player.colorIndexRight = reader.ReadSByte();
                    player.colorIndexDown = reader.ReadSByte();
                    player.colorIndexLeft = reader.ReadSByte();

                    levelData.players.Add(player);
                }

                byte tileInfo;
                int objectID;
                int colorIndex;

                // Load tile info
                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        tileInfo = reader.ReadByte();
                        objectID = (tileInfo & 0xF0) >> 4;
                        colorIndex = tileInfo & 0x0F;

                        levelData.tileArray[x, y] = Utils.IDToTile(objectID);

                        levelData.tileArray[x, y].x = x;
                        levelData.tileArray[x, y].y = y;

                        levelData.tileArray[x, y].colorIndex = colorIndex;

                        if (objectID != 0)
                        {
                            levelData.tileArray[x, y].LoadAdditionalInfo(reader.ReadByte());
                        }
                    }
                }
            }

            return levelData;
        } else
        {
            Debug.LogError("No level found at '" + path + "'");
            return null;
        }
    }
}
