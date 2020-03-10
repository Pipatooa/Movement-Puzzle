using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelObjects
{
    public class BaseLevelObject : MonoBehaviour
    {
        public int objectID;

        public int posX, posY;

        // Nudge the object in dir
        // Returns true if successful, otherwise, false
        public virtual bool Shift(int absDir)
        {
            // Calculate the position the player will end up in
            Vector3 vector = Utils.DirectionToVector(absDir);

            int newPosX = Mathf.RoundToInt(posX + vector.x);
            int newPosY = Mathf.RoundToInt(posY + vector.z);

            // Check whether new position is within level bounds
            if (newPosX < 0 || newPosX >= LevelInfo.levelData.tileArray.GetLength(0) || newPosY < 0 || newPosY >= LevelInfo.levelData.tileArray.GetLength(1))
            {
                return false;
            }

            // Check whether a level object to be nudged is present in new position
            LevelObjects.BaseLevelObject nudgedObject = null;
            foreach (LevelObjects.BaseLevelObject levelObject in LevelInfo.levelObjects)
            {
                if (levelObject.posX == newPosX && levelObject.posY == newPosY)
                {
                    // Check that the object found is not a player that has reached a goal
                    if (levelObject is Player)
                    {
                        Player player = levelObject as Player;

                        if (player.reachedGoal)
                        {
                            continue;
                        }
                    }
                    
                    nudgedObject = levelObject;
                    break;
                }
            }

            // If object to nudge, and object did not move when nudged, don't move this object and return false
            if (nudgedObject != null) if (!nudgedObject.Shift(absDir)) return false;

            // Otherwise, move this object
            var thisObject = this;

            LevelInfo.levelData.tileArray[posX, posY].ProcessObjectExit(ref thisObject);

            posX = newPosX;
            posY = newPosY;

            gameObject.transform.position += vector;

            LevelInfo.levelData.tileArray[posX, posY].ProcessObjectEntry(ref thisObject);

            return true;
        }
    }
}