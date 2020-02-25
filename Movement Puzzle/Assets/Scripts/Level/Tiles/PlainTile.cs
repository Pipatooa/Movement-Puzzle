using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class PlainTile : BaseTile
    {
        public PlainTile() : base()
        {
            objectID = 2;
            traversable = true;
        }
    }
}