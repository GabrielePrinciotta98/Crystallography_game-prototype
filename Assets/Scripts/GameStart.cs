using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GameObject che fa parte della 1a scena del gioco, che 
// fa i setup necessari
public class GameStart : MonoBehaviour
{
    public Button startGameButton;
    public int debugLevelCounter = -1;
    void Awake()
    {
        // Inizializza i livelli
        LevelData.SetLevels = new List<Level>(); 
        if (debugLevelCounter == -1)
            startGameButton.onClick.AddListener(LevelLoader.LoadNextLevel);
        else
            LevelLoader.LevelCounter = debugLevelCounter;
    }

    void Update()
    {
        
    }
}
