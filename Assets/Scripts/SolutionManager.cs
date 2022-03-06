using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{
    private bool isCrystal = true;
    private GameObject moleculeSpace;
    [SerializeReference] GameObject atom;
    [SerializeReference] GameObject pivot;
    [SerializeReference] GameObject centralCell;
    [SerializeReference] GameObject cell;
    [SerializeReference] private GameObject[] platforms;
    [SerializeField] int N = 1;
    [SerializeField] int M = 1;
    [SerializeField] int K = 5;
    [SerializeField] int R = 4;
    private List<Vector3> cellsSpawnPositions = new List<Vector3>();
    private Vector3 centralCellSpawnPos;
    private Vector3[] atomSpawnPositions = new Vector3[8];
    List<SolutionAtom> atoms = new List<SolutionAtom>();
    private List<GameObject> cells = new List<GameObject>();
    Vector4[] positions = new Vector4[60];
    
    private GameObject workspace;
    public Quaternion antiRotation;

    public List<GameObject> AllAtoms { get; } = new List<GameObject>(); //tutti gli atomi (anche cristallo) meno IL pivot


    public string Plane { get; set; }

    
    private void Awake()
    {
        centralCellSpawnPos = pivot.transform.position;
        
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        workspace = GameObject.Find("WorkspaceSol");
        moleculeSpace = GameObject.Find("MoleculeSpaceSolution");
        
        
        if (K > 5)
        {
            for (int x = -K; x < 2 * K; x += K)
            for (int y = -K; y < 2 * K; y += K)
            for (int z = -K; z < 2 * K; z += K)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }
        else
        {
            //Debug.Log(R);

            CalculateCellsPositions();
        }
        
        if (isCrystal)
        {
            workspace = Instantiate(platforms[1], moleculeSpace.transform);
            centralCell = Instantiate(centralCell, centralCellSpawnPos, Quaternion.identity, moleculeSpace.transform);
            if (K >= 5)
                centralCell.transform.localScale *= K; // scalo la dimensione del modulo centrale
            else
                centralCell.transform.localScale *= 5;

            // SPAWN CELLE RIPETUTE
            SpawnRepeatedCells();
            
        }
        else
        {
            workspace = Instantiate(Plane.Equals("YZ") ? platforms[0] : platforms[1] , moleculeSpace.transform);
            //centralCellSpawnPos ha la posizione del pivot
            pivot = Instantiate(pivot, centralCellSpawnPos, Quaternion.identity, moleculeSpace.transform);
            for (int i = 0; i < N - 1; i++)
            {
                Instantiate(atom,
                    new Vector3(centralCellSpawnPos.x + atomSpawnPositions[i].x,
                        centralCellSpawnPos.y + atomSpawnPositions[i].y,
                        centralCellSpawnPos.z + atomSpawnPositions[i].z),
                    Quaternion.identity, moleculeSpace.transform);
            }
        }
    }

    private void CalculateCellsPositions()
    {
        
        if (R == 3)
        {
            for (float x = -5f; x < 10f; x += 5f)
            for (float y = -5f; y < 10f; y += 5f)
            for (float z = -5f; z < 10f; z += 5f)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }

        if (R == 5)
        {
            for (float x = -10f; x < 15f; x += 5f)
            for (float y = -10f; y < 15f; y += 5f)
            for (float z = -10f; z < 15f; z += 5f)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }

        if (R == 7)
        {
            for (float x = -15f; x < 20f; x += 5f)
            for (float y = -15f; y < 20f; y += 5f)
            for (float z = -15f; z < 20f; z += 5f)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }
    }

    private void SpawnRepeatedCells()
    {
        if (R != 1)
        {
            for (int i = 0; i < M - 1; i++)
            {
                var cellTemp = Instantiate(cell, centralCellSpawnPos + cellsSpawnPositions[i], Quaternion.identity,
                                                    centralCell.transform);
                cells.Add(cellTemp);
            }
        }
    }

    

    public void AddAtom(SolutionAtom atom)
    {
        atoms.Add(atom);
        positions[atoms.IndexOf(atom)] = atom.PositionFromPivot;

    }

    public void SetMyPosition(SolutionAtom atom)
    {
        positions[atoms.IndexOf(atom)] = atom.PositionFromPivot;
    }
    
    
    public Vector4[] GetPositions()
    {
        return positions;
    }

    public List<SolutionAtom> GetAtoms()
    {
        return atoms;
    }
    

    public int GetN()
    {
        return N;
    }

    public void SetN(int n)
    {
        N = n;
    }
    public int GetM()
    {
        return M;
    }

    public void SetM(float m)
    {
        M = (int)m;
        FindObjectOfType<SolutionDetector>().SetDirty();
    }
    public int GetK()
    {
        return K;
    }
    
    public Vector3 GetCellForward()
    {
        return centralCell.transform.forward;
    }

    public Vector3 GetCellRight()
    {
        return centralCell.transform.right;
    }
    
    public int GetR()
    {
        return R;
    }

    public void SetR(float r)
    {
        R = (int)r;
        FindObjectOfType<SolutionDetector>().SetDirty();
    }
    
    
    public void Rotate(float degrees)
    {
        moleculeSpace.transform.rotation = Quaternion.Euler(0, degrees, 0);
        antiRotation = Quaternion.Euler(0, -degrees, 0);
        FindObjectOfType<SolutionDetector>().SetDirty();
    }
    
    public bool GetCrystal()
    {
        return isCrystal;
    }

    public void SetCrystal(bool flag)
    {
        isCrystal = flag;
    }

    public void SetAtomSpawnPositions(Vector3[] listPos)
    {
        this.atomSpawnPositions = listPos;
    }

    public Vector3[] GetAtomSpawnPositions()
    {
        return atomSpawnPositions;
    }

    public void ChangeToSymmetric()
    {
        foreach (var atom in atoms)
            atom.transform.localPosition *= -1;

        for (var i = 0; i < positions.Length; i++)
            positions[i] *= -1;
    }

    public GameObject GetPivot()
    {
        return pivot;
    }
    /*
    public void UpdateCells()
    {
        foreach (var cell in cells)
        {
            cell.GetComponent<CellSolution>().DestroyAtoms();
            Destroy(cell);
        }
        
        cells = new List<GameObject>();
        CalculateCellsPositions();
        SpawnRepeatedCells();
    }
    */

    public void AddAtomPositionToAll(GameObject atom)
    {
        //Debug.Log(atom.transform.position);
        AllAtoms.Add(atom);
        //Debug.Log("allAtoms.count: " + AllAtoms.Count);
    }
}
