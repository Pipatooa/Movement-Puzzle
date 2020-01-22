using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class ColorTile : Tile
    {
        bool enabledDefault;

        public ColorTile() : base()
        {
            objectID = 3;
            traversable = true;
        }

        public new byte GetAdditionalInfo()
        {
            byte additionalInfo = enabledDefault ? (byte)1 : (byte)0;

            return additionalInfo;
        }

        public new void LoadAdditionalInfo(byte additionalInfo)
        {
            enabledDefault = additionalInfo == 1;
        }
    }
}