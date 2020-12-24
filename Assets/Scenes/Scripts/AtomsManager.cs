using System;
using System.Collections.Generic;
using UnityEngine;

public class AtomsManager : MonoBehaviour
{

    public GameObject atom;
    [SerializeField] int numberOfAtomsPerBlock = 1;
    [SerializeField] int numberOfBlocks = 1;
    //[SerializeField] int rows = 1;
    //[SerializeField] int columns = 1;
    List<Atom> atoms = new List<Atom>();
    Vector4[] positions = new Vector4[100];
    private bool stop = true;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(atom, new Vector3(25f, 2f, 7f), Quaternion.identity);

        int a;
        Vector3[] spawnPositions = new Vector3[numberOfAtomsPerBlock];
        for (a = 0; a < numberOfAtomsPerBlock; a++)
        {
            spawnPositions[a] = new Vector3(UnityEngine.Random.Range(12f, 30f), UnityEngine.Random.Range(1f, 4.5f), UnityEngine.Random.Range(5.5f, 9.5f));
        }


        for (int i = 0; i < numberOfBlocks / 3; i++)
        {
            for (int j=0; j < numberOfBlocks / 3; j++)
            {
                for (int k=0; k< numberOfBlocks / 3; k++)
                {
                    for (a=0; a < numberOfAtomsPerBlock; a++)
                    {
                        Instantiate(atom, new Vector3(spawnPositions[a].x+k*6, spawnPositions[a].y + i * 6, spawnPositions[a].z + j * 6), Quaternion.identity);

                    }
                }
            }
        }
        
        /*
        for (int i=0; i<rows; i++)
        {
            for (int j=0; j<columns; j++)
            {
                Instantiate(atom, new Vector3(22f, 2f + i*1, 7f + j*1), Quaternion.identity);
            }
        }
        */
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
