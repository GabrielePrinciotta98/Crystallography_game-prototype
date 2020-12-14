using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeReference] AtomsManager atomsManager;
    [SerializeReference] SolutionManager solutionManager;
    [SerializeReference] GameObject curtain;
    HashSet<Vector4> atomPositions;
    HashSet<Vector4> solutionPositions;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        atomPositions = new HashSet<Vector4>(atomsManager.GetPositions());
        solutionPositions = new HashSet<Vector4>(solutionManager.GetPositions());
        
        if (atomPositions.SetEquals(solutionPositions))
        {
            curtain.SetActive(false);
            print("Hai vinto!");
        }
    }

    public void ShowSolution()
    {
        curtain.SetActive(false);
    }
}
