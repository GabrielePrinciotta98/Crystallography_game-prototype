﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{

    public GameObject atom;
    public int numberOfAtomsPerBlock = 1;
    public int numberOfBlocks = 1;

    List<SolutionAtom> atoms = new List<SolutionAtom>();
    Vector4[] positions = new Vector4[100];

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] spawnPositions = new Vector3[numberOfAtomsPerBlock];
        for (int a = 0; a < numberOfAtomsPerBlock; a++)
        {
            spawnPositions[a] = new Vector3(Random.Range(10f, 14f), Random.Range(0.5f, 4.5f), Random.Range(-22.5f, -18.5f));
        }


        for (int i = 0; i < numberOfBlocks / 2; i++)
        {
            for (int j = 0; j < numberOfBlocks / 2; j++)
            {
                for (int k = 0; k < numberOfAtomsPerBlock; k++)
                {
                    Instantiate(atom, new Vector3(spawnPositions[k].x, spawnPositions[k].y + i * 6, spawnPositions[k].z + j * 6), Quaternion.identity);
                }
            }
        }
    }

    public void AddAtom(SolutionAtom atom)
    {
        atoms.Add(atom);
        positions[atoms.IndexOf(atom)] = atom.transform.position;
    }

    public void SetMyPosition(SolutionAtom atom)
    {
        positions[atoms.IndexOf(atom)] = atom.transform.position;
        //Debug.Log(positions[atoms.IndexOf(atom)]);
    }

    public Vector4[] GetPositions()
    {
        return positions;
    }

    public List<SolutionAtom> GetAtoms()
    {
        return atoms;
    }

}