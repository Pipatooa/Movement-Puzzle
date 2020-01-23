using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class PlainTile : Tile
    {
        public PlainTile() : base()
        {
            objectID = 2;
            traversable = true;
        }
    }
}