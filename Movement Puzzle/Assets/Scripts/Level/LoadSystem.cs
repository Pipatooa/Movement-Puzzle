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
            // writer.Write(new byte[] { });

            writer.Write(levelData.levelName);
            writer.Write((byte) levelData.sizeX);
            writer.Write((byte) levelData.sizeY);
            writer.Write((byte) levelData.goalX);
            writer.Write((byte) levelData.goalY);

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
                    LevelData.Tile tile = levelData.tileArray[x, y];

                    writer.Write(tile.exists);
                    writer.Write((sbyte) tile.colorIndex);
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

                levelData.goalX = reader.ReadByte();
                levelData.goalY = reader.ReadByte();

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

                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        levelData.tileArray[x, y] = new LevelData.Tile(x, y, reader.ReadBoolean(), reader.ReadSByte());
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
