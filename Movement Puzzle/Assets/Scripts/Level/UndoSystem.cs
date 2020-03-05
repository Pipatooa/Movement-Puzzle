using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UndoSystem
{
    public static State newState = new State();
    static List<State> states = new List<State>();

    // Collection of changes since previous state
    public class State
    {
        // List of changes that have been made since previous state
        public List<Change> changes = new List<Change>();

        // Undo all changes that have been made since previous state
        public void UndoChanges()
        {
            foreach (Change change in changes)
            {
                change.UndoChange();
            }
        }
    }

    // Single change with function to undo that change
    public class Change
    {
        // Info about change
        public virtual void UndoChange()
        {

        }
    }

    // Adds a change to the list of changes since last state
    public static void AddChange(Change change)
    {
        newState.changes.Add(change);
    }

    // Saves recent changes as a new state
    public static void SaveChanges()
    {
        states.Add(newState);

        newState = new State();
    }

    // Undo changes since previous state
    public static void Undo()
    {
        Debug.Log(states.Count);
        
        if (states.Count == 0) return;
        
        State currentState = states[states.Count - 1];

        // Run undo function of current state
        currentState.UndoChanges();

        // Remove current state from stack
        states.RemoveAt(states.Count - 1);
    }

    // Rolls back all changes made since level load
    public static void ResetLevel()
    {
        while (states.Count > 0)
        {
            Undo();
        }
    }

    // Removes all states from state list
    public static void ClearStates()
    {
        states = new List<State>();
    }
}
