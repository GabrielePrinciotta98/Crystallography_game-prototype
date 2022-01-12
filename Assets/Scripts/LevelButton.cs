using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private LevelSelectionManager levelSelectionManager;
    private AudioManager audioManager;
    private int levelNumber;
    private int availableLevels;

    public int LevelNumber
    {
        get => levelNumber;
        set => levelNumber = value;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        levelSelectionManager = FindObjectOfType<LevelSelectionManager>();
        levelSelectionManager.UpdateLevelButtonsCounter();
        LevelNumber = levelSelectionManager.LevelButtonsCounter;
        transform.GetChild(0).GetComponent<Text>().text = LevelNumber.ToString();
        availableLevels = 11;
        if (LevelNumber <= availableLevels)
        {
            GetComponent<Button>().onClick.AddListener(delegate { LevelLoader.LoadLevel(levelNumber); });
            GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelNumber <= availableLevels)
            transform.GetChild(1).gameObject.SetActive(!GetComponent<Button>().interactable);
    }
}
