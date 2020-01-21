using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles
{
    public class Tile
    {
        public int x;
        public int y;

        public int objectID;
        public int colorIndex;

        public GameObject gameObject;

        public bool traversable;

        public void ProcessPlayer(ref Player player)
        {

        }

        public byte GetAdditionalInfo()
        {
            return 0;
        }

        public void LoadAdditionalInfo(byte additionalInfo)
        {

        }
    }

    public class ColorTile : Tile
    {
        public new bool traversable = true;
        
        bool enabledDefault;

        public new byte GetAdditionalInfo()
        {
            byte additionalInfo = enabledDefault ? (byte) 1 : (byte) 0;

            return additionalInfo;
        }

        public new void LoadAdditionalInfo(byte additionalInfo)
        {
            enabledDefault = additionalInfo == 1;
        }
    }

    public class Goal : Tile
    {
        public new bool traversable = true;

        bool goalUsed;

        public new void ProcessPlayer(ref Player player)
        {
            if (!goalUsed)
            {
                player.reachedGoal = true;
                player.gameObject.SetActive(false);

                Events.OnPlayerReachedGoal();

                goalUsed = true;
            }
        }
    }

    public class Switch : Tile
    {
        public new bool traversable = true;

        public new void ProcessPlayer(ref Player player)
        {
            ColorManager.colorCounts[colorIndex] += 1;
            ColorManager.CalculateColors();
        }
    }
}
