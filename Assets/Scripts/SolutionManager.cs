﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{

    private bool isCrystal = true;
    [SerializeReference] GameObject atom;
    [SerializeReference] GameObject pivot;
    [SerializeReference] GameObject centralCell;
    [SerializeReference] GameObject cell;

    //[SerializeReference] QuestionMarkBox box;
    //[SerializeField] int numberOfAtomsPerBlock = 1;
    //[SerializeField] int numberOfBlocks = 1;
    //[SerializeField] int rows = 1;
    //[SerializeField] int columns = 1;
    //[SerializeField] int depth = 1;
    [SerializeField] int N = 1;
    [SerializeField] int M = 1;
    [SerializeField] int K = 1;
    [SerializeField] int R = 4;
    private List<Vector3> cellsSpawnPositions = new List<Vector3>();
    private Vector3 centralCellSpawnPos;
    private Vector3[] atomSpawnPositions = new Vector3[8];
    private bool stop = true;
    List<SolutionAtom> atoms = new List<SolutionAtom>();
    Vector4[] positions = new Vector4[20];

    
    private void Awake()
    {
        centralCellSpawnPos = pivot.transform.position;
        if (K > 5)
        {
            for (int x = -K; x < 2*K; x+=K)
            for (int y = -K; y < 2*K; y+=K)
            for (int z = -K; z < 2*K; z+=K)    
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }
        else
        {
            for (float x = -5f; x < 10f; x+=5f)
            for (float y = -5f; y < 10f; y+=5f)
            for (float z = -5f; z < 10f; z+=5f)    
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                for (int k=0; k<depth; k++)
                    Instantiate(atom, new Vector3(22f+ k, 2f + i, -26f + j), Quaternion.identity, pivot.transform);
            }
        }
        */
        if (isCrystal)
        {
            centralCell = Instantiate(centralCell, centralCellSpawnPos, Quaternion.identity);
            if (K >= 5)
                centralCell.transform.localScale *= K; // scalo la dimensione del modulo centrale
            else
                centralCell.transform.localScale *= 5;

            // SPAWN CELLE RIPETUTE

            for (int i = 0; i < M - 1; i++)
                Instantiate(cell, centralCellSpawnPos + cellsSpawnPositions[i], Quaternion.identity,
                    centralCell.transform);
        }
        else
        {
            //centralCellSpawnPos ha la posizione del pivot
            
            pivot = Instantiate(pivot, centralCellSpawnPos, Quaternion.identity);
            for (int i = 0; i < N - 1; i++)
            {
                Instantiate(atom,
                    new Vector3(centralCellSpawnPos.x + atomSpawnPositions[i].x,
                        centralCellSpawnPos.y + atomSpawnPositions[i].y,
                        centralCellSpawnPos.z + atomSpawnPositions[i].z),
                    Quaternion.identity, pivot.transform);
            }
        }
    }

    public bool GetStop()
    {
        return stop;
    }

    public void AddAtom(SolutionAtom atom)
    {
        atoms.Add(atom);
        positions[atoms.IndexOf(atom)] = atom.transform.localPosition;
    }

    public void SetMyPosition(SolutionAtom atom)
    {
        positions[atoms.IndexOf(atom)] = atom.transform.localPosition;
      
    }

    public Vector4[] GetPositions()
    {
        return positions;
    }

    public List<SolutionAtom> GetAtoms()
    {
        return atoms;
    }

    public void SetStopTrue()
    {
        stop = true;
    }

    public void SetStopFalse()
    {
        stop = false;
    }

    public int GetN()
    {
        return N;
    }

    public void SetN(int n)
    {
        N = n;
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
    
    public int GetR()
    {
        return R;
    }

    public void SetR(float r)
    {
        R = (int)r;
    }
    
    public void Rotate(float angle)
    {
        foreach (var a in atoms)
        {
            a.Rotate(angle);
        }
        
        centralCell.GetComponent<CentralCellSolution>().Rotate(angle);
    }
    
    public bool GetCrystal()
    {
        return isCrystal;
    }

    public void SetCrystal(bool flag)
    {
        isCrystal = flag;
    }

    public void SetAtomSpawnPositions(Vector3[] listPos)
    {
        this.atomSpawnPositions = listPos;
    }
    
}
