using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CentralCell : MonoBehaviour
{
    private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private AtomsManager atomManager;
    public GameObject pivot;
    public Atom atom;
    private Vector3 pivotPos;
    private Atom[] centralCellAtoms = new Atom[9];
    private Vector3 curPos;
    private float rotationAngle;

    public float RotationAngle
    {
        get => rotationAngle;
        set => rotationAngle = value;
    }
    
    void Awake()
    {

        pivotPos = pivot.transform.position; 
        atomManager = FindObjectOfType<AtomsManager>();
        for (float x = -1.5f; x < 2f; x+=1.5f)
            for (float y = -1.5f; y < 2f; y+=1.5f)
            {
                if (x != 0 || y != 0)
                {
                    atomsSpawnPositions.Add(atomManager.GetK() > 5
                        ? new Vector3(0, y * (atomManager.GetK() - 5), x * (atomManager.GetK() - 5))
                        : new Vector3(0, y, x));
                }
            }
    }

    void Start()
    {
        curPos = transform.localPosition;

        pivot = Instantiate(pivot, pivotPos, Quaternion.identity);
        InstantiateAtoms();
    }

    public void InstantiateAtoms()
    {
        
        for (int i = 0; i < atomManager.GetN() - 1; i++)
        {
            centralCellAtoms[i] = Instantiate(atom, pivotPos + atomsSpawnPositions[i], Quaternion.identity, pivot.transform);
            
        }
    }
    
    private void Update()
    {
        if (atomManager.GetStop()) return;
        Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);
        transform.rotation = rotation;
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * (point - pivot) + pivot;
    }

    public Atom[] GetAtoms()
    {
        return centralCellAtoms;
    }

    public void Rotate(float angle)
    {
        
        transform.position = curPos;
        transform.RotateAround(pivotPos, Vector3.up, angle);
        curPos = transform.position;
    }
}


