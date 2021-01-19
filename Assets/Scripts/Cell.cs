using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private AtomsManager atomManager;
    public AtomRep atom;
    public GameObject pivotRep;
    public CentralCell centralCell;
    private Vector3 pivotPos;
    private Atom[] centralCellAtoms;
    
    void Awake()
    {
        atomManager = GameObject.FindObjectOfType<AtomsManager>();
        centralCell = GameObject.FindObjectOfType<CentralCell>();
        for (float x = -1.5f; x < 2f; x+=1.5f)
            for (float y = -1.5f; y < 2f; y+=1.5f)
                if (x != 0 || y != 0)
                    atomsSpawnPositions.Add(atomManager.GetK() > 5
                        ? new Vector3(0, y * (atomManager.GetK() - 5), x * (atomManager.GetK() - 5))
                        : new Vector3(0, y, x));
    }
    
    private void Start()
    {
        pivotPos = this.transform.position;
        centralCellAtoms = centralCell.GetAtoms();
        
        Instantiate(pivotRep, pivotPos, Quaternion.identity, centralCell.transform).transform.localScale /= atomManager.GetK();
        InstantiateAtoms();
    }

    public void InstantiateAtoms()
    {
        for (int i = 0; i < atomManager.GetN() - 1; i++)
        {
            if (atomsSpawnPositions[i] != Vector3.zero)
                Instantiate(atom, pivotPos + atomsSpawnPositions[i],
                    Quaternion.identity, centralCellAtoms[i].transform);
        }
    }
    

    
}
