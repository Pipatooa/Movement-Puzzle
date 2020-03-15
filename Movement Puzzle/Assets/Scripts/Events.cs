using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public delegate void DelegateFunc();

    public static DelegateFunc LevelInit;

    public static DelegateFunc LevelUpdate;
    public static DelegateFunc OnLevelUpdate;

    public static DelegateFunc OnPlayerReachedGoal;

    // Clears all attached events
    public static void Clear()
    {
        LevelInit = null;
        
        LevelUpdate = null;
        OnLevelUpdate = null;

        OnPlayerReachedGoal = null;
    }

    // Called when the player moves
    public static void PlayerMoved()
    {
        ColorManager.CalculateColors();

        LevelUpdate?.Invoke();
        OnLevelUpdate?.Invoke();

        UndoSystem.SaveChanges();
    }

    // Called when the player reaches a goal
    public static void PlayerReachedGoal()
    {
        OnPlayerReachedGoal?.Invoke();
    }
}
