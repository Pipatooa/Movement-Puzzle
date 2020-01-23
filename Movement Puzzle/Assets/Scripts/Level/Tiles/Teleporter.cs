using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Teleporter : Tile
    {
        public Teleporter() : base()
        {
            objectID = 6;
            traversable = true;
        }

        public new void ProcessPlayer(ref Player player)
        {

        }
    }
}