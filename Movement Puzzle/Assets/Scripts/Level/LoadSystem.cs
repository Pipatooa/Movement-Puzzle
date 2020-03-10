using UnityEngine;
using System.IO;

public static class LoadSystem
{
    // Temporary function to create test level until level editor is complete
    public static LevelData CreateTestLevel()
    {
        LevelData levelData = new LevelData("Test level", 20, 20);

        LevelObjects.Player player1 = new LevelObjects.Player();
        player1.posX = 3;
        player1.posY = 3;

        player1.facingDir = 0;
        player1.lastMoveDir = 3;

        player1.colorIndex = 0;
        player1.colorIndexes = new int[4] { 0, 1, 2, 3 };

        LevelObjects.Player player2 = new LevelObjects.Player();
        player2.posX = 3;
        player2.posY = 7;

        player2.facingDir = 0;
        player2.lastMoveDir = 0;

        player2.colorIndex = 1;
        player2.colorIndexes = new int[4] { 0, 1, 2, 3 };

        levelData.levelObjects.Add(player1);
        levelData.levelObjects.Add(player2);

        for (int x=0; x < 20; x++)
        {
            for (int y=0; y < 20; y++)
            {
               levelData.tileArray[x, y] = new Tiles.PlainTile();
            }
        }

        for (int x=1; x < 10; x++)
        {
            Tiles.ColorTile tile0 = new Tiles.ColorTile();
            Tiles.ColorTile tile1 = new Tiles.ColorTile();
            Tiles.ColorTile tile2 = new Tiles.ColorTile();
            Tiles.ColorTile tile3 = new Tiles.ColorTile();

            tile0.colorIndex = 0;
            tile1.colorIndex = 1;
            tile2.colorIndex = 2;
            tile3.colorIndex = 3;

            levelData.tileArray[x, 10] = tile0;
            levelData.tileArray[x, 11] = tile1;
            levelData.tileArray[x, 12] = tile2;
            levelData.tileArray[x, 13] = tile3;
        }

        levelData.tileArray[9, 3] = new Tiles.Goal();
        levelData.tileArray[9, 7] = new Tiles.Goal();

        Tiles.Switch @switch = new Tiles.Switch();
        @switch.colorIndex = 1;

        levelData.tileArray[15, 3] = @switch;

        Debug.Log("Level Saved!");

        return levelData;
    }
    
    // Save a level data object to file
    public static void SaveLevel(LevelData levelData, string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Levels", fileName);

        // Open file to be writen to
        BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create));

        // Write basic level details
        writer.Write(levelData.levelName);
        writer.Write((byte) levelData.sizeX);
        writer.Write((byte) levelData.sizeY);
        writer.Write((byte) levelData.levelObjects.Count);

        // Write level object info
        foreach (LevelObjects.BaseLevelObject levelObject in levelData.levelObjects)
        {
            writer.Write((byte)levelObject.objectID);

            levelObject.WriteData(ref writer);
        }

        // Write tile data
        for (int x = 0; x < levelData.sizeX; x++)
        {
            for (int y = 0; y < levelData.sizeY; y++)
            {
                Tiles.BaseTile tile = levelData.tileArray[x, y];

                writer.Write((byte)tile.tileID);
                tile.WriteData(ref writer);
            }
        }
    }

    // Load a level data object from file
    public static LevelData LoadLevel(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Levels", fileName);

        if (!File.Exists(path))
        {
            Debug.LogError("No level found at '" + path + "'");
            return null;
        }

        // Open file to be read
        BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));

        // Load basic level details and create a new level data object
        string levelName = reader.ReadString();
        int sizeX = reader.ReadByte();
        int sizeY = reader.ReadByte();
        int numLevelObjects = reader.ReadByte();

        LevelData levelData = new LevelData(levelName, sizeX, sizeY);

        // Load all object info
        for (int i = 0; i < numLevelObjects; i++)
        {
            int objectID = reader.ReadByte();

            LevelObjects.BaseLevelObject levelObject = Utils.IDToLevelObject(objectID);
            levelObject.ReadData(ref reader);

            levelData.levelObjects.Add(levelObject);
        }

        // Load tile info
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                int tileID = reader.ReadByte();

                Tiles.BaseTile tile = Utils.IDToTile(tileID);
                tile.ReadData(ref reader);
                
                tile.x = x;
                tile.y = y;

                levelData.tileArray[x, y] = tile;
            }
        }

        // Close file
        reader.Close();

        return levelData;
    }
}
