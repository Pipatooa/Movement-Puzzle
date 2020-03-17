using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditorGUI
{
    public class ObjectSettingsGUI : MonoBehaviour
    {
        public GameObject guiParent;
        
        [System.Serializable]
        public struct LevelObjectOptionMenu
        {
            public GameObject optionsParent;

            public TMPro.TMP_Dropdown[] dropdowns;
        }

        public LevelObjectOptionMenu[] levelObjectOptionMenus;
        LevelObjectOptionMenu currentLevelObjectOptionMenu;

        void Awake()
        {
            LevelEditor.objectSettingsGUI = this;
        }

        void Start()
        {
            // Add color name options to color menus
            List<TMPro.TMP_Dropdown.OptionData> colorOptions = new List<TMPro.TMP_Dropdown.OptionData>();

            for (int i = 0; i < LevelInfo.colorScheme.colors.Count; i++)
            {
                TMPro.TMP_Dropdown.OptionData colorOption = new TMPro.TMP_Dropdown.OptionData();
                colorOption.text = LevelInfo.colorScheme.colors[i].name;

                colorOptions.Add(colorOption);
            }

            levelObjectOptionMenus[0].dropdowns[0].AddOptions(colorOptions);
            levelObjectOptionMenus[0].dropdowns[1].AddOptions(colorOptions);
            levelObjectOptionMenus[0].dropdowns[2].AddOptions(colorOptions);
            levelObjectOptionMenus[0].dropdowns[3].AddOptions(colorOptions);
            levelObjectOptionMenus[0].dropdowns[4].AddOptions(colorOptions);
        }

        // Sets whether this GUI is visible
        public void SetVisibility(bool value)
        {
            guiParent.SetActive(value);
        }

        // Updates the list of 
        public void UpdateLevelObjectOptions(LevelObjects.BaseLevelObject levelObject)
        {
            // Close current options menu if it is currently open
            if (currentLevelObjectOptionMenu.optionsParent != null)
            {
                currentLevelObjectOptionMenu.optionsParent.SetActive(false);
            }

            // Load in level object values to options menu
            switch (levelObject.objectID)
            {
                case 0:
                    LevelObjects.Player player = levelObject as LevelObjects.Player;

                    levelObjectOptionMenus[0].dropdowns[0].value = player.colorIndex;
                    levelObjectOptionMenus[0].dropdowns[1].value = player.colorIndexes[0] + 1;
                    levelObjectOptionMenus[0].dropdowns[2].value = player.colorIndexes[1] + 1;
                    levelObjectOptionMenus[0].dropdowns[3].value = player.colorIndexes[2] + 1;
                    levelObjectOptionMenus[0].dropdowns[4].value = player.colorIndexes[3] + 1;
                    levelObjectOptionMenus[0].dropdowns[5].value = player.facingDir;
                    levelObjectOptionMenus[0].dropdowns[6].value = player.lastMoveDir;
                    break;
            }

            // Open new options menu
            if (levelObjectOptionMenus[levelObject.objectID].optionsParent != null)
            {
                currentLevelObjectOptionMenu = levelObjectOptionMenus[levelObject.objectID];
                currentLevelObjectOptionMenu.optionsParent.SetActive(true);
            }
        }

        // Applies new settings to the currently selected level object
        public void ApplyLevelObjectSettings()
        {
            // Destory current level object
            LevelEditor.selectedLevelObject.DestroyGameObjects();
            LevelInfo.levelData.levelObjects.Remove(LevelEditor.selectedLevelObject);

            // Create a new level object with new properties
            int objectID = LevelEditor.selectedLevelObject.objectID;
            LevelObjects.BaseLevelObject levelObject = Utils.IDToLevelObject(objectID);
            levelObject.posX = LevelEditor.selectionStart.x;
            levelObject.posY = LevelEditor.selectionStart.y;

            switch (objectID)
            {
                case 0:
                    LevelObjects.Player player = levelObject as LevelObjects.Player;

                    player.colorIndex = levelObjectOptionMenus[0].dropdowns[0].value;
                    player.colorIndexes[0] = levelObjectOptionMenus[0].dropdowns[1].value - 1;
                    player.colorIndexes[1] = levelObjectOptionMenus[0].dropdowns[2].value - 1;
                    player.colorIndexes[2] = levelObjectOptionMenus[0].dropdowns[3].value - 1;
                    player.colorIndexes[3] = levelObjectOptionMenus[0].dropdowns[4].value - 1;
                    player.facingDir = levelObjectOptionMenus[0].dropdowns[5].value;
                    player.lastMoveDir = levelObjectOptionMenus[0].dropdowns[6].value;

                    levelObject = player;

                    break;
            }

            // Re-Add object to level
            LevelInfo.levelData.levelObjects.Add(levelObject);
            levelObject.CreateGameObjects(LevelInfo.levelGenerator.levelObjectParent.transform);

            LevelEditor.selectedLevelObject = levelObject;
        }

        // Deletes selected level object
        public void DeleteLevelObject()
        {
            // Remove game objects for selected level object
            LevelEditor.selectedLevelObject.DestroyGameObjects();

            // Remove level object from lists of level objects
            LevelInfo.levelData.levelObjects.Remove(LevelEditor.selectedLevelObject);

            LevelEditor.selectedLevelObject = null;

            // Enable add object button
            LevelEditor.addObjectGUI.addObjectButton.interactable = true;

            // Close this menu
            SetVisibility(false);
        }

    }
}