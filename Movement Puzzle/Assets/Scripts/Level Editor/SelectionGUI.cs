using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LevelEditorGUI
{
    public class SelectionGUI : MonoBehaviour
    {
        public GameObject guiParent;
        
        public GameObject selectionBox;
        public TMPro.TextMeshProUGUI selectionText;

        bool validSelection;

        void Awake()
        {
            LevelEditor.selectionGUI = this;
        }

        // Sets whether this GUI is visible
        public void SetVisibility(bool value)
        {
            guiParent.SetActive(value);
        }

        void Update()
        {
            // Selection start
            if (Input.GetMouseButtonDown(0))
            {
                // If cursor is over GUI element, don't make a tile selection
                validSelection = !EventSystem.current.IsPointerOverGameObject();

                // Record location of start of selection
                if (validSelection)
                {
                    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    int x = Mathf.RoundToInt(worldPoint.x);
                    int y = Mathf.RoundToInt(worldPoint.z);
                    LevelEditor.selectionStart = new Vector2Int(x, y);

                    SetVisibility(true);

                    LevelEditor.tilePlacementGUI.applyToSelectionButton.interactable = false;
                    LevelEditor.addObjectGUI.enabled = false;
                    LevelEditor.addObjectGUI.addObjectButton.interactable = false;
                    LevelEditor.objectSettingsGUI.enabled = false;
                }
            }

            // Selection current and end
            if (Input.GetMouseButton(0) && validSelection)
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                int x = Mathf.RoundToInt(worldPoint.x);
                int y = Mathf.RoundToInt(worldPoint.z);
                LevelEditor.selectionEnd = new Vector2Int(x, y);

                Vector2 midPoint = (Vector2)(LevelEditor.selectionStart + LevelEditor.selectionEnd) / 2;
                Vector2 selection = LevelEditor.selectionEnd - LevelEditor.selectionStart;

                selectionBox.transform.position = new Vector3(midPoint.x, 1, midPoint.y);
                selectionBox.transform.localScale = new Vector3(Mathf.Abs(selection.x) + 1, Mathf.Abs(selection.y) + 1, 1);

                Vector2Int[] corners = Utils.GetTopLeftAndBottomRight(LevelEditor.selectionStart, LevelEditor.selectionEnd);

                selectionText.text = corners[0].x + "," + corners[0].y + " - " + corners[1].x + "," + corners[1].y;
            }

            // Selection complete
            if (Input.GetMouseButtonUp(0) && validSelection)
            {
                Vector2Int selection = LevelEditor.selectionEnd - LevelEditor.selectionStart;

                // Check whether an object can be added in this position
                bool canAddObject = selection.x == 0 && selection.y == 0
                    && LevelEditor.selectionStart.x >= 0 && LevelEditor.selectionStart.x < LevelInfo.levelData.sizeX
                    && LevelEditor.selectionStart.y >= 0 && LevelEditor.selectionStart.y < LevelInfo.levelData.sizeY;

                LevelEditor.selectedLevelObject = null;

                if (canAddObject)
                {
                    foreach (LevelObjects.BaseLevelObject levelObject in LevelInfo.levelData.levelObjects)
                    {
                        if (LevelEditor.selectionStart.x == levelObject.posX && LevelEditor.selectionStart.y == levelObject.posY)
                        {
                            canAddObject = false;

                            LevelEditor.selectedLevelObject = levelObject;
                            LevelEditor.objectSettingsGUI.UpdateLevelObjectOptions(levelObject);

                            break;
                        }
                    }
                }

                LevelEditor.tilePlacementGUI.applyToSelectionButton.interactable = true;
                LevelEditor.addObjectGUI.addObjectButton.interactable = canAddObject;
                LevelEditor.objectSettingsGUI.SetVisibility(LevelEditor.selectedLevelObject != null);
            }

            // Reset Selection
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResetSelection();
            }
        }

        // Resets selection
        public void ResetSelection()
        {
            LevelEditor.selectionStart = -Vector2Int.one;
            LevelEditor.selectionEnd = -Vector2Int.one;

            selectionBox.transform.localScale = Vector3.zero;

            SetVisibility(false);

            LevelEditor.tilePlacementGUI.applyToSelectionButton.interactable = false;
            LevelEditor.addObjectGUI.addObjectButton.interactable = false;
        }
    }
}