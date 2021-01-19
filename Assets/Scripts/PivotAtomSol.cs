using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotAtomSol : MonoBehaviour
{
    
    SolutionManager solutionManager;
    private Vector3 rotationPoint = new Vector3(25f, 10f, -20f);


    // Start is called before the first frame update
    void Start()
    {
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();

    }

    private void Update()
    {
        if (!solutionManager.GetStop())
            transform.RotateAround(rotationPoint, Vector3.up, 10 * Time.deltaTime);
        //Debug.Log("Atomo: " + transform.position);
    }
}
