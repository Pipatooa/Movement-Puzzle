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

        public bool fallenInVoid;

        public GameObject gameObject;
        public Rigidbody rb;

        public BaseLevelObject()
        {
            Events.LevelInit += LevelInit;
            Events.OnLevelUpdate += OnLevelUpdate;
        }

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
            LevelInfo.levelObjects.Add(this);
        }

        // Destorys all game objects for this level object
        public virtual void DestroyGameObjects()
        {
            GameObject.Destroy(gameObject);
        }

        // Nudge the object in dir
        // Returns true if successful, otherwise, false
        public virtual bool Shift(int absDir)
        {
            // Calculate the position the player will end up in
            Vector3 vector = Utils.DirectionToVector3(absDir);

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
            SaveObjectState();
            
            var thisObject = this;

            LevelInfo.levelData.tileArray[posX, posY].ProcessObjectExit(ref thisObject);

            posX = newPosX;
            posY = newPosY;

            gameObject.transform.position += vector;

            LevelInfo.levelData.tileArray[posX, posY].ProcessObjectEntry(ref thisObject);

            return true;
        }

        // Called before first level update
        void LevelInit()
        {
            var thisObject = this;
            LevelInfo.levelData.tileArray[posX, posY].ProcessObjectEntry(ref thisObject);
        }

        // Called after the level has updated
        void OnLevelUpdate()
        {
            // Check if object should fall into the void
            if (!LevelInfo.levelData.tileArray[posX, posY].traversable)
            {
                Kill();
            }
        }

        // Makes the level object fall into the void
        public virtual void Kill()
        {
            SaveObjectState();
            
            fallenInVoid = true;
            rb.useGravity = true;
        }

        // Information about a change that has occured to this object
        protected class LevelObjectChange : UndoSystem.Change
        {
            // Info about change
            protected BaseLevelObject levelObject;

            public int oldPosX;
            public int oldPosY;

            public bool oldFallenInVoidStatus;

            public LevelObjectChange(BaseLevelObject levelObject)
            {
                this.levelObject = levelObject;

                oldPosX = levelObject.posX;
                oldPosY = levelObject.posY;

                oldFallenInVoidStatus = levelObject.fallenInVoid;
            }

            public override void UndoChange()
            {
                levelObject.UndoObjectChange(this);
            }
        }

        // Undo a change that has occured to the player
        protected virtual void UndoObjectChange(LevelObjectChange levelObjectChange)
        {
            // Load properties
            posX = levelObjectChange.oldPosX;
            posY = levelObjectChange.oldPosY;

            fallenInVoid = levelObjectChange.oldFallenInVoidStatus;

            // Update game objects - Do not update if the object has fallen into the void
            if (!fallenInVoid)
            {
                gameObject.transform.position = new Vector3(posX, 0.5f, posY);

                rb.useGravity = false;
                rb.velocity = Vector3.zero;
            }
        }

        // Saves an old state of this object as a change
        public virtual void SaveObjectState()
        {
            LevelObjectChange levelObjectChange = new LevelObjectChange(this);
            UndoSystem.AddChange(levelObjectChange);
        }
    }
}