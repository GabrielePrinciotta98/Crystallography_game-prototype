using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private LevelSelectionManager levelSelectionManager;
    private AudioManager audioManager;
    private int levelNumber;
    private int availableLevels;
    [SerializeReference] private Sprite completedLevel;

    private int LevelNumber
    {
        get => levelNumber;
        set => levelNumber = value;
    }

    protected void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        levelSelectionManager = FindObjectOfType<LevelSelectionManager>();
        levelSelectionManager.UpdateLevelButtonsCounter();
        LevelNumber = levelSelectionManager.LevelButtonsCounter;
        transform.GetChild(0).GetComponent<Text>().text = LevelNumber.ToString();
        transform.GetChild(1).GetComponent<Text>().text = levelSelectionManager.LevelDescriptions[LevelNumber-1];
        availableLevels = 10;
        if (LevelNumber <= availableLevels)
        {
            transform.GetChild(2).gameObject.SetActive(!GetComponent<Button>().interactable);
            GetComponent<Button>().onClick.AddListener(delegate { LevelLoader.LoadLevel(levelNumber); });
            GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
            GetComponent<Button>().onClick.AddListener(delegate { audioManager.PlayInLoop("GameplayTheme"); });
            GetComponent<Button>().onClick.AddListener(delegate { audioManager.Stop("MenuTheme"); });
        }

        if (LevelNumber >= LevelsUnlocked.NumberOfLevelsUnlocked) return;
        GetComponent<Image>().sprite = completedLevel;
        ColorBlock oldColors = GetComponent<Button>().colors;
        ColorBlock cb = new ColorBlock
        {
            normalColor = oldColors.normalColor,
            highlightedColor = new Color(150, 254, 110),
            pressedColor = new Color(7, 225, 70),
            selectedColor = oldColors.selectedColor,
            disabledColor = oldColors.disabledColor,
            colorMultiplier = 1f,
            fadeDuration = 0.1f
        };
        GetComponent<Button>().colors = cb;

    }
    
}
