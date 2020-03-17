using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditorGUI
{
    public class AddObjectGUI : MonoBehaviour
    {
        public GameObject guiParent;

        public TMPro.TMP_Dropdown objectSelectionDropdown;
        public Button addObjectButton;

        void Awake()
        {
            LevelEditor.addObjectGUI = this;
        }

        // Sets whether this GUI is visible
        public void SetVisibility(bool value)
        {
            guiParent.SetActive(value);
        }

        // Adds an object at selected location
        public void AddObject()
        {
            // Create new object
            int objectID = objectSelectionDropdown.value;
            LevelObjects.BaseLevelObject levelObject = Utils.IDToLevelObject(objectID);
            levelObject.posX = LevelEditor.selectionStart.x;
            levelObject.posY = LevelEditor.selectionStart.y;

            // Add object to level
            LevelInfo.levelData.levelObjects.Add(levelObject);
            levelObject.CreateGameObjects(LevelInfo.levelGenerator.levelObjectParent.transform);

            LevelEditor.selectedLevelObject = levelObject;

            // Disable add object button
            addObjectButton.interactable = false;

            // Open level object options menu
            LevelEditor.objectSettingsGUI.SetVisibility(true);
            LevelEditor.objectSettingsGUI.UpdateLevelObjectOptions(levelObject);
        }
    }
}
