using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Level
{
    public static LevelGenerator levelGenerator;
    public static PlayerManager playerManager;

    public static LevelData levelData;

    public static ColorScheme colorScheme;

    [UnityEngine.RuntimeInitializeOnLoadMethod]
    static void Load()
    {
        Events.OnPlayerReachedGoal += OnPlayerReachedGoal;
    }

    static void OnPlayerReachedGoal()
    {
        // Check if all players have reached goal
        if (playerManager.players.TrueForAll(player => player.reachedGoal))
        {
            Debug.Log("Win!");
        }
    }
}
