﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeReference] AtomsManager atomsManager;
    [SerializeReference] SolutionManager solutionManager;
    [SerializeReference] GameObject curtain;
    HashSet<Vector4> atomPositions;
    HashSet<Vector4> solutionPositions;
    MyEqualityComparer comparer = new MyEqualityComparer();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        

        atomPositions = new HashSet<Vector4>(atomsManager.GetPositions(), comparer);
        solutionPositions = new HashSet<Vector4>(solutionManager.GetPositions(), comparer);
        for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
        {
            if (Vector4.Distance(atomsManager.GetPositions()[i], solutionManager.GetPositions()[i]) < 2)
            {
                //curtain.SetActive(false);
                solutionManager.ShowAtoms();
                print("Hai vinto!");
            }
        }
        /*
        if (atomPositions.SetEquals(solutionPositions))
        {
            curtain.SetActive(false);
            print("Hai vinto!");
        }
        */
    }

    public void ShowSolution()
    {
        solutionManager.ShowAtoms();

    }
}
