using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public Player currentPlayer;
    [HideInInspector] public int currentPlayerIndex;

    public bool resetLocked;
    public float resetLockTime;

    public bool levelCompleted;
    public float levelCompletedTime;

    public List<Player> players = new List<Player>();

    void Awake()
    {
        Events.OnPlayerReachedGoal += OnPlayerReachedGoal;
    }

    void OnDestory()
    {
        Events.OnPlayerReachedGoal -= OnPlayerReachedGoal;
    }

    void Start()
    {
        currentPlayer = players[0];
        currentPlayer.selected = true;

        Camera.main.GetComponent<CameraMovement>().EnableMovement();
    }

    void Update()
    {
        if (!levelCompleted)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                int nextPlayerIndex = currentPlayerIndex;

                do
                {
                    nextPlayerIndex = (nextPlayerIndex + 1) % players.Count;
                }
                while (players[nextPlayerIndex].reachedGoal && currentPlayerIndex != nextPlayerIndex);

                SetActivePlayer(nextPlayerIndex);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                resetLocked = false;

                UndoSystem.Undo();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                UndoSystem.ResetLevel();
            }

            if (resetLocked)
            {
                if (Time.time - resetLockTime > 1f)
                {
                    UndoSystem.Undo();

                    resetLocked = false;
                }
            }
        } else if (levelCompleted)
        {
            if (Time.time - levelCompletedTime > 3f)
            {
                LevelManager.NextLevel();
            }
        }
    }

    void SetActivePlayer(int index)
    {
        currentPlayer.selected = false;
        currentPlayer = players[index];
        currentPlayer.selected = true;

        currentPlayerIndex = index;
    }

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
