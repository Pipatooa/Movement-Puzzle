using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Switch : BaseTile
    {
        GameObject gameObject;

        // Set properties of tile
        public Switch() : base()
        {
            objectID = 4;
            traversable = true;
        }

        // Creates all objects for tile
        public override void CreateGameObjects(Transform parentTransform)
        {
            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), parentTransform);
            gameObject.transform.localScale *= 0.9f;
            gameObject.GetComponent<Renderer>().material = LevelInfo.tileMaterials[colorIndex];
        }

        // Processes a player that has landed on this tile
        public new void ProcessPlayer(ref Player player)
        {
            ColorManager.colorCounts[colorIndex] += 1;
            ColorManager.CalculateColors();
        }
    }
}