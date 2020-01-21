//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class LevelEditorWindow : EditorWindow
//{
//    string fileName;

//    ColorScheme colorScheme;
//    GameObject tilePrefab;
//    Material tileMaterial;

//    bool newLevel = false;
//    string newLevelName = "New Level";

//    int newLevelSizeX;
//    int newLevelSizeY;

//    string[] directions = new string[4] { "Up", "Right", "Down", "Left" };

//    int selectionNewColorIndex;

//    Vector2 scrollPosition;

//    LevelData levelData;
//    Material[] materials;
//    string[] colorNames;

//    GameObject editorObject;
//    GameObject tileParentObject;
//    GameObject textParentObject;

//    [MenuItem("Window/Level Editor")]
//    public static void ShowWindow()
//    {
//        GetWindow<LevelEditorWindow>("Level Editor");
//    }

//    void OnGUI()
//    {
//        fileName = EditorGUILayout.TextField("File Name", fileName);

//        GUILayout.Space(10);

//        GUILayout.BeginHorizontal();
//        EditorGUILayout.PrefixLabel("Color Scheme");
//        colorScheme = EditorGUILayout.ObjectField(colorScheme, typeof(ColorScheme), true) as ColorScheme;
//        GUILayout.EndHorizontal();

//        GUILayout.BeginHorizontal();
//        EditorGUILayout.PrefixLabel("Tile Prefab");
//        tilePrefab = EditorGUILayout.ObjectField(tilePrefab, typeof(GameObject), true) as GameObject;
//        GUILayout.EndHorizontal();

//        GUILayout.BeginHorizontal();
//        EditorGUILayout.PrefixLabel("Tile Material");
//        tileMaterial = EditorGUILayout.ObjectField(tileMaterial, typeof(Material), true) as Material;
//        GUILayout.EndHorizontal();

//        GUILayout.Space(10);

//        newLevel = EditorGUILayout.Toggle("Create new level", newLevel);

//        GUILayout.Space(10);

//        GUILayout.BeginHorizontal();
//        if (!newLevel)
//        {
//            if (GUILayout.Button("Load Level"))
//            {
//                LoadLevel();
//            }

//            if (GUILayout.Button("Save Level"))
//            {
//                SaveLevel();
//            }

//            if (GUILayout.Button("Stop Editing"))
//            {
//                StopEditing();
//            }
//        }
//        else
//        {
//            if (GUILayout.Button("Create New Level"))
//            {
//                NewLevel();
//            }
//        }
//        GUILayout.EndHorizontal();

//        GUILayout.Space(10);

//        if (newLevel)
//        {
//            newLevelName = EditorGUILayout.TextField("Level Name", newLevelName);

//            GUILayout.Space(20);

//            GUILayout.Label("New Level Size");
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("X");
//            newLevelSizeX = Mathf.Max(1, EditorGUILayout.IntField(newLevelSizeX));
//            GUILayout.Label("Y");
//            newLevelSizeY = Mathf.Max(1, EditorGUILayout.IntField(newLevelSizeY));
//            GUILayout.EndHorizontal();
//        } else
//        {
//            if (levelData != null)
//            {
//                GUILayout.Label("Level Info:", EditorStyles.boldLabel);
//                GUILayout.Label("Size: " + levelData.tileArray.GetLength(0) + ", " + levelData.tileArray.GetLength(1));

//                GUILayout.Space(10);

//                levelData.levelName = EditorGUILayout.TextField("Level Name", levelData.levelName);

//                GUILayout.Space(20);

//                GUILayout.Label("Goal Position");
//                GUILayout.BeginHorizontal();
//                GUILayout.Label("X", GUILayout.ExpandWidth(false));
//                levelData.goalX = Mathf.Max(0, EditorGUILayout.IntField(levelData.goalX));
//                GUILayout.Label("Y", GUILayout.ExpandWidth(false));
//                levelData.goalY = Mathf.Max(0, EditorGUILayout.IntField(levelData.goalY));
//                GUILayout.EndHorizontal();

//                GUILayout.Space(10);

//                GUILayout.Label("Selection");

//                GUILayout.BeginHorizontal();
//                EditorGUILayout.PrefixLabel("New Tile Color");
//                selectionNewColorIndex = EditorGUILayout.Popup(selectionNewColorIndex + 1, colorNames) - 1;
//                if (selectionNewColorIndex != -1) EditorGUILayout.ColorField(colorScheme.colors[selectionNewColorIndex].material.color);
//                GUILayout.EndHorizontal();

//                if (GUILayout.Button("Set Tile Color"))
//                {
//                    foreach (GameObject tileObject in Selection.gameObjects)
//                    {
//                        if (tileObject.tag != "LevelEditor-Tile")
//                        {
//                            continue;
//                        }
                        
//                        levelData.tileArray[Mathf.FloorToInt(tileObject.transform.position.x), Mathf.FloorToInt(tileObject.transform.position.y)].colorIndex = selectionNewColorIndex;

//                        if (selectionNewColorIndex != -1)
//                        {
//                            tileObject.GetComponent<Renderer>().material = materials[selectionNewColorIndex];
//                        } else
//                        {
//                            tileObject.GetComponent<Renderer>().material = tileMaterial;
//                        }
//                    }
//                }

//                GUILayout.BeginHorizontal();
//                if (GUILayout.Button("Make Traversable"))
//                {
//                    foreach (GameObject tileObject in Selection.gameObjects)
//                    {
//                        if (tileObject.tag != "LevelEditor-Tile")
//                        {
//                            continue;
//                        }

//                        levelData.tileArray[Mathf.FloorToInt(tileObject.transform.position.x), Mathf.FloorToInt(tileObject.transform.position.y)].exists = true;
//                        tileObject.transform.localScale = Vector3.one * 0.9f;
//                    }
//                }

//                if (GUILayout.Button("Make Non-Traversable"))
//                {
//                    foreach (GameObject tileObject in Selection.gameObjects)
//                    {
//                        if (tileObject.tag != "LevelEditor-Tile")
//                        {
//                            continue;
//                        }

//                        levelData.tileArray[Mathf.FloorToInt(tileObject.transform.position.x), Mathf.FloorToInt(tileObject.transform.position.y)].exists = false;
//                        tileObject.transform.localScale = Vector3.one * 0.1f;
//                    }
//                }
//                GUILayout.EndHorizontal();

//                GUILayout.Space(10);

//                GUILayout.Label("Players");
                
//                if (GUILayout.Button("New Player", GUILayout.Width(100))) {
//                    levelData.players.Add(new LevelData.PlayerInfo());
//                }

//                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
//                for (int i = 0; i < levelData.players.Count; i++)
//                {
//                    GUILayout.BeginVertical(EditorStyles.helpBox);
//                    GUILayout.BeginHorizontal();

//                    if (GUILayout.Button("Delete", GUILayout.Width(100)))
//                    {
//                        levelData.players.RemoveAt(i);
//                    }
//                    else
//                    {
//                        LevelData.PlayerInfo newPlayerInfo = levelData.players[i];

//                        GUILayout.Space(30);

//                        GUILayout.Label("X", GUILayout.ExpandWidth(false));
//                        newPlayerInfo.posX = Mathf.Max(0, EditorGUILayout.IntField(levelData.players[i].posX));
//                        GUILayout.Label("Y", GUILayout.ExpandWidth(false));
//                        newPlayerInfo.posY = Mathf.Max(0, EditorGUILayout.IntField(levelData.players[i].posY));
//                        GUILayout.EndHorizontal();

//                        GUILayout.BeginHorizontal();
//                        EditorGUILayout.PrefixLabel("Facing Dir");
//                        newPlayerInfo.facingDir = EditorGUILayout.Popup(levelData.players[i].facingDir, directions);
//                        GUILayout.EndHorizontal();

//                        GUILayout.BeginHorizontal();
//                        EditorGUILayout.PrefixLabel("Last Move Dir");
//                        newPlayerInfo.lastMoveDir = EditorGUILayout.Popup(levelData.players[i].lastMoveDir, directions);
//                        GUILayout.EndHorizontal();

//                        GUILayout.BeginHorizontal();
//                        EditorGUILayout.PrefixLabel("Color");
//                        newPlayerInfo.colorIndex = EditorGUILayout.Popup(levelData.players[i].colorIndex + 1, colorNames) - 1;
//                        if (newPlayerInfo.colorIndex != -1) EditorGUILayout.ColorField(colorScheme.colors[newPlayerInfo.colorIndex].material.color);
//                        GUILayout.EndHorizontal();

//                        GUILayout.BeginHorizontal();
//                        EditorGUILayout.PrefixLabel("Up Color");
//                        newPlayerInfo.colorIndexUp = EditorGUILayout.Popup(levelData.players[i].colorIndexUp + 1, colorNames) - 1;
//                        if (newPlayerInfo.colorIndexUp != -1) EditorGUILayout.ColorField(colorScheme.colors[newPlayerInfo.colorIndexUp].material.color);
//                        GUILayout.EndHorizontal();

//                        GUILayout.BeginHorizontal();
//                        EditorGUILayout.PrefixLabel("Right Color");
//                        newPlayerInfo.colorIndexRight = EditorGUILayout.Popup(levelData.players[i].colorIndexRight + 1, colorNames) - 1;
//                        if (newPlayerInfo.colorIndexRight != -1) EditorGUILayout.ColorField(colorScheme.colors[newPlayerInfo.colorIndexRight].material.color);
//                        GUILayout.EndHorizontal();

//                        GUILayout.BeginHorizontal();
//                        EditorGUILayout.PrefixLabel("Down Color");
//                        newPlayerInfo.colorIndexDown = EditorGUILayout.Popup(levelData.players[i].colorIndexDown + 1, colorNames) - 1;
//                        if (newPlayerInfo.colorIndexDown != -1) EditorGUILayout.ColorField(colorScheme.colors[newPlayerInfo.colorIndexDown].material.color);
//                        GUILayout.EndHorizontal();

//                        GUILayout.BeginHorizontal();
//                        EditorGUILayout.PrefixLabel("Left Color");
//                        newPlayerInfo.colorIndexLeft = EditorGUILayout.Popup(levelData.players[i].colorIndexLeft + 1, colorNames) - 1;
//                        if (newPlayerInfo.colorIndexLeft != -1) EditorGUILayout.ColorField(colorScheme.colors[newPlayerInfo.colorIndexLeft].material.color);

//                        levelData.players[i] = newPlayerInfo;
//                    }

//                    GUILayout.EndHorizontal();
//                    GUILayout.EndVertical();

//                }
//                GUILayout.EndScrollView();
//            }
//            else
//            {
//                GUILayout.Label("No level currently loaded");
//            }
//        }
//    }

//    void NewLevel()
//    {
//        StopEditing();

//        newLevel = false;

//        levelData = new LevelData(newLevelName, newLevelSizeX, newLevelSizeY);

//        ShowLevel();
//    }

//    void ShowLevel()
//    {
//        materials = new Material[colorScheme.colors.Count];
//        colorNames = new string[colorScheme.colors.Count + 1];
//        colorNames[0] = "None";

//        for (int i = 0; i < colorScheme.colors.Count; i++)
//        {
//            materials[i] = new Material(colorScheme.shader);
//            materials[i].name = "Tile (Color " + i + ")";
//            materials[i].CopyPropertiesFromMaterial(tileMaterial);
//            materials[i].color = Color.Lerp(tileMaterial.color, colorScheme.colors[i].material.color, 0.75f);

//            colorNames[i + 1] = colorScheme.colors[i].name;
//        }

//        editorObject = new GameObject("Level Editor");
//        tileParentObject = new GameObject("Tiles");
//        tileParentObject.transform.parent = editorObject.transform;
//        textParentObject = new GameObject("Text");
//        textParentObject.transform.parent = editorObject.transform;

//        for (int x = 0; x < levelData.sizeX; x++)
//        {
//            for (int y = 0; y < levelData.sizeY; y++)
//            {
//                LevelData.Tile tile = levelData.tileArray[x, y];

//                GameObject tileObject = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, tileParentObject.transform);
//                tileObject.isStatic = true;
//                tileObject.tag = "LevelEditor-Tile";

//                if (tile.exists)
//                {
//                    tileObject.transform.localScale *= 0.9f;
//                }
//                else
//                {
//                    tileObject.transform.localScale *= 0.1f;
//                }

//                if (tile.colorIndex != -1)
//                {
//                    tileObject.GetComponent<Renderer>().material = materials[tile.colorIndex];
//                }
//            }
//        }

//        for (int x = 0; x < levelData.sizeX; x++)
//        {
//            GameObject textObject = new GameObject("Axis Label", typeof(TextMesh));
//            textObject.transform.parent = textParentObject.transform;
//            textObject.transform.localPosition = new Vector3(x, -1, 0);
//            textObject.transform.localScale *= 0.1f;

//            TextMesh textMesh = textObject.GetComponent<TextMesh>();
//            textMesh.text = x.ToString();
//            textMesh.fontSize = 50;
//            textMesh.anchor = TextAnchor.MiddleCenter;
//        }

//        for (int y = 0; y < levelData.sizeY; y++)
//        {
//            GameObject textObject = new GameObject("Axis Label", typeof(TextMesh));
//            textObject.transform.parent = textParentObject.transform;
//            textObject.transform.localPosition = new Vector3(-1, y, 0);
//            textObject.transform.localScale *= 0.1f;

//            TextMesh textMesh = textObject.GetComponent<TextMesh>();
//            textMesh.text = y.ToString();
//            textMesh.fontSize = 50;
//            textMesh.anchor = TextAnchor.MiddleCenter;
//        }
//    }

//    void StopEditing()
//    {
//        levelData = null;

//        DestroyImmediate(editorObject);
//    }

//    void LoadLevel()
//    {
//        StopEditing();

//        levelData = LoadSystem.LoadLevel(fileName);

//        ShowLevel();
//    }

//    void SaveLevel()
//    {
//        if (levelData != null)
//        {
//            LoadSystem.SaveLevel(levelData, fileName);
//        }
//    }
//}
