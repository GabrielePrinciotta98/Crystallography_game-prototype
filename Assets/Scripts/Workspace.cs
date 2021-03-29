using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workspace : MonoBehaviour
{
    private Vector3 curPos;

    public float RotationAngle { get; set; }

    private AtomsManager atomsManager;

    private SolutionManager solutionManager;
    // Start is called before the first frame update
    void Start()
    {
        curPos = transform.localPosition;
    }

    // Update is called once per frame
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
