using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorManager
{
    public static int[] colorCounts;
    public static bool[] colorStates;

    public static void CheckPlayerColors()
    {
        foreach (Player player in LevelInfo.playerManager.players)
        {
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

    public static void CalculateColors()
    {
        for (int i=0; i < colorStates.Length; i++)
        {
            colorStates[i] = colorCounts[i] > 0;
        }

        CheckPlayerColors();

        Events.ColorUpdate();
    }
}
