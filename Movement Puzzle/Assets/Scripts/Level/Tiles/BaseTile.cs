using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class BaseTile
    {
        public int objectID;

        public int x, y;

        public bool traversable;
        public int colorIndex;

        public byte GetAdditionalInfo()
        {
            return 0;
        }

        public void LoadAdditionalInfo(byte additionalInfo)
        {

        }

        // Creates all objects for tile under parent transform
        public virtual void CreateGameObjects(Transform parentTransform)
        {

        }

        // Processes an object that has landed on this tile
        public virtual void ProcessObjectEntry(ref IMoveableObject moveableObject)
        {
            
        }

        // Processes an object that is exiting this tile
        public virtual void ProcessObjectExit(ref IMoveableObject moveableObject)
        {

        }
    }
}
