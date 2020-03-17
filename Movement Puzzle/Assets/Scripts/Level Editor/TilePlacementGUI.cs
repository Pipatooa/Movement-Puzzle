using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditorGUI
{
    public class TilePlacementGUI : MonoBehaviour
    {
        public GameObject guiParent;

        public TMPro.TMP_Dropdown tileSelectionDropdown;
        public Button applyToSelectionButton;

        [System.Serializable]
        public struct AdditionalOptionsMenu
        {
            public GameObject optionsParent;

            public List<TMPro.TMP_Dropdown> dropdowns;
        }

        public AdditionalOptionsMenu[] additionalOptionMenus;
        AdditionalOptionsMenu currentAdditionalOptionsMenu;

        void Awake()
        {
            LevelEditor.tilePlacementGUI = this;

            tileSelectionDropdown.onValueChanged.AddListener(value => UpdateTileOptions(value));
        }

        void Start()
        {
            // Add color names to dropdowns
            List<TMPro.TMP_Dropdown.OptionData> colorOptions = new List<TMPro.TMP_Dropdown.OptionData>();

            for (int i = 0; i < LevelInfo.colorScheme.colors.Count; i++)
            {
                TMPro.TMP_Dropdown.OptionData colorOption = new TMPro.TMP_Dropdown.OptionData();
                colorOption.text = LevelInfo.colorScheme.colors[i].name;

                colorOptions.Add(colorOption);
            }

            additionalOptionMenus[3].dropdowns[0].AddOptions(colorOptions);
        }

        // Sets whether this GUI is visible
        public void SetVisibility(bool value)
        {
            guiParent.SetActive(value);
        }

        // Applies tile settings to the current selection
        public void ApplyToSelection()
        {
            Vector2Int[] corners = Utils.GetTopLeftAndBottomRight(LevelEditor.selectionStart, LevelEditor.selectionEnd);

            // Create new tile to be coppied to selection
            int tileID = tileSelectionDropdown.value;
            Tiles.BaseTile tile = Utils.IDToTile(tileID);

            // Get additonal tile settings
            switch (tileID)
            {
                case 3:
                    Tiles.ColorTile colorTile = tile as Tiles.ColorTile;
                    colorTile.colorIndex = additionalOptionMenus[3].dropdowns[0].value;
                    tile = colorTile;

                    break;
                case 4:
                    Tiles.Switch @switch = tile as Tiles.Switch;
                    @switch.colorIndex = additionalOptionMenus[4].dropdowns[0].value;
                    tile = @switch;

                    break;
            }

            // Iterate through tiles to set properties
            for (int x = corners[0].x; x <= corners[1].x; x++)
            {
                // If not within level bounds, ignore
                if (x < 0 || x >= LevelInfo.levelData.sizeX) continue;

                for (int y = corners[0].y; y <= corners[1].y; y++)
                {
                    // If not within level bounds, ignore
                    if (y < 0 || y >= LevelInfo.levelData.sizeY) continue;

                    LevelEditor.SetTile(x, y, tile);
                }
            }
        }

        // Updates the list of tile options
        void UpdateTileOptions(int tileID)
        {
            // Close current options menu if it is currently open
            currentAdditionalOptionsMenu.optionsParent?.SetActive(false);

            // Open new options menu
            if (additionalOptionMenus[tileID].optionsParent != null)
            {
                currentAdditionalOptionsMenu = additionalOptionMenus[tileID];
                currentAdditionalOptionsMenu.optionsParent.SetActive(true);
            }
        }
    }
}