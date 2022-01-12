using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSolution : MonoBehaviour
{
    private SolutionManager solutionManager;

    public GameObject atom;
    public GameObject pivotRep;
    public GameObject pivot;
    public CentralCellSolution centralCell;
    private Vector3 pivotPos;
    private SolutionAtom[] centralCellAtoms;
    private Vector3[] atomSpawnPositions = new Vector3[8];
    private List<GameObject> atoms = new List<GameObject>();


    void Awake()
    {
        solutionManager = FindObjectOfType<SolutionManager>();
        centralCell = FindObjectOfType<CentralCellSolution>();
        
    }
    
    private void Start()
    {
        pivotPos = transform.position;
        pivot = centralCell.GetPivot();
        centralCellAtoms = centralCell.GetAtoms();
        var pivotTemp = Instantiate(pivotRep, pivotPos, Quaternion.identity, centralCell.transform);
        pivotTemp.transform.localScale /= solutionManager.GetK();
        atoms.Add(pivotTemp);
        InstantiateAtoms();
    }

    private void InstantiateAtoms()
    {
        atomSpawnPositions = solutionManager.GetAtomSpawnPositions();

        for (int i = 0; i < solutionManager.GetN()-1; i++)
        {
            if (atomSpawnPositions[i] != Vector3.zero)
            {
                var atomTemp = Instantiate(atom, pivotPos + atomSpawnPositions[i],
                                                    Quaternion.identity, centralCellAtoms[i].transform);
                atoms.Add(atomTemp);
            }
        }
    }
    
    public void DestroyAtoms()
    {
        foreach (var atom in atoms)
        {
            Destroy(atom);
        } 
    }
}
