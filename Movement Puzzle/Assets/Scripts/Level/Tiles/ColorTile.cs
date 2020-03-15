using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Tiles
{
    public class ColorTile : BaseTile
    {
        public int colorIndex;
        public bool enabledDefault;

        GameObject gameObject;

        // Set properties of tile
        public ColorTile() : base()
        {
            tileID = 3;
            traversable = true;

            Events.LevelUpdate += LevelUpdate;
        }

        // Writes additional data about the object given a binary reader
        public override void WriteData(ref BinaryWriter writer)
        {
            writer.Write((byte)colorIndex);

            writer.Write(enabledDefault);
        }

        // Reads additional data about the object given a binary reader
        public override void ReadData(ref BinaryReader reader)
        {
            colorIndex = reader.ReadByte();

            enabledDefault = reader.ReadBoolean();
        }

        // Creates all objects for tile
        public override void CreateGameObjects(Transform parentTransform)
        {
            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), parentTransform);
            gameObject.transform.localScale *= LevelInfo.levelGenerator.tileSize;
            gameObject.GetComponent<Renderer>().material = LevelInfo.tileMaterials[colorIndex];
        }

        // Creates level editor game objects for level object under parent transform
        public override void LevelEditorCreateGameObjects(Transform parentTransform)
        {
            CreateGameObjects(parentTransform);
        }

        // Destorys all game objects for this tile
        public override void DestroyGameObjects()
        {
            GameObject.Destroy(gameObject);
        }

        // Returns a new tile of this type with same properties
        public override BaseTile CreateCopy()
        {
            ColorTile tile = new ColorTile();
            tile.colorIndex = colorIndex;
            tile.enabledDefault = enabledDefault;

            return tile;
        }

        // Called when the level should be updated
        void LevelUpdate()
        {
            if (ColorManager.colorStates[colorIndex])
            {
                gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSize;
                traversable = true;
            } else
            {
                gameObject.transform.localScale = Vector3.one * LevelInfo.levelGenerator.tileSizeSmall;
                traversable = false;
            }
        }
    }
}