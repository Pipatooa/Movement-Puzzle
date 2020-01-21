using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static void NextLevel()
    {
        LevelInfo.currentLevel = "level2.level";
        SceneManager.LoadScene("Level");
    }
}
