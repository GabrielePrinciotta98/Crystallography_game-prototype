using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CentralCell : MonoBehaviour
{
    private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private AtomsManager atomManager;
    public GameObject pivot;
    public Atom atom;
    private Vector3 pivotPos = new Vector3(20f, 6.6f, 10f);
    private Atom[] centralCellAtoms = new Atom[9];
    void Awake()
    {
        atomManager = GameObject.FindObjectOfType<AtomsManager>();
        for (float x = -1.5f; x < 2f; x+=1.5f)
            for (float y = -1.5f; y < 2f; y+=1.5f)
            {
                if (x != 0 || y != 0)
                {
                    atomsSpawnPositions.Add(new Vector3(0, y, x));
                }
            }
    }

    void Start()
    {
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
    /*
    public void InstantiateAtomsSlider(Slider slider)
    {
        
        Debug.Log("inst: " + slider.value);
        Debug.Log("spawnLength1: " + atomsSpawnPositions.Count);
        for (int i = 0; i < slider.value; i++)
        {
            Instantiate(atom, pivotPos + atomsSpawnPositions[i], Quaternion.identity, pivot.transform);
        }
    }
*/
    //TODO rotazione delle celle 
    private void Update()
    {
        if (!atomManager.GetStop())
            transform.RotateAround(pivotPos, Vector3.up, 10 * Time.fixedDeltaTime);
    }

    public Atom[] GetAtoms()
    {
        return centralCellAtoms;
    }
}


