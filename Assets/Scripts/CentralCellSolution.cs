using UnityEngine;

public class CentralCellSolution : MonoBehaviour
{
    private Material mat;
    private SolutionManager solutionManager;
    public GameObject pivot;
    public SolutionAtom atom;
    private Vector3 pivotPos;
    private readonly SolutionAtom[] centralCellAtoms = new SolutionAtom[9];
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
        mat = GetComponent<Renderer>().material;
        mat.color = new Color(1, 1, 1, 0); 
        pivot = Instantiate(pivot, pivotPos, Quaternion.identity, moleculeSpace.transform);
        InstantiateAtoms();
    }

    private void InstantiateAtoms()
    {
        atomSpawnPositions = solutionManager.GetAtomSpawnPositions();
        for (int i = 0; i < solutionManager.GetN()-1; i++)
        {
            centralCellAtoms[i] = Instantiate(atom, pivotPos + atomSpawnPositions[i], Quaternion.identity, pivot.transform);
            
        }
    }
    

    public SolutionAtom[] GetAtoms()
    {
        return centralCellAtoms;
    }
    
    
}
