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
    public bool debugMode; 
    public bool zoomUnlocked;
    public bool lambdaUnlocked;
    public bool powerUnlocked;
    public bool rotationUnlocked;
    public bool swapUnlocked;
    public bool moleculeUnlocked;
    void Awake()
    {
        print("build 1.3");
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

        if (!debugMode) return;
        PowerUpsManger.ZoomUnlocked = zoomUnlocked;
        PowerUpsManger.LambdaUnlocked = lambdaUnlocked;
        PowerUpsManger.PowerUnlocked = powerUnlocked;
        PowerUpsManger.RotationUnlocked = rotationUnlocked;
        PowerUpsManger.SwapUnlocked = swapUnlocked;
        PowerUpsManger.MoleculeUnlocked = moleculeUnlocked;
    }

    void LoadLevelSelection()
    {
        SceneManager.LoadScene(2);
    }
    
    
}
