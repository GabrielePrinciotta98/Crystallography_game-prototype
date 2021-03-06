﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtomsManager : MonoBehaviour
{
    private bool isCrystal;

    public bool GameStart { get; set; }

    [SerializeReference] GameObject atom;
    [SerializeReference] GameObject pivot;
    [SerializeReference] GameObject centralCell;
    [SerializeReference] GameObject cell;
    [SerializeField] int N = 1;
    [SerializeField] int M = 1;
    [SerializeField] int K = 5;
    [SerializeField] int R = 4;
    //[SerializeField] int rows = 1;
    //[SerializeField] int columns = 1;
    //[SerializeField] int depth = 1;
    List<Atom> atoms = new List<Atom>();
    Vector4[] positions = new Vector4[20];
    private Vector4 sumPos;


    private bool stop = true;
    private List<Vector3> cellsSpawnPositions = new List<Vector3>();
    private Vector3 centralCellSpawnPos;
    private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private Atom draggingAtom;

    public string Plane { get; set; }

    public bool AnAtomIsMoving { get; set; }

    private void Awake()
    {
        centralCellSpawnPos = pivot.transform.position;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        // POSIZIONI DI SPAWN DEGLI ATOMI
        if (!Plane.Equals("XZ"))
        {
            for (float x = -1.5f; x < 2f; x+=1.5f)
            for (float y = -1.5f; y < 2f; y+=1.5f)
                if (x != 0 || y != 0)
                    atomsSpawnPositions.Add(new Vector3(0, y, x));
        }
        else
        {
            for (float x = -1.5f; x < 2f; x+=1.5f)
            for (float y = -1.5f; y < 2f; y+=1.5f)
                if (x != 0 || y != 0)
                    atomsSpawnPositions.Add(new Vector3(y, 0, x));
        }
        
        //POSIZIONI DI SPAWN DELLE CELLE 
        if (K > 5)
        {
            for (int x = -K; x < 2 * K; x += K)
            for (int y = -K; y < 2 * K; y += K)
            for (int z = -K; z < 2 * K; z += K)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }
        else
        {
            Debug.Log("R: " + R);

            if (R == 3)
            {
                for (float x = -5f; x < 10f; x += 5f)
                for (float y = -5f; y < 10f; y += 5f)
                for (float z = -5f; z < 10f; z += 5f)
                    if (x != 0 || y != 0 || z != 0)
                        cellsSpawnPositions.Add(new Vector3(x, y, z));
            }
            if (R == 5)
            {
                for (float x = -10f; x < 15f; x += 5f)
                for (float y = -10f; y < 15f; y += 5f)
                for (float z = -10f; z < 15f; z += 5f)
                    if (x != 0 || y != 0 || z != 0)
                        cellsSpawnPositions.Add(new Vector3(x, y, z));
            }
            if (R == 7)
            {
                for (float x = -15f; x < 20f; x += 5f)
                for (float y = -15f; y < 20f; y += 5f)
                for (float z = -15f; z < 20f; z += 5f)
                    if (x != 0 || y != 0 || z != 0)
                        cellsSpawnPositions.Add(new Vector3(x, y, z));
            }
        }
        
        
        
        if (isCrystal)
        {
            // SPAWN CELLA CENTRALE
            centralCell = Instantiate(centralCell, centralCellSpawnPos, Quaternion.identity);
            if (K >= 5)
                centralCell.transform.localScale *= K; // scalo la dimensione del modulo centrale
            else
                centralCell.transform.localScale *= 5;

            // SPAWN CELLE RIPETUTE
            //Debug.Log(M);
            
            for (int i = 0; i < M - 1; i++)
                Instantiate(cell, centralCellSpawnPos + cellsSpawnPositions[i], Quaternion.identity,
                    centralCell.transform);
        }
        else
        {
            //centralCellSpawnPos ha la posizione del pivot
            pivot = Instantiate(pivot, centralCellSpawnPos, Quaternion.identity);
            for (int i=0; i<N-1; i++)
                Instantiate(atom, new Vector3(centralCellSpawnPos.x + atomsSpawnPositions[i].x,
                        centralCellSpawnPos.y + atomsSpawnPositions[i].y,
                        centralCellSpawnPos.z + atomsSpawnPositions[i].z),
                    Quaternion.identity, pivot.transform);
        }
    }

    public void UnsetHoveredAtom(Atom hovered)
    {
        foreach (var atom in atoms)
        {
            if (atom != hovered)
                atom.LastHovered = false;
        }
    }
    
    public void FreezeAtoms()
    {
        foreach (var atom in atoms)
        {
            if (atom != draggingAtom)
                atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void UnFreezeAtoms()
    {
        foreach (var atom in atoms)
        {
            atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void SetDraggingAtom(Atom atom)
    {
        draggingAtom = atom;
    }

    public Atom GetDraggingAtom()
    {
        return this.draggingAtom;
    }
    
    public void AddAtom(Atom atom)
    {
        atoms.Add(atom);
        positions[atoms.IndexOf(atom)] = atom.transform.localPosition;
    }

    public List<Atom> GetAtoms()
    {
        return atoms;
    }

    public void SetMyPosition(Atom atom)
    {
        positions[atoms.IndexOf(atom)] = atom.transform.localPosition;
    }
    

    public Vector4[] GetPositions()
    {
        return positions;
    } 

    public bool GetStop()
    {
        return stop;
    }

    public void SetStopTrue()
    {
        stop = true;
        AnAtomIsMoving = false;
    }

    public void SetStopFalse()
    {
        stop = false;
        AnAtomIsMoving = true;

    }

    public int GetN()
    {
        return N;
    }

    public void SetN(int n)
    {
        N = n;
        //centralCell.GetComponent<CentralCell>().InstantiateAtoms();
        //Debug.Log("setN: " + GetN());
    }
    
    public void SetN(Slider slider)
    {
        N = (int)slider.value;
        
        //Debug.Log("setN: " + GetN());
    }
    
    public int GetM()
    {
        return M;
    }
    
    public void SetM(int m)
    {
        M = m;
    }

    public int GetK()
    {
        return K;
    }

    public Vector3 GetCellForward()
    {
        return centralCell.transform.forward;
    }

    public Vector3 GetCellRight()
    {
        return centralCell.transform.right;
    }
    
    public void Rotate(float angle)
    {
        //SetStopFalse();
        foreach (var a in atoms)
        {
            a.RotationAngle = (int)angle;
        }

        centralCell.GetComponent<CentralCell>().RotationAngle = (int) angle;
    }


    public int GetR()
    {
        return R;
    }

    public void SetR(float r)
    {
        R = (int)r;
    }

    public bool GetCrystal()
    {
        return isCrystal;
    }

    public void SetCrystal(bool flag)
    {
        isCrystal = flag;
    }
}
