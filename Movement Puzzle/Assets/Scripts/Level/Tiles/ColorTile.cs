using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class ColorTile : BaseTile
    {
        GameObject gameObject;
        bool enabledDefault;

        // Set properties of tile
        public ColorTile() : base()
        {
            objectID = 3;
            traversable = true;

            Events.LevelUpdate += LevelUpdate;
        }

        ~ColorTile()
        {
            Events.LevelUpdate -= LevelUpdate;
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
        }

        // Called when the level should be updated
        void LevelUpdate()
        {
            if (ColorManager.colorStates[colorIndex])
            {
                gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSize;
                traversable = true;
            } else
            {
                gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSizeSmall;
                traversable = false;
            }
        }
    }
}