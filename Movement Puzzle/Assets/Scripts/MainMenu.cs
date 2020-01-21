using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level");
    }

    public void Editor()
    {
        SceneManager.LoadScene("Level Editor");
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
