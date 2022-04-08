using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{

    private AudioManager audioManager;
    private GameObject canvas;
    [SerializeReference] private GameObject levelButton;
    private readonly Vector3 originPos = new Vector3(-400, -550, 0);
    private GameObject backButton;
    private GameObject shopButton;
    private GameObject score;
    private readonly List<GameObject> levelButtons = new List<GameObject>();
    public int LevelButtonsCounter { get; private set; }

    public List<string> LevelDescriptions { get; } = new List<string>();
    private const string Desc1 = "Atomi: 2\n Piano: XY";
    private const string Desc2 = "Atomi: 3\n Piano: XY";
    private const string Desc3 = "Atomi: 4\n Piano: XY";
    private const string Desc4 = "Atomi: 2\n Piano: YZ";
    private const string Desc5 = "Atomi: 3\n Piano: YZ";
    private const string Desc6 = "Atomi: 4\n Piano: YZ";
    private const string Desc7 = "Atomi: 2\n Spazio: XYZ";
    private const string Desc8 = "Atomi: 3\n Spazio: XYZ";
    private const string Desc9 = "Atomi: 4\n Spazio: XYZ";
    private const string Desc10 = "Atomi: ???\n Spazio: XYZ";
    
    void Start()
    {
        LevelDescriptions.Add(Desc1);
        LevelDescriptions.Add(Desc2);
        LevelDescriptions.Add(Desc3);
        LevelDescriptions.Add(Desc4);
        LevelDescriptions.Add(Desc5);
        LevelDescriptions.Add(Desc6);
        LevelDescriptions.Add(Desc7);
        LevelDescriptions.Add(Desc8);
        LevelDescriptions.Add(Desc9);
        LevelDescriptions.Add(Desc10);
        audioManager = FindObjectOfType<AudioManager>();
        backButton = GameObject.Find("BackButton");
        backButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadMenu);
        backButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        shopButton = GameObject.Find("ShopButton");
        shopButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadShop);
        shopButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        score = GameObject.Find("Score");
        score.GetComponent<ScoreDisplay>().DisplayScore();
        canvas = GameObject.Find("Canvas");
        int c = 0;
        for (int i = 2; i > 0; i--)
        for (int j = 0; j < 5; j++)
        {
            var cur = Instantiate(levelButton, canvas.transform);
            levelButtons.Add(cur);
            cur.transform.localPosition = originPos + new Vector3(200 * j, 325 * i, 0);
            if (c == LevelsUnlocked.NumberOfLevelsUnlocked)
                cur.GetComponent<Button>().interactable = false;
            else
                c++;
        }

    }
    
    public void UpdateInteractable()
    {
        int c = 0;
        foreach (var button in levelButtons)
        {
            if (c == LevelsUnlocked.NumberOfLevelsUnlocked)
                button.GetComponent<Button>().interactable = false;
            else
            {
                button.GetComponent<Button>().interactable = true;
                c++;
            }
        }
    }
    
    public void UpdateLevelButtonsCounter()
    {
        LevelButtonsCounter++;
    }
}
