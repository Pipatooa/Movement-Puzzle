using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Rotator : BaseTile
    {
        // Set properties of tile
        public Rotator() : base()
        {
            tileID = 5;
            traversable = true;
        }

        // Processes an object that has landed on this tile
        public override void ProcessObjectEntry(ref ILevelObject moveableObject)
        {
            
        }
    }
}