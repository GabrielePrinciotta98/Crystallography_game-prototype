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
        ShowDialogBox(false);
        Debug.Log("dialogue box started");
    }
    
    public void StartShiftButtonTutorial()
    {
        Debug.Log(TutorialData.firstTimeLevel7);
        Debug.Log("StartShift started");
        Debug.Log(gameObject);
        Debug.Log("pippo");
        if (!TutorialData.firstTimeLevel7) return;
        ShowDialogBox(true);
        Debug.Log("ciao");
        StartCoroutine(BuildText(TutorialData.ShiftButtonTutorial));
        TutorialData.firstTimeLevel7 = false;
    }
    
    public void StartFinalLevelInterference()
    {
        if (gameObject == null)
            Debug.Log("NULLO");
        ShowDialogBox(true);
        Debug.Log("ciao");
        StartCoroutine(BuildText(TutorialData.FinalLevelInterference));
    }
    
    public void StartMoleculeModeImpossibleAfterHint()
    {
        ShowDialogBox(true);
        StartCoroutine(BuildText(TutorialData.MoleculeModeImpossibleAfterHint));
    }

    public void StartAlertMoleculeModeImpossibleAfterHint()
    {
        if (moleculeManager.Activated) return;
        ShowDialogBox(true);
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
        Debug.Log("BuildText started");
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

    private void ShowDialogBox(bool flag)
    {
        transform.GetChild(0).gameObject.SetActive(flag);
        transform.GetChild(1).gameObject.SetActive(flag);
    }
}
