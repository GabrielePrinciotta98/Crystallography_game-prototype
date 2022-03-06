using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralCellSolution : MonoBehaviour
{
    private SolutionManager solutionManager;
    public GameObject pivot;
    public SolutionAtom atom;
    private Vector3 pivotPos;
    private SolutionAtom[] centralCellAtoms = new SolutionAtom[9];
    private float rotationAngle;
    private GameObject moleculeSpace;
    
    
    private Vector3[] atomSpawnPositions = new Vector3[8];

    void Awake()
    {
        
        
        pivotPos = pivot.transform.position;
        solutionManager = FindObjectOfType<SolutionManager>();
        moleculeSpace = GameObject.Find("MoleculeSpaceSolution");

    }

    void Start()
    {
        pivot = Instantiate(pivot, pivotPos, Quaternion.identity, moleculeSpace.transform);
        InstantiateAtoms();
    }

    public void InstantiateAtoms()
    {
        atomSpawnPositions = solutionManager.GetAtomSpawnPositions();
        for (int i = 0; i < solutionManager.GetN()-1; i++)
        {
            centralCellAtoms[i] = Instantiate(atom, pivotPos + atomSpawnPositions[i], Quaternion.identity, pivot.transform);
            
        }
    }
    
    private void Update()
    {
        //if (solutionManager.GetStop()) return;
        //Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);
        //transform.rotation = rotation;

    }

    public SolutionAtom[] GetAtoms()
    {
        return centralCellAtoms;
    }

    public GameObject GetPivot()
    {
        return pivot;
    }
    
}
