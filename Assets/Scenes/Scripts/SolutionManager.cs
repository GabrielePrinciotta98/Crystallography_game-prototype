using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{

    public GameObject atom;
    [SerializeReference] SolutionDetector solutionDetector;
    [SerializeReference] QuestionMarkBox box;
    public int numberOfAtomsPerBlock = 1;
    public int numberOfBlocks = 1;

    List<SolutionAtom> atoms = new List<SolutionAtom>();
    Vector4[] positions = new Vector4[100];

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Instantiate(atom, new Vector3(25f, 2f + i * (Random.Range(2, 4)), -24f + j * (Random.Range(2,4))), Quaternion.identity);
            }
        }

        /*
        Vector3[] spawnPositions = new Vector3[numberOfAtomsPerBlock];
        for (int a = 0; a < numberOfAtomsPerBlock; a++)
        {
            spawnPositions[a] = new Vector3(Random.Range(22f, 30f), Random.Range(1f, 4.5f), Random.Range(-20.5f, -16.5f));
        }


        for (int i = 0; i < numberOfBlocks / 2; i++)
        {
            for (int j = 0; j < numberOfBlocks / 2; j++)
            {
                for (int k = 0; k < numberOfAtomsPerBlock; k++)
                {
                    Instantiate(atom, new Vector3(spawnPositions[k].x, spawnPositions[k].y + i * 6, spawnPositions[k].z - j * 6), Quaternion.identity);
                }
            }
        }

        float degree = Random.Range(0, 360);
        transform.RotateAround(new Vector3(26f, 10f, 22f), Vector3.up, degree * Time.fixedDeltaTime);
        */
        solutionDetector.Project();
        DontShowAtoms();
    }

    public void AddAtom(SolutionAtom atom)
    {
        atoms.Add(atom);
        positions[atoms.IndexOf(atom)] = atom.transform.position;
    }

    public void SetMyPosition(SolutionAtom atom)
    {
        positions[atoms.IndexOf(atom)] = atom.transform.position;
      
    }

    public Vector4[] GetPositions()
    {
        return positions;
    }

    public List<SolutionAtom> GetAtoms()
    {
        return atoms;
    }

    public void DontShowAtoms()
    {
        foreach (SolutionAtom a in atoms)
        {
            a.DontShowAtom();
        }
    }

    public void ShowAtoms()
    {
        box.Hide();
        foreach (SolutionAtom a in atoms)
        {
            a.ShowAtom();
        }
    }

}
