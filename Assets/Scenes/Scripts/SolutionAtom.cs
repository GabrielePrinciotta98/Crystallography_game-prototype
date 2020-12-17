using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionAtom : MonoBehaviour
{

    SolutionManager solutionManager;
    // Start is called before the first frame update
    void Awake()
    {
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();
        solutionManager.AddAtom(this);
       // Debug.Log("Soluzione: " + transform.position);

    }

    public void DontShowAtom()
    {
        GetComponent<Renderer>().enabled = false;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        
    }

    internal void ShowAtom()
    {
        GetComponent<Renderer>().enabled = true;
        transform.GetChild(0).GetComponent<Renderer>().enabled = true;
    }
}
