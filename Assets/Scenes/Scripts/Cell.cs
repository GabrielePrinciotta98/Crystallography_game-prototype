using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private AtomsManager atomManager;
    public AtomRep atom;
    public GameObject pivot;
    public GameObject pivotRep;
    public CentralCell centralCell;
    private Vector3 pivotPos;
    private Vector3 rotationPoint = new Vector3(20f, 6.6f, 10f);
    private Atom[] centralCellAtoms;
    
    void Awake()
    {
        atomManager = GameObject.FindObjectOfType<AtomsManager>();
        centralCell = GameObject.FindObjectOfType<CentralCell>();
        for (float x = -1.5f; x < 2f; x+=1.5f)
            for (float y = -1.5f; y < 2f; y+=1.5f)
            {
                if (x != 0 || y != 0)
                    atomsSpawnPositions.Add(new Vector3(0, y, x));
                
            }
    }
    
    private void Start()
    {
        
        pivotPos = this.transform.position;
        centralCellAtoms = centralCell.GetAtoms();
        
        Instantiate(pivotRep, pivotPos, Quaternion.identity);
        for (int i = 0; i < atomManager.GetN()-1; i++)
        {
            if (atomsSpawnPositions[i] != Vector3.zero)
                Instantiate(atom, pivotPos + atomsSpawnPositions[i],
                    Quaternion.identity, centralCellAtoms[i].transform);


        }
        //pivotRep = Instantiate(pivotRep, pivotPos, Quaternion.identity);
        //InstantiateAtoms();
    }
    /*
    public void InstantiateAtoms()
    {
        
        for (int i = 0; i < atomManager.GetN() - 1; i++)
        {
            Instantiate(atom, pivotPos + atomsSpawnPositions[i], Quaternion.identity, pivot.transform);
        }
    }
    */

    private void Update()
    {
        if (!atomManager.GetStop())
            transform.RotateAround(rotationPoint, Vector3.up, 10 * Time.fixedDeltaTime);
    }
}
