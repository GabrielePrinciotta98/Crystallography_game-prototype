using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotAtomRepSol : MonoBehaviour
{
    private SolutionManager solutionManager;

    // Start is called before the first frame update
    void Start()
    {
        solutionManager = FindObjectOfType<SolutionManager>();
        solutionManager.AddAtomPositionToAll(gameObject);
    }
    
}
