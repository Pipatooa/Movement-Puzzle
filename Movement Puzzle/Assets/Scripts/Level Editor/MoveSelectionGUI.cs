using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditorGUI
{
    public class MoveSelectionGUI : MonoBehaviour
    {
        public Button upButton;
        public Button rightButton;
        public Button downButton;
        public Button leftButton;
        
        void Awake()
        {
            LevelEditor.moveSelectionGUI = this;

            upButton.onClick.AddListener(delegate { MoveSelectedTiles(0, 1); });
            rightButton.onClick.AddListener(delegate { MoveSelectedTiles(1, 0); });
            downButton.onClick.AddListener(delegate { MoveSelectedTiles(0, -1); });
            leftButton.onClick.AddListener(delegate { MoveSelectedTiles(-1, 0); });
        }

        // Sets all buttons interactibility
        public void SetButtonsInteractable(bool value)
        {
            upButton.interactable = value;
            rightButton.interactable = value;
            downButton.interactable = value;
            leftButton.interactable = value;
        }

        // Moves tiles and objects selected by amount specified
        public void MoveSelectedTiles(int translationX, int translationY)
        {
            // Get selection size and corners
            Vector2Int selection = LevelEditor.selectionEnd - LevelEditor.selectionStart;
            Vector2Int[] corners = Utils.GetTopLeftAndBottomRight(LevelEditor.selectionStart, LevelEditor.selectionEnd);
            Vector2Int[] destinationCorners = new Vector2Int[2] { corners[0] + new Vector2Int(translationX, translationY), corners[1] + new Vector2Int(translationX, translationY) };

            // Update selection
            LevelEditor.selectionStart += new Vector2Int(translationX, translationY);
            LevelEditor.selectionEnd += new Vector2Int(translationX, translationY);
            LevelEditor.selectionGUI.UpdateSelectionBox();
        }
    }
}