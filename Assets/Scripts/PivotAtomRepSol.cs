using UnityEngine;

public class PivotAtomRepSol : MonoBehaviour
{
    private SolutionManager solutionManager;

    void Start()
    {
        solutionManager = FindObjectOfType<SolutionManager>();
        solutionManager.AddAtomPositionToAll(gameObject);
    }
    
}
