using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UndoSystem
{
    static List<State> states = new List<State>();

    struct State
    {
        public List<LevelData.PlayerInfo> players;

        public int[] colorCounts;
        public bool[] colorStates;
    }

    public static void SaveState()
    {
        State state = new State();

        state.players = new List<LevelData.PlayerInfo>();
        foreach (Player player in LevelInfo.playerManager.players)
        {
            state.players.Add(new LevelData.PlayerInfo(player));
        }

        state.colorCounts = ColorManager.colorCounts;
        state.colorStates = ColorManager.colorStates;

        states.Add(state);
    }

    static void LoadState(State state)
    {
        for (int i = 0; i < state.players.Count; i++)
        {
            LevelInfo.playerManager.players[i].LoadState(state.players[i], false);
        }

        ColorManager.colorCounts = state.colorCounts;
        ColorManager.colorStates = state.colorStates;

        Events.LevelUpdate();
    }

    public static void Undo()
    {
        if (states.Count > 1)
        {
            LoadState(states[states.Count - 2]);
            states.RemoveAt(states.Count - 1);
        }
    }

    public static void ResetLevel()
    {
        LoadState(states[0]);
        states.RemoveRange(1, states.Count - 1);
    }

    public static void ClearStates()
    {
        states = new List<State>();
    }
}
