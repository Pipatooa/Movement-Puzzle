using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class TraversableTile : Tile
    {
        public TraversableTile() : base()
        {
            objectID = 2;
            traversable = true;
        }
    }
}