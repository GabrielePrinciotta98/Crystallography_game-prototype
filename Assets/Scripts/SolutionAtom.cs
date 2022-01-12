using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionAtom : MonoBehaviour
{
    SolutionManager solutionManager;
    private Vector3 curPos;
    private float rotationAngle;
    public Vector3 PositionFromPivot { get; set; }
    
    // Start is called before the first frame update
    void Awake()
    {
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();
        solutionManager.AddAtom(this);
        solutionManager.AddAtomPositionToAll(gameObject);
       // Debug.Log("Soluzione: " + transform.position);

    }

    private void Start()
    {
        curPos = transform.localPosition;
    }

    private void Update()
    {
        PositionFromPivot = transform.position - new Vector3(22, 6.6f, -20); 
        //PositionFromPivot = transform.localPosition - Vector3.zero;
        solutionManager.SetMyPosition(this);
    }

   
}
