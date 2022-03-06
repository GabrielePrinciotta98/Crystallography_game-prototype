using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    
    private static readonly string[] FinalLevelInterference = 
    {
        "C'è troppa interferenza!",
        "È impossibile usare la modalità molecola e i suggerimenti!",
    };


    private Text dialogue;
    private HUDManager hudManager;
    // Start is called before the first frame update
    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();
        dialogue = GetComponentInChildren<Text>();
        dialogue.text = string.Empty;
        gameObject.SetActive(false);
    }
    

    public void StartFinalLevelInterference()
    {
        hudManager.DisablePowerUps();
        gameObject.SetActive(true);
        StartCoroutine(BuildText(FinalLevelInterference));
    }
    
    private IEnumerator BuildText(string[] dialogues)
    {
        foreach (var sentence in dialogues)
        {
            dialogue.text = string.Empty;
            foreach (var character in sentence)
            {
                Debug.Log(character);
                dialogue.text = string.Concat(dialogue.text, character);
                
                yield return new WaitForSeconds(0.02f);
            }
            dialogue.text = sentence;
            //wait for input
            yield return new WaitWhile(() => !Input.anyKeyDown);
        }
        
        gameObject.SetActive(false);
        hudManager.EnablePowerUps();
    }
}
