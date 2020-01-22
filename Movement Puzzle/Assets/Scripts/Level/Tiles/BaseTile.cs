using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Tile
    {
        public int objectID;

        public int x, y;

        public bool traversable;
        public int colorIndex;

        public GameObject gameObject;

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
}