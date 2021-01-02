using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtomsManager : MonoBehaviour
{

    [SerializeReference] GameObject atom;
    [SerializeReference] GameObject pivot;
    [SerializeReference] GameObject centralCell;
    [SerializeReference] GameObject cell;
    [SerializeField] int N = 1;
    [SerializeField] int M = 1;
    [SerializeField] int K = 1;
    //[SerializeField] int rows = 1;
    //[SerializeField] int columns = 1;
    //[SerializeField] int depth = 1;
    List<Atom> atoms = new List<Atom>();
    Vector4[] positions = new Vector4[200];
    private bool stop = true;
    private List<Vector3> cellsSpawnPositions = new List<Vector3>();
    private Vector3 centralCellSpawnPos = new Vector3(20f, 6.6f, 10f);
    
    private void Awake()
    {
        if (K >= 5)
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
        //Instantiate(atom, new Vector3(25f, 2f, 7f), Quaternion.identity);
        /*
        int a;
        Vector3[] spawnPositions = new Vector3[N];
        for (a = 0; a < N; a++)
        {
            spawnPositions[a] = new Vector3(UnityEngine.Random.Range(12f, 30f), UnityEngine.Random.Range(1f, 4.5f), UnityEngine.Random.Range(5.5f, 9.5f));
        }


        for (int i = 0; i < M / 3; i++)
        {
            for (int j=0; j < M / 3; j++)
            {
                for (int k=0; k< M / 3; k++)
                {
                    for (a=0; a < N; a++)
                    {
                        Instantiate(atom, new Vector3(spawnPositions[a].x+k*6, spawnPositions[a].y + i * 6, spawnPositions[a].z + j * 6), Quaternion.identity);

                    }
                }
            }
        }
        */
        /*
        for (int i=0; i<rows; i++)
        {
            for (int j=0; j<columns; j++)
            {
                for (int k=0; k<depth; k++)
                    (Instantiate(atom, new Vector3(22f + k*3, 1f + i*3, 4f + j*3), Quaternion.identity) as GameObject).transform.parent = pivot.transform;
            }
        }
        */
        // SPAWN CELLA CENTRALE
        
        centralCell = Instantiate(centralCell, centralCellSpawnPos, Quaternion.identity);
        if (K >= 5)    
            centralCell.transform.localScale *= K; // scalo la dimensione del modulo centrale
        else
            centralCell.transform.localScale *= 5;
            
        // SPAWN CELLE RIPETUTE

        GameObject tmp = new GameObject();
        for (int i = 0; i < M-1; i++)
        {
                
            tmp  = Instantiate(cell, centralCellSpawnPos + cellsSpawnPositions[i], Quaternion.identity);
            
            if (K >= 5)
                tmp.transform.localScale *= K;
            else
                tmp.transform.localScale *= 5;
        }
        
        
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
        //Debug.Log(positions[atoms.IndexOf(atom)]);
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
    }

    public void SetStopFalse()
    {
        stop = false;
    }

    public int GetN()
    {
        return N;
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
   
    
}
