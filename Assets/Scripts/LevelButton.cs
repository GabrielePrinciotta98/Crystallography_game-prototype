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
        this.GetComponent<Button>().onClick.AddListener(delegate { LevelLoader.LoadLevel(levelNumber); });
        this.transform.GetChild(0).GetComponent<Text>().text = LevelNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(1).gameObject.SetActive(!GetComponent<Button>().interactable);
    }
}
