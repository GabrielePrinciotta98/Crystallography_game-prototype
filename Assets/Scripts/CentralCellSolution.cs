using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralCellSolution : MonoBehaviour
{
    private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private SolutionManager solutionManager;
    public GameObject pivot;
    public SolutionAtom atom;
    private Vector3 pivotPos;
    private SolutionAtom[] centralCellAtoms = new SolutionAtom[9];
    private Vector3 curPos;

    void Awake()
    {
        curPos = transform.position;
        
        pivotPos = pivot.transform.position;
        solutionManager = FindObjectOfType<SolutionManager>();
        for (float x = -1.5f; x < 2f; x+=1.5f)
        for (float y = -1.5f; y < 2f; y+=1.5f)
        {
            if (x != 0 || y != 0)
            {
                atomsSpawnPositions.Add(solutionManager.GetK() > 5
                    ? new Vector3(0, y * (solutionManager.GetK() - 5), x * (solutionManager.GetK() - 5))
                    : new Vector3(0, y, x));
            }
        }
    }

    void Start()
    {
        pivot = Instantiate(pivot, pivotPos, Quaternion.identity);
        InstantiateAtoms();
    }

    public void InstantiateAtoms()
    {
        
        for (int i = 0; i < solutionManager.GetN()-1; i++)
        {
            centralCellAtoms[i] = Instantiate(atom, pivotPos + atomsSpawnPositions[i], Quaternion.identity, pivot.transform);
            
        }
    }
    
    private void Update()
    {
        if (!solutionManager.GetStop())
            transform.RotateAround(pivotPos, Vector3.up, 10 * Time.deltaTime);
    }

    public SolutionAtom[] GetAtoms()
    {
        return centralCellAtoms;
    }

    public GameObject GetPivot()
    {
        return pivot;
    }
    
    public void Rotate(float angle)
    {
        transform.position = curPos;
        transform.RotateAround(pivotPos, Vector3.up, angle * Time.deltaTime);
        curPos = transform.position;
    }
}
