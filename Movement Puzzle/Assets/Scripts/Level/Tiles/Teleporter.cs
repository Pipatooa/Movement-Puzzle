﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Tiles
{
    public class Teleporter : BaseTile
    {
        // Set properties of tile
        public Teleporter() : base()
        {
            tileID = 6;
            traversable = true;
        }

        // Processes an object that has landed on this tile
        public override void ProcessObjectEntry(ref LevelObjects.BaseLevelObject moveableObject)
        {

        }
    }
}