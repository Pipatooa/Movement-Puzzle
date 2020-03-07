using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class ColorTile : BaseTile
    {
        bool enabledDefault;

        // Set properties of tile
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

        // Creates all objects for tile
        public override void CreateGameObjects(Transform parentTransform)
        {
            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), parentTransform);
            gameObject.transform.localScale *= 0.9f;
            gameObject.GetComponent<Renderer>().material = LevelInfo.tileMaterials[colorIndex];

            LevelInfo.tileManager.colorGroups[colorIndex].Add(this);
        }
    }
}