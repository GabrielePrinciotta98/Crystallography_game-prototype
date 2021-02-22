using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    private static int levelCounter = 0;
    private static int prevScene;

    public static int PrevScene
    {
        get => prevScene;
        set => prevScene = value;
    }

    private static bool loadedData;

    public static bool LoadedData
    {
        get => loadedData;
        set => loadedData = value;
    }

    public static int LevelCounter
    {
        get => levelCounter;
        set => levelCounter = value;
    }
    

    public static void LoadLevel(int i)
    {
        prevScene = SceneManager.GetActiveScene().buildIndex;
        levelCounter = i;
        SceneManager.LoadScene(1);
        
    }

    public static void LoadNextLevel()
    {
        prevScene = SceneManager.GetActiveScene().buildIndex;

        Debug.Log(levelCounter);
        levelCounter++;
        SceneManager.LoadScene(1);
    }
    
    public static void LoadMenu()
    {
        prevScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(0);
    }

    public static void LoadPrevScene()
    {
        SceneManager.LoadScene(prevScene);
    }

    public static void LoadShop()
    {
        prevScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(3);
    }

    public static void QuitGame()
    {
        Application.Quit();
        Debug.Log("exit");
    } 
}
