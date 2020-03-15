using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Goal : BaseTile
    {
        GameObject goalParent;
        GameObject gameObject;
        GameObject tileObject;
        
        GameObject textObject;
        TMPro.TextMeshPro textMesh;

        bool goalUsed;

        // Set properties of tile
        public Goal() : base()
        {
            tileID = 1;
            traversable = true;
        }

        // Creates all objects for tile under parent transform
        public override void CreateGameObjects(Transform parentTransform)
        {
            goalParent = new GameObject("Goal");
            goalParent.transform.SetParent(parentTransform);
            goalParent.transform.position = new Vector3(x, 2.5f, y);

            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.goal, goalParent.transform);
            gameObject.GetComponent<Renderer>().material = LevelInfo.colorScheme.goalColor.material;

            tileObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), goalParent.transform);
            tileObject.transform.localScale *= LevelInfo.levelGenerator.tileSize;
            tileObject.GetComponent<Renderer>().material = LevelInfo.colorScheme.goalColor.material;
            tileObject.isStatic = true;
        }

        // Creates level editor game objects for level object under parent transform
        public override void LevelEditorCreateGameObjects(Transform parentTransform)
        {
            goalParent = new GameObject("Goal");
            goalParent.transform.SetParent(parentTransform);
            goalParent.transform.position = new Vector3(x, 2.5f, y);

            tileObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), goalParent.transform);
            tileObject.transform.localScale *= LevelInfo.levelGenerator.tileSize;
            tileObject.GetComponent<Renderer>().material = LevelInfo.colorScheme.goalColor.material;
            tileObject.isStatic = true;

            textObject = new GameObject("Text");
            textObject.transform.SetParent(goalParent.transform);
            textObject.transform.localPosition = new Vector3(0, 0.5f, 0);
            textObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            textMesh = textObject.AddComponent<TMPro.TextMeshPro>();
            textMesh.transform.position = new Vector3(x, 0.5f, y);
            textMesh.text = "G";
            textMesh.fontSize = 7;
            textMesh.alignment = TMPro.TextAlignmentOptions.Center;
        }

        // Destorys all game objects for this tile
        public override void DestroyGameObjects()
        {
            GameObject.Destroy(goalParent);
            GameObject.Destroy(gameObject);
            GameObject.Destroy(tileObject);
            GameObject.Destroy(textMesh);
        }

        // Processes an object that has landed on this tile
        public override void ProcessObjectEntry(ref LevelObjects.BaseLevelObject moveableObject)
        {
            if (!goalUsed & moveableObject is LevelObjects.Player)
            {
                LevelObjects.Player player = moveableObject as LevelObjects.Player;

                SaveGoalState();

                player.reachedGoal = true;
                player.gameObject.SetActive(false);

                Events.OnPlayerReachedGoal?.Invoke();

                goalUsed = true;
                gameObject.SetActive(false);
            }
        }

        // Returns a new tile of this type with same properties
        public override BaseTile CreateCopy()
        {
            Goal tile = new Goal();

            return tile;
        }

        // Information about a change that has occured to the goal
        class GoalChange : UndoSystem.Change
        {
            // Info about change
            Goal goal;

            public bool oldGoalUsedStatus;

            public GoalChange(Goal goal)
            {
                this.goal = goal;

                oldGoalUsedStatus = goal.goalUsed;
            }

            public override void UndoChange()
            {
                goal.UndoGoalChange(this);
            }
        }

        // Undo a change that has occured to the goal
        void UndoGoalChange(GoalChange goalChange)
        {
            goalUsed = goalChange.oldGoalUsedStatus;

            gameObject.SetActive(!goalUsed);
        }

        // Saves an old state of the goal as a change
        void SaveGoalState()
        {
            GoalChange goalChange = new GoalChange(this);
            UndoSystem.AddChange(goalChange);
        }
    }
}