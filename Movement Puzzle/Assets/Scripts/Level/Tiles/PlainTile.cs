using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class PlainTile : BaseTile
    {
        GameObject gameObject;
        
        // Set properties of tile
        public PlainTile() : base()
        {
            objectID = 2;
            traversable = true;
        }

        // Creates all objects for tile
        public override void CreateGameObjects(Transform parentTransform)
        {
            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), parentTransform);
            gameObject.transform.localScale *= LevelInfo.levelGenerator.tileSize;
            gameObject.isStatic = true;
        }
    }
}