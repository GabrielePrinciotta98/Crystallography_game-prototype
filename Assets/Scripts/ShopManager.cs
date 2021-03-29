using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    private GameObject score;
    private GameObject backButton;
    private GameObject textBG;
    private ShopItem[] items;
    private GameObject buyButton;
    private GameObject[] buttons;
    private GameObject infoButton;
    private GameObject infoPanel;
    private GameObject closeInfoButton;
    private GameObject nextPageButton;
    private GameObject prevPageButton;
    public Sprite[] zoomInfoSprites;
    public Sprite[] lambdaInfoSprites;
    public Sprite[] powerInfoSprites;
    public Sprite[] rotationInfoSprites;
    public Sprite[] swapInfoSprites;
    
    private GameObject zoomShopButton;
    private GameObject lambdaShopButton;
    private GameObject powerShopButton;
    private GameObject rotationShopButton;
    private GameObject swapShopButton;
    
    public Sprite[] zoomSprites;
    public Sprite[] lambdaSprites;
    public Sprite[] powerSprites;
    public Sprite[] rotationSprites;
    public Sprite[] swapSprites;
    
    private int currentItem = -1;
    private int currentInfoPage = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.Find("Score");
        score.GetComponent<ScoreDisplay>().DisplayScore();
        backButton = GameObject.Find("BackButton");
        backButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadPrevScene);
        buyButton = GameObject.Find("BuyButton");
        buyButton.GetComponent<Button>().onClick.AddListener(Buy);
        infoButton = GameObject.Find("InfoButton");
        infoButton.GetComponent<Button>().onClick.AddListener(DisplayInfo);
        closeInfoButton = GameObject.Find("CloseInfoButton");
        closeInfoButton.GetComponent<Button>().onClick.AddListener(CloseInfo);
        nextPageButton = GameObject.Find("NextPageButton");
        nextPageButton.GetComponent<Button>().onClick.AddListener(NextPage);
        prevPageButton = GameObject.Find("PrevPageButton");
        prevPageButton.GetComponent<Button>().onClick.AddListener(PreviousPage);
        
        infoPanel = GameObject.Find("InfoPanel");
        infoPanel.SetActive(false);
        textBG = GameObject.Find("TextBG");
        textBG.transform.GetChild(1).gameObject.SetActive(false);
        textBG.transform.GetChild(3).gameObject.SetActive(false);
        textBG.transform.GetChild(4).gameObject.SetActive(false);

        // INIZIALIZZO I POWER UP DA COMPRARE   

        items = ShopItemsData.ShopItems.ToArray();
        

        // INIZIALIZZO I BOTTONI 
        buttons = new[]
        {
            zoomShopButton = GameObject.Find("ZoomShop"),
            lambdaShopButton = GameObject.Find("LambdaShop"),
            powerShopButton = GameObject.Find("PowerShop"),
            rotationShopButton = GameObject.Find("RotationShop"),
            swapShopButton = GameObject.Find("SwapShop")
        };
        
        buttons[0].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(0); });
        buttons[1].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(1); });
        buttons[2].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(2); });
        buttons[3].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(3); });
        buttons[4].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(4); });

        if (LevelsUnlocked.NumberOfLevelsUnlocked < 3)
        {
            items[2].Buyable = true;
            items[4].Buyable = true;
        }
        
        if (LevelsUnlocked.NumberOfLevelsUnlocked == 3)
        {
            items[0].Buyable = true;
            items[1].Buyable = true;
            items[2].Buyable = true;
            items[4].Buyable = true;
        }
        if (LevelsUnlocked.NumberOfLevelsUnlocked >= 4)
        {
            items[3].Buyable = true;
            items[0].Buyable = true;
            items[1].Buyable = true;
            items[2].Buyable = true;
            items[4].Buyable = true;
        }



        if (!items[0].Buyable)
        {
            buttons[0].GetComponent<Image>().sprite = zoomSprites[2];
            buttons[0].GetComponent<Button>().interactable = false;
        }

        if (!items[1].Buyable)
        {
            buttons[1].GetComponent<Image>().sprite = lambdaSprites[2];
            buttons[1].GetComponent<Button>().interactable = false;
        }
        if (!items[2].Buyable)
        {
            buttons[2].GetComponent<Image>().sprite = powerSprites[2];
            buttons[2].GetComponent<Button>().interactable = false;
        }
        if (!items[3].Buyable)
        {
            buttons[3].GetComponent<Image>().sprite = rotationSprites[2];
            buttons[3].GetComponent<Button>().interactable = false;
        }
        if (!items[4].Buyable)
        {
            buttons[4].GetComponent<Image>().sprite = swapSprites[2];
            buttons[4].GetComponent<Button>().interactable = false;
        }
        
        
        if (items[0].Sold)
            buttons[0].GetComponent<Image>().sprite = zoomSprites[1];
        if (items[1].Sold)
            buttons[1].GetComponent<Image>().sprite = lambdaSprites[1];
        if (items[2].Sold)
            buttons[2].GetComponent<Image>().sprite = powerSprites[1];
        if (items[3].Sold)
            buttons[3].GetComponent<Image>().sprite = rotationSprites[1];
        if (items[4].Sold)
            buttons[4].GetComponent<Image>().sprite = swapSprites[1];
        
        

    }

    private void DisplayInfo()
    {
        if (currentItem == -1) return;
        
        zoomShopButton.GetComponent<Button>().interactable = false;
        lambdaShopButton.GetComponent<Button>().interactable = false;
        powerShopButton.GetComponent<Button>().interactable = false;
        rotationShopButton.GetComponent<Button>().interactable = false;
        swapShopButton.GetComponent<Button>().interactable = false;
        prevPageButton.SetActive(false);
        nextPageButton.SetActive(true);
        switch (currentItem)
        {
            case 0: 
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = zoomInfoSprites[0];
                if (zoomInfoSprites.Length == 1)
                    nextPageButton.SetActive(false);
                break;
            case 1: 
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = lambdaInfoSprites[0];
                if (lambdaInfoSprites.Length == 1)
                    nextPageButton.SetActive(false);
                break;
            case 2: 
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = powerInfoSprites[0];
                if (powerInfoSprites.Length == 1)
                    nextPageButton.SetActive(false);
                break;
            case 3:
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = rotationInfoSprites[0];
                if (rotationInfoSprites.Length == 1)
                    nextPageButton.SetActive(false);
                break;
            case 4:
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = swapInfoSprites[0];
                if (swapInfoSprites.Length == 1)
                    nextPageButton.SetActive(false);
                break;

        }
        currentInfoPage = 0;

        //infoPanel.transform.GetChild(0).GetComponent<Text>().text = items[currentItem].Info;
        infoPanel.SetActive(true);
        backButton.SetActive(false);
    }

    private void NextPage()
    {
        prevPageButton.SetActive(true);
        
        switch (currentItem)
        {
            case 0:
                if (currentInfoPage < zoomInfoSprites.Length-1)
                {
                    infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = zoomInfoSprites[++currentInfoPage];
                    if (currentInfoPage == zoomInfoSprites.Length-1)
                        nextPageButton.SetActive(false);
                }

                break;
            case 1:
                if (currentInfoPage < lambdaInfoSprites.Length-1)
                {
                    infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = lambdaInfoSprites[++currentInfoPage];
                    if (currentInfoPage == lambdaInfoSprites.Length-1)
                        nextPageButton.SetActive(false);
                }
                break;
            case 2:
                if (currentInfoPage < powerInfoSprites.Length-1)
                {
                    infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = powerInfoSprites[++currentInfoPage];
                    if (currentInfoPage == powerInfoSprites.Length-1)
                        nextPageButton.SetActive(false);
                }
                break;
            case 3:
                if (currentInfoPage < rotationInfoSprites.Length-1)
                {
                    infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = rotationInfoSprites[++currentInfoPage];
                    if (currentInfoPage == rotationInfoSprites.Length-1)
                        nextPageButton.SetActive(false);
                }
                break;
            
            case 4:
                if (currentInfoPage < swapInfoSprites.Length-1)
                {
                    infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = swapInfoSprites[++currentInfoPage];
                    if (currentInfoPage == swapInfoSprites.Length-1)
                        nextPageButton.SetActive(false);
                }
                break;
        }
    }
    
    private void PreviousPage()
    {
        
        if (currentInfoPage == 0) return;
        nextPageButton.SetActive(true);

        currentInfoPage--;
        if (currentInfoPage == 0) 
            prevPageButton.SetActive(false);
        switch (currentItem)
        {
            case 0:
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = zoomInfoSprites[currentInfoPage];
                break;
            case 1: 
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = lambdaInfoSprites[currentInfoPage];
                break;
            case 2: 
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = powerInfoSprites[currentInfoPage];
                break;
            case 3:
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = rotationInfoSprites[currentInfoPage];
                break;
            case 4:
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = swapInfoSprites[currentInfoPage];
                break;

        }
    }
    
    private void CloseInfo()
    {
        if (infoPanel.activeSelf == false) return;
        zoomShopButton.GetComponent<Button>().interactable = true;
        lambdaShopButton.GetComponent<Button>().interactable = true;
        powerShopButton.GetComponent<Button>().interactable = true;
        rotationShopButton.GetComponent<Button>().interactable = true;
        swapShopButton.GetComponent<Button>().interactable = true;

        infoPanel.SetActive(false);
        backButton.SetActive(true);
    }
    
    public void DisplayItem(int id)
    {
        
        currentItem = id;
       
        textBG.transform.GetChild(0).GetComponent<Text>().text = items[id].Description;
        textBG.transform.GetChild(1).gameObject.SetActive(true);
        textBG.transform.GetChild(1).GetComponent<Text>().text = items[id].Price.ToString();
        textBG.transform.GetChild(3).gameObject.SetActive(true);
        textBG.transform.GetChild(4).gameObject.SetActive(true);
    }

    public void Buy()
    {
        if (currentItem == -1) return;
        //Debug.Log(id + ": " + items[id].Sold);
        if (items[currentItem].Sold) return;
        

        if (ScoreManager.Score < items[currentItem].Price)
        {
            textBG.transform.GetChild(0).GetComponent<Text>().text = "Not enough points!";
            textBG.transform.GetChild(1).gameObject.SetActive(false);

            return;
        }
        ScoreManager.Score -= items[currentItem].Price;
        score.GetComponent<ScoreDisplay>().DisplayScore();
        switch (currentItem)
        {
            case 0: buttons[0].GetComponent<Image>().sprite = zoomSprites[1];
                PowerUpsManger.ZoomUnlocked = true;
                ShopItemsData.ShopItems[0].Sold = true;
                break;
            case 1: buttons[1].GetComponent<Image>().sprite = lambdaSprites[1];
                PowerUpsManger.LambdaUnlocked = true;
                ShopItemsData.ShopItems[1].Sold = true;
                break;
            case 2: buttons[2].GetComponent<Image>().sprite = powerSprites[1];
                PowerUpsManger.PowerUnlocked = true;
                ShopItemsData.ShopItems[2].Sold = true;
                break;
            case 3: buttons[3].GetComponent<Image>().sprite = rotationSprites[1];
                PowerUpsManger.RotationUnlocked = true;
                ShopItemsData.ShopItems[3].Sold = true;
                break;
            case 4: buttons[4].GetComponent<Image>().sprite = swapSprites[1];
                PowerUpsManger.SwapUnlocked = true;
                ShopItemsData.ShopItems[4].Sold = true;
                break;
        }
        
    }

}