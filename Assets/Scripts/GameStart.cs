using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GameObject che fa parte della 1a scena del gioco, che 
// fa i setup necessari
public class GameStart : MonoBehaviour
{
    private AudioManager audioManager;
    public Button startGameButton;
    public Button musicButton;
    public Button musicButtonDisabled;
    public Button sfxButton;
    public Button sfxButtonDisabled;
    public Button creditsButton;
    public Button backButton;
    public GameObject creditsBG;
    public int debugLevelCounter = -1;
    public bool debugMode;
    public bool zoomUnlocked;
    public bool lambdaUnlocked;
    public bool powerUnlocked;
    public bool rotationUnlocked;
    public bool swapUnlocked;
    public bool moleculeUnlocked;
    public bool deleteData;
    
    void Awake()
    {
        print("build 1.5");
        // Inizializza i livelli
        if (deleteData)
        {
            PlayerPrefs.DeleteAll();
        }
        if (LevelLoader.LoadedData == false)
        {
            LevelData.Levels = new List<Level>();
            ShopItemsData.ShopItems = new List<ShopItem>();
            LoadGameData();
            Debug.Log("loaded");
        }

        audioManager = FindObjectOfType<AudioManager>();
        LevelLoader.LoadedData = true;
        

        switch (debugLevelCounter)
        {
            case -1:
                startGameButton.onClick.AddListener(LevelLoader.LoadNextLevel);
                startGameButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
                break;
            case -2:
                startGameButton.onClick.AddListener(LoadLevelSelection);
                startGameButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
                break;
            default:
                LevelLoader.LevelCounter = debugLevelCounter;
                break;
        }

        if (!debugMode)
        {
            musicButton.onClick.AddListener(StopMusic);
            musicButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            musicButtonDisabled.onClick.AddListener(PlayMusic);
            musicButtonDisabled.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            sfxButton.onClick.AddListener(StopSfx);
            sfxButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            sfxButtonDisabled.onClick.AddListener(PlaySfx);
            sfxButtonDisabled.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            creditsButton.onClick.AddListener(ShowCredits);
            creditsButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            backButton.onClick.AddListener(ExitCredits);
            backButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            return;
        }

        PowerUpsManager.ZoomUnlocked = zoomUnlocked;
        PowerUpsManager.LambdaUnlocked = lambdaUnlocked;
        PowerUpsManager.PowerUnlocked = powerUnlocked;
        PowerUpsManager.RotationUnlocked = rotationUnlocked;
        PowerUpsManager.SwapUnlocked = swapUnlocked;
        PowerUpsManager.MoleculeUnlocked = moleculeUnlocked;
    }

    private void LoadGameData()
    {
        LevelLoader.LevelCounter = PlayerPrefs.GetInt("LevelsUnlocked", 1);
        LevelsUnlocked.LoadLevelsUnlocked(PlayerPrefs.GetInt("LevelsUnlocked", 1));

        ScoreManager.Score = PlayerPrefs.GetInt("Score", 5000);
        PowerUpsManager.LambdaUnlocked = IntToBool(PlayerPrefs.GetInt("LambdaUnlocked", 0));
        PowerUpsManager.ZoomUnlocked = IntToBool(PlayerPrefs.GetInt("ZoomUnlocked", 0));
        PowerUpsManager.SwapUnlocked = IntToBool(PlayerPrefs.GetInt("SwapUnlocked", 0));
        PowerUpsManager.PowerUnlocked = IntToBool(PlayerPrefs.GetInt("PowerUnlocked", 0));
        PowerUpsManager.RotationUnlocked = IntToBool(PlayerPrefs.GetInt("RotationUnlocked", 0));
        PowerUpsManager.MoleculeUnlocked = IntToBool(PlayerPrefs.GetInt("MoleculeUnlocked", 0));
        PowerUpsManager.CrystalUnlocked = IntToBool(PlayerPrefs.GetInt("CrystalUnlocked", 0));
    }

    private void Start()
    {
        if (debugMode) return;
        if (AudioManager.musicOn)
            PlayMusic();
        else
            StopMusic();
        
        if (AudioManager.sfxOn)
            PlaySfx();
        else
            StopSfx();
    }

    void LoadLevelSelection()
    {
        SceneManager.LoadScene(2);
    }

    private void StopMusic()
    {
        audioManager.StopAll(SoundType.Music);
        musicButton.gameObject.SetActive(false);
        musicButtonDisabled.gameObject.SetActive(true);
    }

    private void PlayMusic()
    {
        audioManager.PlayAll(SoundType.Music);
        musicButton.gameObject.SetActive(true);
        musicButtonDisabled.gameObject.SetActive(false);
        audioManager.PlayInLoop("MenuTheme");
    }

    private void StopSfx()
    {
        audioManager.StopAll(SoundType.Sound);
        sfxButton.gameObject.SetActive(false);
        sfxButtonDisabled.gameObject.SetActive(true);
    }

    private void PlaySfx()
    {
        audioManager.PlayAll(SoundType.Sound);
        sfxButton.gameObject.SetActive(true);
        sfxButtonDisabled.gameObject.SetActive(false);
    }

    private void ShowCredits()
    {
        creditsBG.SetActive(true);
    }

    private void ExitCredits()
    {
        creditsBG.SetActive(false);
    }

    private bool IntToBool(int val)
    {
        return val != 0;
    }
}
