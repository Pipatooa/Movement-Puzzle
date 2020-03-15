using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Tiles
{
    public class BaseTile
    {
        public int tileID;
        public bool traversable;
        
        public int x, y;

        GameObject gameObject;

        // Writes additional data about the object given a binary reader
        public virtual void WriteData(ref BinaryWriter writer)
        {
            
        }

        // Reads additional data about the object given a binary reader
        public virtual void ReadData(ref BinaryReader reader)
        {
            
        }

        // Creates all game objects for level object under parent transform
        public virtual void CreateGameObjects(Transform parentTransform)
        {

        }

        // Creates level editor game objects for level object under parent transform
        public virtual void LevelEditorCreateGameObjects(Transform parentTransform)
        {
            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), parentTransform);
            gameObject.transform.localScale *= LevelInfo.levelGenerator.tileSizeSmall;
            gameObject.isStatic = true;
        }

        // Destorys all game objects for this tile
        public virtual void DestroyGameObjects()
        {
            GameObject.Destroy(gameObject);
        }

        // Processes an object that has landed on this tile
        public virtual void ProcessObjectEntry(ref LevelObjects.BaseLevelObject moveableObject)
        {

        }

        // Processes an object that is exiting this tile
        public virtual void ProcessObjectExit(ref LevelObjects.BaseLevelObject moveableObject)
        {

        }

        // Returns a new tile of this type with same properties
        public virtual BaseTile CreateCopy()
        {
            BaseTile tile = new BaseTile();

            return tile;
        }
    }
}
