using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Switch : Tile
    {
        public Switch() : base()
        {
            objectID = 4;
            traversable = true;
        }

        public new void ProcessPlayer(ref Player player)
        {
            ColorManager.colorCounts[colorIndex] += 1;
            ColorManager.CalculateColors();
        }
    }
}