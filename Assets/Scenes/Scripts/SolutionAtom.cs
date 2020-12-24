using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionAtom : MonoBehaviour
{
    private Vector3 rotationPoint = new Vector3(22f, 10f, -21f);
    SolutionManager solutionManager;
    // Start is called before the first frame update
    void Awake()
    {
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();
        solutionManager.AddAtom(this);
       // Debug.Log("Soluzione: " + transform.position);

    }

    private void Update()
    {
        if (!solutionManager.GetStop())
            transform.RotateAround(rotationPoint, Vector3.up, 10 * Time.fixedDeltaTime);
        solutionManager.SetMyPosition(this);
    }
    /*
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
    */
}
