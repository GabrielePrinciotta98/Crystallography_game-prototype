using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{

    public GameObject atom;
    [SerializeReference] SolutionDetector solutionDetector;
    //[SerializeReference] QuestionMarkBox box;
    [SerializeField] int numberOfAtomsPerBlock = 1;
    [SerializeField] int numberOfBlocks = 1;
    //[SerializeField] int rows = 1;
    //[SerializeField] int columns = 1;

    private bool stop = true;
    List<SolutionAtom> atoms = new List<SolutionAtom>();
    Vector4[] positions = new Vector4[100];

    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Instantiate(atom, new Vector3(22f, 2f + i * (Random.Range(2f, 3f)), -26f + j * (Random.Range(2f,3f))), Quaternion.identity);
            }
        }
        */
        int a;
        Vector3[] spawnPositions = new Vector3[numberOfAtomsPerBlock];
        for (a = 0; a < numberOfAtomsPerBlock; a++)
        {
            spawnPositions[a] = new Vector3(Random.Range(12f, 30f), Random.Range(1f, 4.5f), Random.Range(-20.5f, -16.5f));
        }


        for (int i = 0; i < numberOfBlocks / 3; i++)
        {
            for (int j = 0; j < numberOfBlocks / 3; j++)
            {
                for (int k = 0; k < numberOfBlocks / 3; k++)
                {
                    for (a=0; a < numberOfAtomsPerBlock; a++)
                    {
                        Instantiate(atom, new Vector3(spawnPositions[a].x+k*6, spawnPositions[a].y + i * 6, spawnPositions[a].z + j * 6), Quaternion.identity);
                    }
                }
            }
        }

        float degree = Random.Range(0, 360);
        transform.RotateAround(new Vector3(26f, 10f, 22f), Vector3.up, degree * Time.fixedDeltaTime);
        
        //solutionDetector.Project();
        //DontShowAtoms();
    }

    public bool GetStop()
    {
        return stop;
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

    public void SetStopTrue()
    {
        stop = true;
    }

    public void SetStopFalse()
    {
        stop = false;
    }

    /*
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
    */
}
