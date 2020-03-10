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

        // Processes an object that has landed on this tile
        public virtual void ProcessObjectEntry(ref LevelObjects.BaseLevelObject moveableObject)
        {

        }

        // Processes an object that is exiting this tile
        public virtual void ProcessObjectExit(ref LevelObjects.BaseLevelObject moveableObject)
        {

        }
    }
}
