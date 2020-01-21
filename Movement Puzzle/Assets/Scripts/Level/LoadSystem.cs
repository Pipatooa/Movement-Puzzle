using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class LoadSystem
{
    public static void SaveLevel(LevelData levelData, string fileName)
    {
        string path = Path.Combine(Application.dataPath, "Level Data", fileName);

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
                    Tiles.Tile tile = levelData.tileArray[x, y];

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
                byte[] header = reader.ReadBytes(3);
                
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
                                levelData.tileArray[x, y] = new Tiles.Tile();
                                break;
                            case 1:
                                levelData.tileArray[x, y] = new Tiles.Goal();
                                break;
                            case 2:
                                levelData.tileArray[x, y] = new Tiles.ColorTile();
                                break;
                            case 3:
                                levelData.tileArray[x, y] = new Tiles.Switch();
                                break;
                        }

                        levelData.tileArray[x, y].x = x;
                        levelData.tileArray[x, y].y = y;

                        levelData.tileArray[x, y].objectID = objectID;
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
