﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public LevelObjects.Player currentPlayer;
    [HideInInspector] public int currentPlayerIndex;

    public bool resetLocked;
    public float resetLockTime;

    public bool levelCompleted;
    public float levelCompletedTime;

    public List<LevelObjects.Player> players = new List<LevelObjects.Player>();

    void Awake()
    {
        Events.OnPlayerReachedGoal += OnPlayerReachedGoal;
    }

    void Start()
    {
        // Set current player to first in list
        currentPlayer = players[0];

        // Tell camera to move to that player and start tracking
        Camera.main.GetComponent<CameraMovement>().StartTracking();
    }

    void Update()
    {
        // Update all players
        foreach (LevelObjects.Player player in players)
        {
            player.Update();
        }
        
        // Move onto next level if level timer is complete
        if (levelCompleted)
        {
            if (Time.time - levelCompletedTime > 3f)
            {
                LevelManager.NextLevel();
            }

            return;
        }

        // Switch players
        if (Input.GetKeyDown(KeyCode.Tab)) SwitchPlayer();

        // Undo
        if (Input.GetKeyDown(KeyCode.U)) { resetLocked = false; UndoSystem.Undo(); }
        
        // Restart Level
        if (Input.GetKeyDown(KeyCode.I)) UndoSystem.ResetLevel();

        // Player movement input
        if (!resetLocked && !currentPlayer.reachedGoal)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && currentPlayer.colorIndexes[0] != -1) currentPlayer.Move(0);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && currentPlayer.colorIndexes[1] != -1) currentPlayer.Move(1);
            else if (Input.GetKeyDown(KeyCode.DownArrow) && currentPlayer.colorIndexes[2] != -1) currentPlayer.Move(2);
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentPlayer.colorIndexes[3] != -1) currentPlayer.Move(3);

            // Temporary player rotation
            if (Input.GetKeyDown(KeyCode.R))
            {
                currentPlayer.facingDir += 1;
                currentPlayer.facingDir %= 4;

                currentPlayer.gameObject.transform.rotation = Quaternion.Euler(0, currentPlayer.facingDir * 90, 0);
            }

            // Temporary next level
            if (Input.GetKeyDown(KeyCode.N)) LevelManager.NextLevel();
        }

        // Trigger undo if timer is complete
        if (resetLocked)
        {
            if (Time.time - resetLockTime > 1f)
            {
                UndoSystem.Undo();

                resetLocked = false;
            }
        }
    }

    // Selects next controllable player in player list
    void SwitchPlayer()
    {
        int nextPlayerIndex = currentPlayerIndex;

        do
        {
            nextPlayerIndex = (nextPlayerIndex + 1) % players.Count;
        }
        while (players[nextPlayerIndex].reachedGoal && currentPlayerIndex != nextPlayerIndex);

        currentPlayer = players[nextPlayerIndex];
        currentPlayerIndex = nextPlayerIndex;
    }

    // Triggered when a player reaches enters a goal
    void OnPlayerReachedGoal()
    {
        // Check if all players have reached goal
        if (players.TrueForAll(player => player.reachedGoal))
        {
            levelCompleted = true;
            levelCompletedTime = Time.time;
        }
    }
}
