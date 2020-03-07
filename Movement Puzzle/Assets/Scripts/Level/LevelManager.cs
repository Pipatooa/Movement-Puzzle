using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    // Loads the next level
    public static void NextLevel()
    {
        Events.Clear();
        
        // LevelInfo.currentLevel = "level2.level";
        SceneManager.LoadScene("Level");
    }
}
