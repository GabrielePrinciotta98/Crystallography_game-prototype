using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    private static int levelCounter;
    private static int prevScene;

    public static bool LoadedData { get; set; }

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
        prevScene = 2;

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
}
