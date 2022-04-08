using System.Collections.Generic;
using UnityEngine;

public class SolutionAtom : MonoBehaviour
{
    SolutionManager solutionManager;
    private float rotationAngle;
    public Vector3 PositionFromPivot { get; set; }
    public float distanceToMolecularParent;
    public GameObject molecularParent;
    public List<SolutionAtom> molecularChildren;

    void Awake()
    {
        solutionManager = FindObjectOfType<SolutionManager>();
        solutionManager.AddAtom(this);
        solutionManager.AddAtomPositionToAll(gameObject);

    }

    private void Update()
    {
        PositionFromPivot = transform.position - new Vector3(22, 6.6f, -20); 
        solutionManager.SetMyPosition(this);
    }

   
}
