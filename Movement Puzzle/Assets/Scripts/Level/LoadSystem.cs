using UnityEngine;
using System.IO;

public static class LoadSystem
{
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

        levelData.players.Add(player1);

        for (int x=0; x < 20; x++)
        {
            for (int y=0; y < 20; y++)
            {
               levelData.tileArray[x, y] = new Tiles.PlainTile();
            }
        }

        levelData.tileArray[9, 4] = new Tiles.Goal();

        return levelData;
    }
    
    public static void SaveLevel(LevelData levelData, string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Levels", fileName);

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(levelData.levelName);
            writer.Write((byte) levelData.sizeX);
            writer.Write((byte) levelData.sizeY);

            writer.Write((byte) levelData.players.Count);

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

            for (int x = 0; x < levelData.sizeX; x++)
            {
                for (int y = 0; y < levelData.sizeY; y++)
                {
                    Tiles.BaseTile tile = levelData.tileArray[x, y];

                    // [0000][0000] - [objectID][colorIndex]
                    writer.Write((byte)((tile.objectID << 4 & 0xF0) | (tile.colorIndex & 0x0F)));

                    if (tile.objectID != 0)
                    {
                        writer.Write(tile.GetAdditionalInfo());
                    }
                }
            }
        }
    }

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
                levelName = reader.ReadString();
                sizeX = reader.ReadByte();
                sizeY = reader.ReadByte();

                levelData = new LevelData(levelName, sizeX, sizeY);

                int numPlayers = reader.ReadByte();

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

                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        tileInfo = reader.ReadByte();
                        objectID = (tileInfo & 0xF0) >> 4;
                        colorIndex = tileInfo & 0x0F;

                        switch (objectID)
                        {
                            case 0:
                                levelData.tileArray[x, y] = new Tiles.BaseTile();
                                break;
                            case 1:
                                levelData.tileArray[x, y] = new Tiles.Goal();
                                break;
                            case 2:
                                levelData.tileArray[x, y] = new Tiles.PlainTile();
                                break;
                            case 3:
                                levelData.tileArray[x, y] = new Tiles.ColorTile();
                                break;
                            case 4:
                                levelData.tileArray[x, y] = new Tiles.Switch();
                                break;
                            case 5:
                                levelData.tileArray[x, y] = new Tiles.Rotator();
                                break;
                            case 6:
                                levelData.tileArray[x, y] = new Tiles.Teleporter();
                                break;
                        }

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
