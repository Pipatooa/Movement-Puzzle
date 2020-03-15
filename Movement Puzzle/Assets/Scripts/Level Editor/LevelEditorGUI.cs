using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class LevelEditorGUI : MonoBehaviour
{
    public TMPro.TMP_InputField levelNameInputField;
    public TMPro.TMP_Dropdown tileSelectionDropdown;
    public TMPro.TMP_Dropdown levelObjectSelectionDropdown;

    [System.Serializable]
    public struct TileOptionMenu
    {
        public GameObject optionsParent;

        public TMPro.TMP_Dropdown dropdown1;
    }

    [System.Serializable]
    public struct LevelObjectOptionMenu
    {
        public GameObject optionsParent;

        public TMPro.TMP_Dropdown[] dropdowns;
    }

    public GameObject selectionInfoPanel;
    public TMPro.TextMeshProUGUI selectionInfoText;

    public Button applyToSelectionButton;
    public Button addObjectButton;

    public TileOptionMenu[] tileOptionMenus;
    TileOptionMenu currentDropDown;

    public GameObject levelObjectOptionsPanel;
    public LevelObjectOptionMenu[] levelObjectOptionMenus;
    LevelObjectOptionMenu currentLevelObjectOptionMenu;

    public GameObject selectionBox;
    bool validSelection;
    Vector2Int selectionStart = -Vector2Int.one;
    Vector2Int selectionEnd = -Vector2Int.one;
    LevelObjects.BaseLevelObject selectedLevelObject;

    void Awake()
    {
        LevelEditor.levelEditorGUI = this;
        
        levelNameInputField.onEndEdit.AddListener(value => LevelEditor.UpdateLevelName(value));
        tileSelectionDropdown.onValueChanged.AddListener(value => UpdateTileOptions(value));
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

        tileOptionMenus[3].dropdown1.AddOptions(colorOptions);

        levelObjectOptionMenus[0].dropdowns[0].AddOptions(colorOptions);
        levelObjectOptionMenus[0].dropdowns[1].AddOptions(colorOptions);
        levelObjectOptionMenus[0].dropdowns[2].AddOptions(colorOptions);
        levelObjectOptionMenus[0].dropdowns[3].AddOptions(colorOptions);
        levelObjectOptionMenus[0].dropdowns[4].AddOptions(colorOptions);
    }

    // New level button
    public void NewLevel()
    {
        LevelEditor.CreateNewLevel();
        LevelEditor.ReloadLevel();

        ResetSelection();
    }

    // Load level button
    public void LoadLevel()
    {
        string path = EditorUtility.OpenFilePanel("Open Level", "", "level");

        // If no file selected, ignore
        if (path.Length == 0) return;

        LevelEditor.LoadLevel(path);

        ResetSelection();
    }

    // Save level button
    public void SaveLevel()
    {
        string path = EditorUtility.SaveFilePanel("Save Level", "", LevelInfo.currentLevelName + ".level", "level");
        LevelEditor.filePath = path;

        LoadSystem.SaveLevel(LevelInfo.levelData, path);
    }

    // Updates the list of tile options
    void UpdateTileOptions(int tileID)
    {
        // Close current options menu if it is currently open
        if (currentDropDown.optionsParent != null)
        {
            currentDropDown.optionsParent.SetActive(false);
        }
        
        // Open new options menu
        if (tileOptionMenus[tileID].optionsParent != null)
        {
            currentDropDown = tileOptionMenus[tileID];
            currentDropDown.optionsParent.SetActive(true);
        }
    }

    // Applies tile settings to the current selection
    public void ApplyToSelection()
    {
        Vector2Int[] corners = Utils.GetTopLeftAndBottomRight(selectionStart, selectionEnd);

        // Create new tile to be coppied to selection
        int tileID = tileSelectionDropdown.value;
        Tiles.BaseTile tile = Utils.IDToTile(tileID);

        // Get additonal tile settings
        switch (tileID)
        {
            case 3:
                Tiles.ColorTile colorTile = tile as Tiles.ColorTile;
                colorTile.colorIndex = tileOptionMenus[3].dropdown1.value;
                tile = colorTile;

                break;
            case 4:
                Tiles.Switch @switch = tile as Tiles.Switch;
                @switch.colorIndex = tileOptionMenus[4].dropdown1.value;
                tile = @switch;

                break;
        }

        // Iterate through tiles to set properties
        for (int x=corners[0].x; x <= corners[1].x; x++)
        {
            // If not within level bounds, ignore
            if (x < 0 || x >= LevelInfo.levelData.sizeX)
            {
                continue;
            }
            
            for (int y=corners[0].y; y <= corners[1].y; y++)
            {
                // If not within level bounds, ignore
                if (y < 0 || y >= LevelInfo.levelData.sizeY)
                {
                    continue;
                }

                LevelEditor.SetTile(x, y, tile);
            }
        }
    }

    // Updates the list of 
    void UpdateLevelObjectOptions(LevelObjects.BaseLevelObject levelObject)
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
                levelObjectOptionMenus[0].dropdowns[1].value = player.colorIndexes[0];
                levelObjectOptionMenus[0].dropdowns[2].value = player.colorIndexes[1];
                levelObjectOptionMenus[0].dropdowns[3].value = player.colorIndexes[2];
                levelObjectOptionMenus[0].dropdowns[4].value = player.colorIndexes[3];
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
        selectedLevelObject.DestroyGameObjects();
        LevelInfo.levelObjects.Remove(selectedLevelObject);
        LevelInfo.levelData.levelObjects.Remove(selectedLevelObject);

        // Create a new level object with new properties
        int objectID = selectedLevelObject.objectID;
        LevelObjects.BaseLevelObject levelObject = Utils.IDToLevelObject(objectID);
        levelObject.posX = selectionStart.x;
        levelObject.posY = selectionStart.y;

        switch (objectID)
        {
            case 0:
                LevelObjects.Player player = levelObject as LevelObjects.Player;

                player.colorIndex = levelObjectOptionMenus[0].dropdowns[0].value;
                player.colorIndexes[0] = levelObjectOptionMenus[0].dropdowns[1].value;
                player.colorIndexes[1] = levelObjectOptionMenus[0].dropdowns[2].value;
                player.colorIndexes[2] = levelObjectOptionMenus[0].dropdowns[3].value;
                player.colorIndexes[3] = levelObjectOptionMenus[0].dropdowns[4].value;
                player.facingDir = levelObjectOptionMenus[0].dropdowns[5].value;
                player.lastMoveDir = levelObjectOptionMenus[0].dropdowns[6].value;

                levelObject = player;

                break;
        }

        // Re-Add object to level
        LevelInfo.levelData.levelObjects.Add(levelObject);
        levelObject.CreateGameObjects(LevelInfo.levelGenerator.levelObjectParent.transform);

        selectedLevelObject = levelObject;
    }

    // Deletes selected level object
    public void DeleteLevelObject()
    {
        // Remove game objects for selected level object
        selectedLevelObject.DestroyGameObjects();

        // Remove level object from lists of level objects
        LevelInfo.levelObjects.Remove(selectedLevelObject);
        LevelInfo.levelData.levelObjects.Remove(selectedLevelObject);

        selectedLevelObject = null;

        // Enable add object button
        addObjectButton.interactable = true;

        // Close level object options menu
        levelObjectOptionsPanel.SetActive(false);
    }

    // Adds an object at selected location
    public void AddObject()
    {
        // Create new object
        int objectID = levelObjectSelectionDropdown.value;
        LevelObjects.BaseLevelObject levelObject = Utils.IDToLevelObject(objectID);
        levelObject.posX = selectionStart.x;
        levelObject.posY = selectionStart.y;

        // Add object to level
        LevelInfo.levelData.levelObjects.Add(levelObject);
        levelObject.CreateGameObjects(LevelInfo.levelGenerator.levelObjectParent.transform);

        selectedLevelObject = levelObject;

        // Disable add object button
        addObjectButton.interactable = false;

        // Open level object options menu
        levelObjectOptionsPanel.SetActive(true);
        UpdateLevelObjectOptions(levelObject);
    }

    // Resets selection
    void ResetSelection()
    {
        selectionStart = -Vector2Int.one;
        selectionEnd = -Vector2Int.one;

        selectionBox.transform.localScale = Vector3.zero;

        selectionInfoPanel.SetActive(false);

        applyToSelectionButton.interactable = false;
        addObjectButton.interactable = false;
    }

    void Update()
    {
        // Selection start
        if (Input.GetMouseButtonDown(0))
        {
            // If cursor is over GUI element, don't make a tile selection
            validSelection = !EventSystem.current.IsPointerOverGameObject();

            // Record location of start of selection
            if (validSelection) {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                int x = Mathf.RoundToInt(worldPoint.x);
                int y = Mathf.RoundToInt(worldPoint.z);
                selectionStart = new Vector2Int(x, y);

                selectionInfoPanel.SetActive(true);

                applyToSelectionButton.interactable = false;
                levelObjectOptionsPanel.SetActive(false);
                addObjectButton.interactable = false;
            }
        }
        
        // Selection current and end
        if (Input.GetMouseButton(0) && validSelection)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(worldPoint.x);
            int y = Mathf.RoundToInt(worldPoint.z);
            selectionEnd = new Vector2Int(x, y);

            Vector2 midPoint = (Vector2)(selectionStart + selectionEnd) / 2;
            Vector2 selection = selectionEnd - selectionStart;

            selectionBox.transform.position = new Vector3(midPoint.x, 1, midPoint.y);
            selectionBox.transform.localScale = new Vector3(Mathf.Abs(selection.x) + 1, Mathf.Abs(selection.y) + 1, 1);

            Vector2Int[] corners = Utils.GetTopLeftAndBottomRight(selectionStart, selectionEnd);

            selectionInfoText.text = corners[0].x + "," + corners[0].y + " - " + corners[1].x + "," + corners[1].y;
        }

        // Selection complete
        if (Input.GetMouseButtonUp(0) && validSelection)
        {
            Vector2Int selection = selectionEnd - selectionStart;

            applyToSelectionButton.interactable = true;

            // Check whether an object can be added in this position
            bool canAddObject = selection.x == 0 && selection.y == 0
                && selectionStart.x >= 0 && selectionStart.x < LevelInfo.levelData.sizeX
                && selectionStart.y >= 0 && selectionStart.y < LevelInfo.levelData.sizeY;

            selectedLevelObject = null;

            if (canAddObject)
            {
                foreach (LevelObjects.BaseLevelObject levelObject in LevelInfo.levelObjects)
                {
                    if (selectionStart.x == levelObject.posX && selectionStart.y == levelObject.posY)
                    {
                        canAddObject = false;

                        selectedLevelObject = levelObject;
                        UpdateLevelObjectOptions(levelObject);

                        break;
                    }
                }
            }

            levelObjectOptionsPanel.SetActive(selectedLevelObject != null);
            addObjectButton.interactable = canAddObject;
        }

        // Reset Selection
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetSelection();
        }
    }
}
