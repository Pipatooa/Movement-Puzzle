using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Tiles
{
    public class PlainTile : BaseTile
    {
        GameObject gameObject;
        
        // Set properties of tile
        public PlainTile() : base()
        {
            tileID = 2;
            traversable = true;
        }

        // Creates all objects for tile
        public override void CreateGameObjects(Transform parentTransform)
        {
            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), parentTransform);
            gameObject.transform.localScale *= LevelInfo.levelGenerator.tileSize;
            gameObject.isStatic = true;
        }

        // Creates level editor game objects for level object under parent transform
        public override void LevelEditorCreateGameObjects(Transform parentTransform)
        {
            CreateGameObjects(parentTransform);
        }

        // Destorys all game objects for this tile
        public override void DestroyGameObjects()
        {
            GameObject.Destroy(gameObject);
        }

        // Returns a new tile of this type with same properties
        public override BaseTile CreateCopy()
        {
            PlainTile tile = new PlainTile();

            return tile;
        }
    }
}