using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionAtom : MonoBehaviour
{
    private Vector3 rotationPoint = new Vector3(25f, 10f, -20f);
    SolutionManager solutionManager;
    private Vector3 curPos;
    private float rotationAngle;

    public float RotationAngle
    {
        get => rotationAngle;
        set => rotationAngle = value;
    }

    // Start is called before the first frame update
    void Awake()
    {
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();
        solutionManager.AddAtom(this);
       // Debug.Log("Soluzione: " + transform.position);

    }

    private void Start()
    {
        curPos = transform.localPosition;
    }

    private void Update()
    {
        if (!solutionManager.GetStop())
        {
            Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);
            transform.localPosition = Matrix4x4.Rotate(rotation).MultiplyPoint3x4(curPos);
        }
        else
        {
            curPos = transform.localPosition;
        }
        solutionManager.SetMyPosition(this);
    }
    
    
    
    
}
