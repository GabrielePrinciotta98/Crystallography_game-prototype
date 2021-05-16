using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AtomsManager : MonoBehaviour
{
    private SolutionManager solutionManager;
    public bool isCrystal;

    public bool GameStart { get; set; }
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
    List<Atom> atoms = new List<Atom>();
    Vector4[] positions = new Vector4[20];


    private bool stop = true;
    private List<Vector3> cellsSpawnPositions;
    private Vector3 centralCellSpawnPos;
    private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private Vector3[] solutionSpawnPositions;
    private Atom draggingAtom;
    private List<GameObject> cells = new List<GameObject>();
    public string LevelType { get; set; }

    private void Awake()
    {
        centralCellSpawnPos = pivot.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        moleculeSpace = GameObject.Find("MoleculeSpace");
        Debug.Log("N: " + N);
        // POSIZIONI DI SPAWN DEGLI ATOMI
        GenerateRandomPositions(LevelType, N-1);
        /*
        if (!Plane.Equals("XZ"))
        {
            for (float x = -1.5f; x < 2f; x+=1.5f)
            for (float y = -1.5f; y < 2f; y+=1.5f)
                if (x != 0 || y != 0)
                    atomsSpawnPositions.Add(new Vector3(0, y, x));
        }
        else
        {
            for (float x = -1.5f; x < 2f; x+=1.5f)
            for (float y = -1.5f; y < 2f; y+=1.5f)
                if (x != 0 || y != 0)
                    atomsSpawnPositions.Add(new Vector3(y, 0, x));
        }
        */
        //POSIZIONI DI SPAWN DELLE CELLE 
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
            CalculateCellsPositions();
        }
        
        
        if (isCrystal)
        {
            Instantiate(platforms[1]);

            // SPAWN CELLA CENTRALE
            centralCell = Instantiate(centralCell, centralCellSpawnPos, Quaternion.identity);
            if (K >= 5)
                centralCell.transform.localScale *= K; // scalo la dimensione del modulo centrale
            else
                centralCell.transform.localScale *= 5;

            SpawnRepeatedCells();
        }
        else
        {
            Instantiate(LevelType.Equals("YZ") ? platforms[0] : platforms[1], moleculeSpace.transform);
            //centralCellSpawnPos ha la posizione del pivot
            pivot = Instantiate(pivot, centralCellSpawnPos, Quaternion.identity, moleculeSpace.transform);
            for (int i=0; i<N-1; i++)
                Instantiate(atom, new Vector3(centralCellSpawnPos.x + atomsSpawnPositions[i].x,
                        centralCellSpawnPos.y + atomsSpawnPositions[i].y,
                        centralCellSpawnPos.z + atomsSpawnPositions[i].z),
                    Quaternion.identity, moleculeSpace.transform);
        }
        

    }

    private void CalculateCellsPositions()
    {
        Debug.Log("R: " + R);
        cellsSpawnPositions = new List<Vector3>();

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
        // SPAWN CELLE RIPETUTE
        Debug.Log(M);
        if (R != 1)
            for (int i = 0; i < M - 1; i++)
            {
                var cellTemp = Instantiate(cell, centralCellSpawnPos + cellsSpawnPositions[i], Quaternion.identity,
                                                    centralCell.transform);
                cells.Add(cellTemp);
            }
    }


    public void FreezeAtoms()
    {
        foreach (var atom in atoms)
        {
            if (atom != draggingAtom)
                atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            else
            {
                if (LevelType.Equals("YZ"))
                    atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
                if (LevelType.Equals("XZ"))
                    atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            }
        }
    }

    public void UnFreezeAtoms()
    {
        foreach (var atom in atoms)
        {
            atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void SetDraggingAtom(Atom atom)
    {
        draggingAtom = atom;
    }

    public Atom GetDraggingAtom()
    {
        return this.draggingAtom;
    }
    
    public void AddAtom(Atom atom)
    {
        atoms.Add(atom);
        positions[atoms.IndexOf(atom)] = atom.transform.localPosition;
    }

    public List<Atom> GetAtoms()
    {
        return atoms;
    }

    public void SetMyPosition(Atom atom)
    {
        positions[atoms.IndexOf(atom)] = atom.PositionFromPivot;
    }
    

    public Vector4[] GetPositions()
    {
        return positions;
    } 

    public bool GetStop()
    {
        return stop;
    }

    public int GetN()
    {
        return N;
    }

    public void SetN(int n)
    {
        N = n;
        //centralCell.GetComponent<CentralCell>().InstantiateAtoms();
        //Debug.Log("setN: " + GetN());
    }
    
    public void SetN(Slider slider)
    {
        N = (int)slider.value;
        
        //Debug.Log("setN: " + GetN());
    }
    
    public int GetM()
    {
        return M;
    }
    
    public void SetM(float m)
    {
        M = (int)m;
        FindObjectOfType<Detector>().SetDirty();
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
    

    public void Rotate(float degrees)
    {
        moleculeSpace.transform.rotation = Quaternion.Euler(0, degrees, 0);
        FindObjectOfType<Detector>().SetDirty();
    }


    public int GetR()
    {
        return R;
    }

    public void SetR(float r)
    {
        R = (int)r;
        FindObjectOfType<Detector>().SetDirty();
    }

    public bool GetCrystal()
    {
        return isCrystal;
    }

    public void SetCrystal(bool flag)
    {
        isCrystal = flag;
    }
    
    private void GenerateRandomPositions(string plane, int n)
    {
        Vector3[] ris = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            bool isEqual = false;
            bool isPivot = false;
            bool isSpawn = false;

            Vector3 newPos = plane switch
            {
                "YZ" => new Vector3(0, Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
                "XZ" => new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)),
                "XYZ" => new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
                _ => Vector3.zero
            };
            for (int j = 0; j < i; j++)
            {
                if (Vector3.Distance(ris[j], newPos) > 1) continue;
                isEqual = true;
                break;
            }

            for (int j = 0; j < N-1; j++)
            {
                if (!(Vector3.Distance(newPos, solutionSpawnPositions[j]) < 1)) continue;
                isSpawn = true;
                break;
            }
            

            if (Vector3.Distance(newPos, Vector3.zero) <= 1)
                isPivot = true;
            
            if (isEqual || isPivot || isSpawn)
            {
                i--;
                continue;
            }
            ris[i] = newPos;
        }

        foreach (var v in ris)
        {
            atomsSpawnPositions.Add(v);
        } 
        
    }

    public void SetSolutionSpawnPositions(Vector3[] listPos)
    {
        solutionSpawnPositions = listPos;
    }

    public GameObject GetPivot()
    {
        return pivot;
    }

    public void UpdateCells()
    {
        foreach (var cell in cells)
        {
            cell.GetComponent<Cell>().DestroyAtoms();
            Destroy(cell);
        }

        cells = new List<GameObject>();
        CalculateCellsPositions();
        SpawnRepeatedCells();
    }
}
