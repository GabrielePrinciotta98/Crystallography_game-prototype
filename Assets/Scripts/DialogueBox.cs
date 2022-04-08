using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    private MoleculeManager moleculeManager;
    private HintArrow hintArrow;
    private new bool tag;
    private bool keyDown;
    
    private Text dialogue;
    private HUDManager hudManager;
    
    void Start()
    {
        moleculeManager = FindObjectOfType<MoleculeManager>();
        hintArrow = FindObjectOfType<HintArrow>();
        hudManager = FindObjectOfType<HUDManager>();
        dialogue = GetComponentInChildren<Text>();
        dialogue.text = string.Empty;
        ShowDialogBox(false);
    }
    
    public void StartShiftButtonTutorial()
    {
        if (!TutorialData.firstTimeLevel7) return;
        ShowDialogBox(true);
        StartCoroutine(BuildText(TutorialData.ShiftButtonTutorial));
        TutorialData.firstTimeLevel7 = false;
    }
    
    public void StartFinalLevelInterference()
    {
        StartCoroutine(BuildText(TutorialData.FinalLevelInterference));
    }
    
    public void StartMoleculeModeImpossibleAfterHint()
    {
        ShowDialogBox(true);
        StartCoroutine(BuildText(TutorialData.MoleculeModeImpossibleAfterHint));
    }
    public void StartHintImpossibleAfterMoleculeMode()
    {
        ShowDialogBox(true);
        StartCoroutine(BuildText(TutorialData.HintImpossibleWhileMoleculeMode));
    }

    public void StartAlertMoleculeModeImpossibleAfterHint()
    {
        if (moleculeManager.Activated) return;
        ShowDialogBox(true);
        StartCoroutine(BuildText(TutorialData.AlertMoleculeModeImpossibleAfterHint));
    }
    
    public void StartAlertHintImpossibleAfterMoleculeMode()
    {
        if (hintArrow.activated) return;
        ShowDialogBox(true);
        StartCoroutine(BuildText(TutorialData.AlertHintImpossibleAfterMoleculeMode));
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
        
        ShowDialogBox(false);
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
