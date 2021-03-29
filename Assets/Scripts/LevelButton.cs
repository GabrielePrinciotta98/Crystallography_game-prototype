using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private LevelSelectionManager levelSelectionManager;
    private int levelNumber;

    public int LevelNumber
    {
        get => levelNumber;
        set => levelNumber = value;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        levelSelectionManager = FindObjectOfType<LevelSelectionManager>();
        levelSelectionManager.UpdateLevelButtonsCounter();
        LevelNumber = levelSelectionManager.LevelButtonsCounter;
        transform.GetChild(0).GetComponent<Text>().text = LevelNumber.ToString();
        if (LevelNumber <= 8) 
            GetComponent<Button>().onClick.AddListener(delegate { LevelLoader.LoadLevel(levelNumber); });
        else
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelNumber <= 8)
            transform.GetChild(1).gameObject.SetActive(!GetComponent<Button>().interactable);
    }
}
