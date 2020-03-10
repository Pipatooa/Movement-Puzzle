using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Tiles
{
    public class Switch : BaseTile
    {
        public int colorIndex;
        bool active;

        GameObject gameObject;

        // Set properties of tile
        public Switch() : base()
        {
            tileID = 4;
            traversable = true;
        }

        // Writes additional data about the object given a binary reader
        public override void WriteData(ref BinaryWriter writer)
        {
            writer.Write((byte)colorIndex);
        }

        // Reads additional data about the object given a binary reader
        public override void ReadData(ref BinaryReader reader)
        {
            colorIndex = reader.ReadByte();
        }

        // Creates all objects for tile
        public override void CreateGameObjects(Transform parentTransform)
        {
            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), parentTransform);
            gameObject.transform.localScale *= LevelInfo.levelGenerator.tileSize;
            gameObject.GetComponent<Renderer>().material = LevelInfo.tileMaterials[colorIndex];
        }

        // Processes an object that has landed on this tile
        public override void ProcessObjectEntry(ref LevelObjects.BaseLevelObject moveableObject)
        {
            SaveSwitchState(SwitchChange.Event.entry);
            ColorManager.colorCounts[colorIndex] += 1;
        }

        // Processes an object that is exiting this tile
        public override void ProcessObjectExit(ref LevelObjects.BaseLevelObject moveableObject)
        {
            SaveSwitchState(SwitchChange.Event.exit);
            ColorManager.colorCounts[colorIndex] -= 1;
        }

        // Information about a change that has occured to the switch
        class SwitchChange : UndoSystem.Change
        {
            // Info about change
            Switch @switch;

            public enum Event { entry, exit }
            public Event @event;

            public SwitchChange(Switch @switch, Event @event)
            {
                this.@switch = @switch;
                this.@event = @event;
            }

            public override void UndoChange()
            {
                @switch.UndoSwitchChange(this);
            }
        }

        // Undo a change that has occured to the switch
        void UndoSwitchChange(SwitchChange switchChange)
        {
            if (switchChange.@event == SwitchChange.Event.entry)
            {
                ColorManager.colorCounts[colorIndex] -= 1;
            } else
            {
                ColorManager.colorCounts[colorIndex] += 1;
            }
        }

        // Saves an old state of the switch as a change
        void SaveSwitchState(SwitchChange.Event @event)
        {
            SwitchChange goalChange = new SwitchChange(this, @event);
            UndoSystem.AddChange(goalChange);
        }
    }
}
