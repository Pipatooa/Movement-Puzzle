﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    // Returns a vector direction given integer direction
    public static Vector3 DirectionToVector3(int dir)
    {
        switch (dir)
        {
            case 0:
                return new Vector3(0, 0, 1);
            case 1:
                return new Vector3(1, 0, 0);
            case 2:
                return new Vector3(0, 0, -1);
            case 3:
                return new Vector3(-1, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    // Gets the bottom left and top right of a rectangle given two corners
    public static Vector2Int[] GetTopLeftAndBottomRight(Vector2Int corner1, Vector2Int corner2)
    {
        Vector2Int bottomLeft = corner1;
        Vector2Int topRight = corner2;

        // Swap coordinates to be correct
        if (topRight.x < bottomLeft.x)
        {
            int temp = topRight.x;
            topRight.x = bottomLeft.x;
            bottomLeft.x = temp;
        }

        if (topRight.y < bottomLeft.y)
        {
            int temp = topRight.y;
            topRight.y = bottomLeft.y;
            bottomLeft.y = temp;
        }

        return new Vector2Int[2] { bottomLeft, topRight };
    }

    // Returns a new tile object given an id
    public static Tiles.BaseTile IDToTile(int id)
    {
        switch (id)
        {
            case 0:
                return new Tiles.BaseTile();
            case 1:
                return new Tiles.Goal();
            case 2:
                return new Tiles.PlainTile();
            case 3:
                return new Tiles.ColorTile();
            case 4:
                return new Tiles.Switch();
            case 5:
                return new Tiles.Rotator();
            case 6:
                return new Tiles.Teleporter();
            default:
                return null;
        }
    }

    // Returns a new level object given an id
    public static LevelObjects.BaseLevelObject IDToLevelObject(int id)
    {
        switch (id)
        {
            case 0:
                return new LevelObjects.Player();
            case 1:
                return new LevelObjects.Cube();
            default:
                return null;
        }
    }
}
