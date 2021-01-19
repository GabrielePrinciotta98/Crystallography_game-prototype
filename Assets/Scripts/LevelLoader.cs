using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    private static int levelCounter = 0;

    public static int LevelCounter
    {
        get => levelCounter;
        set => levelCounter = value;
    }

    public static void LoadLevel(int i)
    {
        levelCounter = i;
        SceneManager.LoadScene(1);
    }

    public static void LoadNextLevel()
    {
        Debug.Log(levelCounter);
        levelCounter++;
        SceneManager.LoadScene(1);
    }
}
