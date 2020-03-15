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

        GameObject switchParent;
        GameObject gameObject;
        
        GameObject textObject;
        TMPro.TextMeshPro textMesh;

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
            switchParent = new GameObject("Switch");
            switchParent.transform.SetParent(parentTransform);
            switchParent.transform.position = new Vector3(x, 2.5f, y);

            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), switchParent.transform);
            gameObject.transform.localScale *= LevelInfo.levelGenerator.tileSize;
            gameObject.GetComponent<Renderer>().material = LevelInfo.tileMaterials[colorIndex];

            textObject = new GameObject("Text");
            textObject.transform.SetParent(switchParent.transform);
            textObject.transform.localPosition = new Vector3(0, 0.5f, 0);
            textObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            textMesh = textObject.AddComponent<TMPro.TextMeshPro>();
            textMesh.transform.position = new Vector3(x, 0.5f, y);
            textMesh.text = "S";
            textMesh.fontSize = 7;
            textMesh.alignment = TMPro.TextAlignmentOptions.Center;
        }

        // Creates level editor game objects for level object under parent transform
        public override void LevelEditorCreateGameObjects(Transform parentTransform)
        {
            switchParent = new GameObject("Switch");
            switchParent.transform.SetParent(parentTransform);
            switchParent.transform.position = new Vector3(x, 2.5f, y);

            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), switchParent.transform);
            gameObject.transform.localScale *= LevelInfo.levelGenerator.tileSize;
            gameObject.GetComponent<Renderer>().material = LevelInfo.tileMaterials[colorIndex];

            textObject = new GameObject("Text");
            textObject.transform.SetParent(switchParent.transform);
            textObject.transform.localPosition = new Vector3(0, 0.5f, 0);
            textObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            textMesh = textObject.AddComponent<TMPro.TextMeshPro>();
            textMesh.transform.position = new Vector3(x, 0.5f, y);
            textMesh.text = "S";
            textMesh.fontSize = 7;
            textMesh.alignment = TMPro.TextAlignmentOptions.Center;
        }

        // Destorys all game objects for this tile
        public override void DestroyGameObjects()
        {
            GameObject.Destroy(switchParent);
            GameObject.Destroy(gameObject);
            GameObject.Destroy(textObject);
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

        // Returns a new tile of this type with same properties
        public override BaseTile CreateCopy()
        {
            Switch tile = new Switch();
            tile.colorIndex = colorIndex;

            return tile;
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
