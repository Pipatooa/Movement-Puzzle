using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LevelObjects
{
    public class BaseLevelObject
    {
        public int objectID;
        public int posX, posY;

        public GameObject gameObject;

        // Writes additional data about the object given a binary reader
        public virtual void WriteData(ref BinaryWriter writer)
        {
            writer.Write((byte)posX);
            writer.Write((byte)posY);
        }

        // Reads additional data about the object given a binary reader
        public virtual void ReadData(ref BinaryReader reader)
        {
            posX = reader.ReadByte();
            posY = reader.ReadByte();
        }

        // Creates all game objects for level object under parent transform
        public virtual void CreateGameObjects(Transform parentTransform)
        {

        }

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
            BaseLevelObject nudgedObject = null;
            foreach (BaseLevelObject levelObject in LevelInfo.levelObjects)
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