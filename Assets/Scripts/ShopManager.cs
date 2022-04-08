using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private AudioManager audioManager;
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
    public Sprite[] moleculeInfoSprites;
    
    private GameObject zoomShopButton;
    private GameObject lambdaShopButton;
    private GameObject powerShopButton;
    private GameObject rotationShopButton;
    private GameObject swapShopButton;
    private GameObject moleculeShopButton;
    
    public Sprite[] zoomSprites;
    public Sprite[] lambdaSprites;
    public Sprite[] powerSprites;
    public Sprite[] rotationSprites;
    public Sprite[] swapSprites;
    public Sprite[] moleculeSprites;
    
    private int currentItem = -1;
    private int currentInfoPage;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        score = GameObject.Find("Score");
        score.GetComponent<ScoreDisplay>().DisplayScore();
        backButton = GameObject.Find("BackButton");
        backButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadPrevScene);
        backButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

        
        buyButton = GameObject.Find("BuyButton");
        buyButton.GetComponent<Button>().onClick.AddListener(Buy);
        
        infoButton = GameObject.Find("InfoButton");
        infoButton.GetComponent<Button>().onClick.AddListener(DisplayInfo);
        infoButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

        closeInfoButton = GameObject.Find("CloseInfoButton");
        closeInfoButton.GetComponent<Button>().onClick.AddListener(CloseInfo);
        closeInfoButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

        nextPageButton = GameObject.Find("NextPageButton");
        nextPageButton.GetComponent<Button>().onClick.AddListener(NextPage);
        nextPageButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

        prevPageButton = GameObject.Find("PrevPageButton");
        prevPageButton.GetComponent<Button>().onClick.AddListener(PreviousPage);
        prevPageButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

        
        infoPanel = GameObject.Find("InfoPanel");
        infoPanel.SetActive(false);
        textBG = GameObject.Find("TextBG");
        textBG.transform.GetChild(1).gameObject.SetActive(false);
        textBG.transform.GetChild(3).gameObject.SetActive(false);
        textBG.transform.GetChild(4).gameObject.SetActive(false);
        textBG.transform.GetChild(5).gameObject.SetActive(false);

        // INIZIALIZZO I POWER UP DA COMPRARE   

        items = ShopItemsData.ShopItems.ToArray();
        

        // INIZIALIZZO I BOTTONI 
        buttons = new[]
        {
            zoomShopButton = GameObject.Find("ZoomShop"),
            lambdaShopButton = GameObject.Find("LambdaShop"),
            powerShopButton = GameObject.Find("PowerShop"),
            rotationShopButton = GameObject.Find("RotationShop"),
            swapShopButton = GameObject.Find("SwapShop"),
            moleculeShopButton = GameObject.Find("MoleculeShop"),
        };
        
        buttons[0].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(0); });
        buttons[0].GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        buttons[1].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(1); });
        buttons[1].GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        buttons[2].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(2); });
        buttons[2].GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        buttons[3].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(3); });
        buttons[3].GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        buttons[4].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(4); });
        buttons[4].GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        buttons[5].GetComponent<Button>().onClick.AddListener(delegate { DisplayItem(5); });
        buttons[5].GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        
        //VerifyBuyableItems();
        UpdateUnlockedItems();
        
        if (items[0].Sold || PowerUpsManager.ZoomUnlocked)
            buttons[0].GetComponent<Image>().sprite = zoomSprites[1];
        if (items[1].Sold || PowerUpsManager.LambdaUnlocked)
            buttons[1].GetComponent<Image>().sprite = lambdaSprites[1];
        if (items[2].Sold || PowerUpsManager.PowerUnlocked)
            buttons[2].GetComponent<Image>().sprite = powerSprites[1];
        if (items[3].Sold || PowerUpsManager.RotationUnlocked)
            buttons[3].GetComponent<Image>().sprite = rotationSprites[1];
        if (items[4].Sold || PowerUpsManager.SwapUnlocked)
            buttons[4].GetComponent<Image>().sprite = swapSprites[1];
        if (items[5].Sold || PowerUpsManager.MoleculeUnlocked)
            buttons[5].GetComponent<Image>().sprite = moleculeSprites[1];
        
    }

    private void UpdateUnlockedItems()
    {
        items[0].Unlocked = PowerUpsManager.ZoomUnlocked;
        items[1].Unlocked = PowerUpsManager.LambdaUnlocked;
        items[2].Unlocked = PowerUpsManager.PowerUnlocked;
        items[3].Unlocked = PowerUpsManager.RotationUnlocked;
        items[4].Unlocked = PowerUpsManager.SwapUnlocked;
        items[5].Unlocked = PowerUpsManager.MoleculeUnlocked;
    }

    public void VerifyBuyableItems()
    {
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
            items[5].Buyable = true;
        }
        
        if (!items[0].Buyable)
        {
            buttons[0].GetComponent<Image>().sprite = zoomSprites[2];
            buttons[0].GetComponent<Button>().interactable = false;
        }
        else
        {
            buttons[0].GetComponent<Image>().sprite = zoomSprites[0];
            buttons[0].GetComponent<Button>().interactable = true;
        }

        if (!items[1].Buyable)
        {
            buttons[1].GetComponent<Image>().sprite = lambdaSprites[2];
            buttons[1].GetComponent<Button>().interactable = false;
        }
        else
        {
            buttons[1].GetComponent<Image>().sprite = lambdaSprites[0];
            buttons[1].GetComponent<Button>().interactable = true;
        }
        
        if (!items[2].Buyable)
        {
            buttons[2].GetComponent<Image>().sprite = powerSprites[2];
            buttons[2].GetComponent<Button>().interactable = false;
        }
        else
        {
            buttons[2].GetComponent<Image>().sprite = powerSprites[0];
            buttons[2].GetComponent<Button>().interactable = true;
        }
        
        if (!items[3].Buyable)
        {
            buttons[3].GetComponent<Image>().sprite = rotationSprites[2];
            buttons[3].GetComponent<Button>().interactable = false;
        }
        else
        {
            buttons[3].GetComponent<Image>().sprite = rotationSprites[0];
            buttons[3].GetComponent<Button>().interactable = true;
        }
        
        if (!items[4].Buyable)
        {
            buttons[4].GetComponent<Image>().sprite = swapSprites[2];
            buttons[4].GetComponent<Button>().interactable = false;
        }
        else
        {
            buttons[4].GetComponent<Image>().sprite = swapSprites[0];
            buttons[4].GetComponent<Button>().interactable = true;
        }
        
        if (!items[5].Buyable)
        {
            buttons[5].GetComponent<Image>().sprite = moleculeSprites[2];
            buttons[5].GetComponent<Button>().interactable = false;
        }
        else
        {
            buttons[5].GetComponent<Image>().sprite = moleculeSprites[0];
            buttons[5].GetComponent<Button>().interactable = true;
        }
    }

    private void DisplayInfo()
    {
        if (currentItem == -1) return;
        
        zoomShopButton.GetComponent<Button>().interactable = false;
        lambdaShopButton.GetComponent<Button>().interactable = false;
        powerShopButton.GetComponent<Button>().interactable = false;
        rotationShopButton.GetComponent<Button>().interactable = false;
        swapShopButton.GetComponent<Button>().interactable = false;
        moleculeShopButton.GetComponent<Button>().interactable = false;
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
            case 5:
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = moleculeInfoSprites[0];
                if (moleculeInfoSprites.Length == 1)
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
            case 5:
                if (currentInfoPage < moleculeInfoSprites.Length-1)
                {
                    infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = moleculeInfoSprites[++currentInfoPage];
                    if (currentInfoPage == moleculeInfoSprites.Length-1)
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
            case 5:
                infoPanel.transform.GetChild(2).GetComponent<Image>().sprite = moleculeInfoSprites[currentInfoPage];
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
        moleculeShopButton.GetComponent<Button>().interactable = true;
        infoPanel.SetActive(false);
        backButton.SetActive(true);
    }
    
    public void DisplayItem(int id)
    {
        currentItem = id;
        textBG.transform.GetChild(0).GetComponent<Text>().text = items[id].Description;
        if (ShopItemsData.ShopItems[currentItem].Sold == false && ShopItemsData.ShopItems[currentItem].Unlocked == false)
        {
            textBG.transform.GetChild(1).GetComponent<Text>().text = items[id].Price.ToString();
            textBG.transform.GetChild(1).gameObject.SetActive(true);
            textBG.transform.GetChild(3).gameObject.SetActive(true);
            textBG.transform.GetChild(5).gameObject.SetActive(false);
        }
        else
        {
            textBG.transform.GetChild(1).gameObject.SetActive(false);
            textBG.transform.GetChild(3).gameObject.SetActive(false);
            textBG.transform.GetChild(5).gameObject.SetActive(true);
        }
        textBG.transform.GetChild(4).gameObject.SetActive(true);
    }

    private void Buy()
    {
        if (currentItem == -1) return;
        if (items[currentItem].Sold) return;
        

        if (ScoreManager.Score < items[currentItem].Price)
        {
            textBG.transform.GetChild(0).GetComponent<Text>().text = "Non hai abbastanza punti!";
            textBG.transform.GetChild(1).gameObject.SetActive(false);
            audioManager.Play("MenuError");
            return;
        }
        ScoreManager.Score -= items[currentItem].Price;
        PlayerPrefs.SetInt("Score", ScoreManager.Score);
        score.GetComponent<ScoreDisplay>().DisplayScore();
        switch (currentItem)
        {
            case 0: buttons[0].GetComponent<Image>().sprite = zoomSprites[1];
                PowerUpsManager.ZoomUnlocked = true;
                PlayerPrefs.SetInt("ZoomUnlocked", PowerUpsManager.ZoomUnlocked ? 1 : 0);
                ShopItemsData.ShopItems[0].Sold = true;
                textBG.transform.GetChild(1).gameObject.SetActive(false);
                textBG.transform.GetChild(3).gameObject.SetActive(false);
                textBG.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 1: buttons[1].GetComponent<Image>().sprite = lambdaSprites[1];
                PowerUpsManager.LambdaUnlocked = true;
                PlayerPrefs.SetInt("LambdaUnlocked", PowerUpsManager.LambdaUnlocked ? 1 : 0);
                ShopItemsData.ShopItems[1].Sold = true;
                textBG.transform.GetChild(1).gameObject.SetActive(false);
                textBG.transform.GetChild(3).gameObject.SetActive(false);
                textBG.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 2: buttons[2].GetComponent<Image>().sprite = powerSprites[1];
                PowerUpsManager.PowerUnlocked = true;
                PlayerPrefs.SetInt("PowerUnlocked", PowerUpsManager.PowerUnlocked ? 1 : 0);
                ShopItemsData.ShopItems[2].Sold = true;
                textBG.transform.GetChild(1).gameObject.SetActive(false);
                textBG.transform.GetChild(3).gameObject.SetActive(false);
                textBG.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 3: buttons[3].GetComponent<Image>().sprite = rotationSprites[1];
                PowerUpsManager.RotationUnlocked = true;
                PlayerPrefs.SetInt("RotationUnlocked", PowerUpsManager.RotationUnlocked ? 1 : 0);
                
                ShopItemsData.ShopItems[3].Sold = true;
                textBG.transform.GetChild(1).gameObject.SetActive(false);
                textBG.transform.GetChild(3).gameObject.SetActive(false);
                textBG.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 4: buttons[4].GetComponent<Image>().sprite = swapSprites[1];
                PowerUpsManager.SwapUnlocked = true;
                PlayerPrefs.SetInt("SwapUnlocked", PowerUpsManager.SwapUnlocked ? 1 : 0);
                
                ShopItemsData.ShopItems[4].Sold = true;
                textBG.transform.GetChild(1).gameObject.SetActive(false);
                textBG.transform.GetChild(3).gameObject.SetActive(false);
                textBG.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 5: buttons[5].GetComponent<Image>().sprite = moleculeSprites[1];
                PowerUpsManager.MoleculeUnlocked = true;
                PlayerPrefs.SetInt("MoleculeUnlocked", PowerUpsManager.MoleculeUnlocked ? 1 : 0);
                ShopItemsData.ShopItems[5].Sold = true;
                textBG.transform.GetChild(1).gameObject.SetActive(false);
                textBG.transform.GetChild(3).gameObject.SetActive(false);
                textBG.transform.GetChild(5).gameObject.SetActive(true);
                break;
        }
        audioManager.Play("Selling");
        PlayerPrefs.Save();
    }

}
