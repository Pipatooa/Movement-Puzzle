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

        public GameObject gameObject;

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

        // Processes a player that has landed on this tile
        public virtual void ProcessPlayer(ref Player player)
        {
            
        }
    }
}