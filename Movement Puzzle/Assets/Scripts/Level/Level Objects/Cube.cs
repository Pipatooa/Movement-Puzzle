using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelObjects {
    public class Cube : BaseLevelObject
    {
        // Set properties of object
        public Cube() : base()
        {
            objectID = 1;
        }

        // Creates all game objects for level object under parent transform
        public override void CreateGameObjects(Transform parentTransform)
        {
            base.CreateGameObjects(parentTransform);

            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.cube, new Vector3(posX, 0.5f, posY), Quaternion.identity, parentTransform);
        }
    }
}
