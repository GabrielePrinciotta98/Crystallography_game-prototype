using System;
using System.Collections.Generic;
using UnityEngine;

public class AtomsManager : MonoBehaviour
{

    public GameObject atom;
    //public int numberOfAtomsPerBlock = 1;
    //public int numberOfBlocks = 1;
    [SerializeField] int rows = 1;
    [SerializeField] int columns = 1;
    List<Atom> atoms = new List<Atom>();
    Vector4[] positions = new Vector4[100];
    private bool stop = true;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(atom, new Vector3(25f, 2f, 7f), Quaternion.identity);
        
        /*
        Vector3[] spawnPositions = new Vector3[numberOfAtomsPerBlock];
        for (int a = 0; a < numberOfAtomsPerBlock; a++)
        {
            spawnPositions[a] = new Vector3(UnityEngine.Random.Range(22f, 30f), UnityEngine.Random.Range(1f, 4.5f), UnityEngine.Random.Range(5.5f, 9.5f));
        }


        for (int i = 0; i < numberOfBlocks / 2; i++)
        {
            for (int j=0; j < numberOfBlocks / 2; j++)
            {
                for (int k=0; k< numberOfAtomsPerBlock; k++)
                {
                    Instantiate(atom, new Vector3(spawnPositions[k].x, spawnPositions[k].y+i*6, spawnPositions[k].z + j * 6), Quaternion.identity);
                }
            }
        }
        */
        for (int i=0; i<rows; i++)
        {
            for (int j=0; j<columns; j++)
            {
                Instantiate(atom, new Vector3(22f, 2f + i*2, 7f + j*2), Quaternion.identity);
            }
        }

    }

  
    public void AddAtom(Atom atom)
    {
        atoms.Add(atom);
        positions[atoms.IndexOf(atom)] = atom.transform.position;
    }

    public List<Atom> GetAtoms()
    {
        return atoms;
    }

    public void SetMyPosition(Atom atom)
    {
        positions[atoms.IndexOf(atom)] = atom.transform.position;
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

}
