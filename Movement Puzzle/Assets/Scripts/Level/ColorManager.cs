using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorManager
{
    public static int[] colorCounts;
    public static bool[] colorStates;

    // Resets all color counts to be disabled
    public static void ResetColorCounts()
    {
        colorCounts = new int[LevelInfo.colorScheme.colors.Count];
        colorStates = new bool[LevelInfo.colorScheme.colors.Count];
    }

    // Sets color states based on player facing directions
    public static void CheckPlayerColors()
    {
        // Loop through all players
        foreach (Player player in LevelInfo.playerManager.players)
        {
            // Do not count player if they have reached the goal
            if (player.reachedGoal)
            {
                continue;
            }
            
            // Set the appropriate color state to true if the player is facing in that direction
            switch (player.lastMoveDir)
            {
                case 0:
                    colorStates[player.colorIndexUp] = true;
                    break;
                case 1:
                    colorStates[player.colorIndexRight] = true;
                    break;
                case 2:
                    colorStates[player.colorIndexDown] = true;
                    break;
                case 3:
                    colorStates[player.colorIndexLeft] = true;
                    break;
            }
        }
    }

    // Calculate which colors should be enabled
    public static void CalculateColors()
    {
        // Take color counts set by tiles (eg. switches) into account
        for (int i = 0; i < colorStates.Length; i++)
        {
            colorStates[i] = colorCounts[i] > 0;
        }

        CheckPlayerColors();
    }
}
