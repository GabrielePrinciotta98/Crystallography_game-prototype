using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomRepSolution : MonoBehaviour
{
    private SolutionManager solutionManager;
    private CentralCellSolution centralCellSolution;
    private Transform atomFather;

    private void Start()
    {
        atomFather = transform.parent;
        solutionManager = FindObjectOfType<SolutionManager>();
        centralCellSolution = FindObjectOfType<CentralCellSolution>();
    }


    void Update()
    {
        if (!solutionManager.GetCrystal()) return;
        if (!solutionManager.GetStop())
        {
            transform.parent = centralCellSolution.transform;
        }
        else
        {
            transform.parent = atomFather;
        }
    }
    
}
