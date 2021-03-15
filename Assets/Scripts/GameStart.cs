using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GameObject che fa parte della 1a scena del gioco, che 
// fa i setup necessari
public class GameStart : MonoBehaviour
{
    public Button startGameButton;
    public Button quitButton;
    public int debugLevelCounter = -1;
    public bool zoomUnlocked;
    public bool lambdaUnlocked;
    public bool powerUnlocked;
    public bool rotationUnlocked;
    
    
    void Awake()
    {
        print("build 1.2");
        // Inizializza i livelli
        if (LevelLoader.LoadedData == false)
        {
            LevelData.Levels = new List<Level>();
            ShopItemsData.ShopItems = new List<ShopItem>();
            Debug.Log("loaded");
        }

        LevelLoader.LoadedData = true;
        if (quitButton)
            quitButton.onClick.AddListener(LevelLoader.QuitGame);
        switch (debugLevelCounter)
        {
            case -1:
                startGameButton.onClick.AddListener(LevelLoader.LoadNextLevel);
                break;
            case -2:
                startGameButton.onClick.AddListener(LoadLevelSelection);
                break;
            default:
                LevelLoader.LevelCounter = debugLevelCounter;
                break;
        }

        if (zoomUnlocked)
            PowerUpsManger.ZoomUnlocked = true;
        if (lambdaUnlocked)
            PowerUpsManger.LambdaUnlocked = true;
        if (powerUnlocked)
            PowerUpsManger.PowerUnlocked = true;
        if (rotationUnlocked)
            PowerUpsManger.RotationUnlocked = true;
    }

    void LoadLevelSelection()
    {
        SceneManager.LoadScene(2);
    }
    
    
}
