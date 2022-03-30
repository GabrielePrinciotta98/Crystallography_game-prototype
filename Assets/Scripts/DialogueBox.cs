using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    private MoleculeManager moleculeManager;
    private new bool tag;
    private bool keyDown;
    
    private Text dialogue;
    private HUDManager hudManager;
    // Start is called before the first frame update
    void Start()
    {
        moleculeManager = FindObjectOfType<MoleculeManager>();
        hudManager = FindObjectOfType<HUDManager>();
        dialogue = GetComponentInChildren<Text>();
        dialogue.text = string.Empty;
        gameObject.SetActive(false);
        Debug.Log("dialogue box started");
    }
    
    public void StartShiftButtonTutorial()
    {
        Debug.Log(TutorialData.ShiftButtonTutorial);
        if (!TutorialData.firstTimeLevel7) return;
        Debug.Log("gameobject: " + gameObject);
        gameObject.SetActive(true);
        StartCoroutine(BuildText(TutorialData.ShiftButtonTutorial));
        TutorialData.firstTimeLevel7 = false;
    }
    
    public void StartFinalLevelInterference()
    {
        gameObject.SetActive(true);
        StartCoroutine(BuildText(TutorialData.FinalLevelInterference));
    }
    
    public void StartMoleculeModeImpossibleAfterHint()
    {
        gameObject.SetActive(true);
        StartCoroutine(BuildText(TutorialData.MoleculeModeImpossibleAfterHint));
    }

    public void StartAlertMoleculeModeImpossibleAfterHint()
    {
        if (moleculeManager.Activated) return;
        gameObject.SetActive(true);
        StartCoroutine(BuildText(TutorialData.AlertMoleculeModeImpossibleAfterHint));
    }
    
    /*  VERSIONE LETTERA PER LETTERA
    private IEnumerator BuildText(string[] dialogues)
    {
        hudManager.DisablePowerUps();
        foreach (var sentence in dialogues)
        {
            dialogue.text = string.Empty;
            foreach (var character in sentence)
            {
                Debug.Log(character);
                if (character == '<') tag = true;
                if (!tag)
                    dialogue.text = string.Concat(dialogue.text, character);
                else
                {
                    if (character == '>') tag = false;
                    continue;
                }

                yield return new WaitForSeconds(0.02f);
            }
            dialogue.text = sentence;
            //wait for input
            yield return new WaitWhile(() => !Input.anyKeyDown);
        }
        
        gameObject.SetActive(false);
        hudManager.EnablePowerUps();
    }
    */
    private IEnumerator BuildText(string[] dialogues)
    {
        hudManager.DisablePowerUps();
        foreach (var sentence in dialogues)
        {
            dialogue.text = sentence;
            //wait for input
            yield return new WaitForSeconds(0.016f);
            yield return new WaitWhile(() => !keyDown);
        }
        
        gameObject.SetActive(false);
        hudManager.EnablePowerUps();
    }

    private void Update()
    {
        keyDown = Input.anyKeyDown;
    }
}
