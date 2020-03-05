using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class PlainTile : BaseTile
    {
        // Set properties of tile
        public PlainTile() : base()
        {
            objectID = 2;
            traversable = true;
        }
    }
}