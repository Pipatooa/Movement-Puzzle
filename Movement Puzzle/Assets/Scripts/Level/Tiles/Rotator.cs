using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Rotator : BaseTile
    {
        public Rotator() : base()
        {
            objectID = 5;
            traversable = true;
        }

        public new void ProcessPlayer(ref Player player)
        {
            
        }
    }
}