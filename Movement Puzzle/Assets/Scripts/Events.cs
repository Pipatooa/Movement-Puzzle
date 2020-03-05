using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public delegate void DelegateFunc();
    
    public static DelegateFunc LevelUpdate;
    public static DelegateFunc OnLevelUpdate;

    public static DelegateFunc OnPlayerReachedGoal;

    public static void ColorUpdate()
    {
        LevelUpdate();
        OnLevelUpdate();

        UndoSystem.SaveChanges();
    }

    public static void PlayerReachedGoal()
    {
        OnPlayerReachedGoal();
    }
}
