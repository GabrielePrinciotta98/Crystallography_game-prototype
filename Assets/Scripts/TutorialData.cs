using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialData
{
    public static bool firstTimeLevel7 = true;
    
    public static readonly string[] ShiftButtonTutorial = 
    {
        "In questo livello gli atomi si possono muovere in tutte e tre le dimensioni X,Y e Z!",
        "Tenendo premuto il tasto <b>SHIFT</b>, mentre si trascina un atomo, si passa dal movimento orizzontale a quello verticale."
    };
    
    public static readonly string[] FinalLevelInterference = 
    {
        "C'è troppa interferenza!",
        "È impossibile usare la <color=#1758BA>modalità molecola</color> e i <color=#10D3CE>suggerimenti</color>!",
    };
    
    public static readonly string[] MoleculeModeImpossibleAfterHint = 
    {
        "Non puoi attivare o disattivare la <color=#1758BA>modalità molecola</color> dopo aver usato i <color=#10D3CE>suggerimenti</color>!"
    };
    
    public static readonly string[] AlertMoleculeModeImpossibleAfterHint = 
    {
        "Attenzione! Una volta usati i <color=#10D3CE>suggerimenti</color> la <color=#1758BA>modalità molecola</color> non sarà attivabile!",
        "È possibile attivarla prima di chiedere i <color=#10D3CE>suggerimenti</color>.",
        "Per attivare comunque i <color=#10D3CE>suggerimenti</color> prova ad usarli ancora!"
    };

}
