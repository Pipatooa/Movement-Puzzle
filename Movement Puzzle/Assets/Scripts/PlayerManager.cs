using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public Player currentPlayer;
    [HideInInspector] public int currentPlayerIndex;
    [HideInInspector] public bool[] colorStatuses;

    public bool resetLocked;
    public float resetLockTime;

    public List<Player> players = new List<Player>();

    void Start()
    {
        currentPlayer = players[0];
        currentPlayer.selected = true;

        colorStatuses = new bool[Level.colorScheme.colors.Count];
    }

    void Update()
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
    }

    void SetActivePlayer(int index)
    {
        currentPlayer.selected = false;
        currentPlayer = players[index];
        currentPlayer.selected = true;

        currentPlayerIndex = index;
    }

    public void UpdateColorCount()
    {
        colorStatuses = new bool[Level.colorScheme.colors.Count];

        foreach (Player player in players)
        {
            switch(player.lastMoveDir)
            {
                case 0:
                    colorStatuses[player.colorIndexUp] = true;
                    break;
                case 1:
                    colorStatuses[player.colorIndexRight] = true;
                    break;
                case 2:
                    colorStatuses[player.colorIndexDown] = true;
                    break;
                case 3:
                    colorStatuses[player.colorIndexLeft] = true;
                    break;
            }
        }

        Events.ColorUpdate();
    }
}
