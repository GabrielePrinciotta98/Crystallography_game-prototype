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
    }

    
}
