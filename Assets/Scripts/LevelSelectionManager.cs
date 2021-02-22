using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    private int levelButtonsCounter;
    //private int levelButtonsNumber = 20;
    private GameObject canvas;
    [SerializeReference] private GameObject levelButton;
    private Vector3 originPos = new Vector3(-500, -600, 0);
    private GameObject backButton;
    
    public int LevelButtonsCounter
    {
        get => levelButtonsCounter;
        private set => levelButtonsCounter = value;
    }

    void Start()
    {
        backButton = GameObject.Find("BackButton");
        backButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadMenu);
        
        canvas = GameObject.Find("Canvas");
        int c = 0;
        for (int i = 4; i > 0; i--)
        for (int j = 0; j < 6; j++)
        {
            var cur = Instantiate(levelButton, canvas.transform);
            cur.transform.localPosition = originPos + new Vector3(200 * j, 200 * i, 0);
            if (c == 7)
                cur.GetComponent<Button>().interactable = false;
            else
                c++;
        }

    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateLevelButtonsCounter()
    {
        LevelButtonsCounter++;
    }
}
