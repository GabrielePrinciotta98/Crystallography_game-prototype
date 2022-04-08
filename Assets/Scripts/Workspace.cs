using UnityEngine;

public class Workspace : MonoBehaviour
{

    private float RotationAngle { get; set; }

    private AtomsManager atomsManager;

    private SolutionManager solutionManager;
    
    void Update()
    {
        if (!solutionManager && !atomsManager) return;
        Quaternion rotation = Quaternion.Euler(0, RotationAngle, 0);
        transform.rotation = rotation;
    }

    public void SetAtomsManager(AtomsManager am)
    {
        atomsManager = am;
    }

    public void SetSolutionManager(SolutionManager sm)
    {
        solutionManager = sm;
    }
}
