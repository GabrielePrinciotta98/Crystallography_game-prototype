using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CheatCode : MonoBehaviour
{
    private ScoreDisplay scoreDisplay;
    private LevelSelectionManager lsManager;
    private ShopManager shopManager;
    private bool alreadyActivated;

    public bool AlreadyActivated => alreadyActivated;

    private void Start()
    {
        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        lsManager = FindObjectOfType<LevelSelectionManager>();
        shopManager = FindObjectOfType<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey("up") || !Input.GetKey("down") || alreadyActivated) return;
        alreadyActivated = true;
        print("cheatcode activated");
        LevelsUnlocked.NumberOfLevelsUnlocked = 11;
        ScoreManager.Score = 99999;
        if (scoreDisplay)
            scoreDisplay.DisplayScore();
        if (lsManager)
            lsManager.UpdateInteractable();
        if (shopManager)
            shopManager.VerifyBuyableItems();
        
    }
}
