using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSolution : MonoBehaviour
{
    //private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private SolutionManager solutionManager;

    //public SolutionAtom atom;
    public AtomRepSolution atom;
    public GameObject pivotRep;
    public GameObject pivot;
    public CentralCellSolution centralCell;
    private Vector3 pivotPos;
    private SolutionAtom[] centralCellAtoms;
    private Vector3[] atomSpawnPositions = new Vector3[8];

    void Awake()
    {
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();
        centralCell = GameObject.FindObjectOfType<CentralCellSolution>();
        /*
        for (float x = -1.5f; x < 2f; x+=1.5f)
        for (float y = -1.5f; y < 2f; y+=1.5f)
            if (x != 0 || y != 0)
                atomsSpawnPositions.Add(solutionManager.GetK() > 5
                    ? new Vector3(0, y * (solutionManager.GetK() - 5), x * (solutionManager.GetK() - 5))
                    : new Vector3(0, y, x));
                    */
    }
    
    private void Start()
    {
        pivotPos = this.transform.position;
        pivot = centralCell.GetPivot();
        centralCellAtoms = centralCell.GetAtoms();
        
        Instantiate(pivotRep, pivotPos, Quaternion.identity, centralCell.transform).transform.localScale /= solutionManager.GetK();
        //Instantiate(atom, pivotPos, Quaternion.identity, pivot.transform);
        InstantiateAtoms();
    }

    public void InstantiateAtoms()
    {
        atomSpawnPositions = solutionManager.GetAtomSpawnPositions();

        //Debug.Log(pivot.transform);
        for (int i = 0; i < solutionManager.GetN()-1; i++)
        {
            if (atomSpawnPositions[i] != Vector3.zero)
                Instantiate(atom, pivotPos + atomSpawnPositions[i], Quaternion.identity, pivot.transform/*centralCellAtoms[i].transform*/);
        }
    }
}
