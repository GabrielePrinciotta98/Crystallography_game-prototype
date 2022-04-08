using UnityEngine;

public class AtomRepSolution : MonoBehaviour
{
    private SolutionManager solutionManager;
    private Transform atomFather;

    private void Start()
    {
        atomFather = transform.parent;
        solutionManager = FindObjectOfType<SolutionManager>();
        solutionManager.AddAtomPositionToAll(gameObject);

    }


    void Update()
    {
        if (!solutionManager.GetCrystal()) return;
        transform.parent = atomFather;
    }
    
}
