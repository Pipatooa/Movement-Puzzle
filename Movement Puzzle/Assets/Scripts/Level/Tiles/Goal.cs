﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Goal : BaseTile
    {
        GameObject gameObject;
        bool goalUsed;

        // Set properties of tile
        public Goal() : base()
        {
            objectID = 1;
            traversable = true;
        }

        // Creates all objects for tile under parent transform
        public override void CreateGameObjects(Transform parentTransform)
        {
            GameObject goalParent = new GameObject("Goal");
            goalParent.transform.SetParent(parentTransform);
            goalParent.transform.position = new Vector3(x, 2.5f, y);

            GameObject goalObject = GameObject.Instantiate(LevelInfo.levelAssets.goal, goalParent.transform);
            goalObject.GetComponent<Renderer>().material = LevelInfo.colorScheme.goalColor.material;

            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.tile, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), goalParent.transform);
            gameObject.transform.localScale *= 0.9f;
            gameObject.GetComponent<Renderer>().material = LevelInfo.colorScheme.goalColor.material;
            gameObject.isStatic = true;
        }

        // Processes a player that has landed on this tile
        public override void ProcessPlayer(ref Player player)
        {
            if (!goalUsed)
            {
                SaveGoalState();
                
                player.reachedGoal = true;
                player.gameObject.SetActive(false);

                Events.OnPlayerReachedGoal();

                goalUsed = true;
                gameObject.SetActive(false);
            }
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