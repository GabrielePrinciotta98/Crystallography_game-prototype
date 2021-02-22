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
    public Button shopButton;
    public Button quitButton;
    public int debugLevelCounter = -1;
    
    void Awake()
    {
        // Inizializza i livelli
        if (LevelLoader.LoadedData == false)
        {
            LevelData.Levels = new List<Level>();
            ShopItemsData.ShopItems = new List<ShopItem>();
            Debug.Log("loaded");
        }

        LevelLoader.LoadedData = true;
        if (shopButton)
            shopButton.onClick.AddListener(LevelLoader.LoadShop);
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
    }

    void LoadLevelSelection()
    {
        SceneManager.LoadScene(2);
    }
    
    
}
