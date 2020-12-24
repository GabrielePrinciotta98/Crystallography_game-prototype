using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionDetector : MonoBehaviour
{
    private SolutionManager solutionManager;
    private Vector4[] positions;
    private Vector4[] centers = new Vector4[100];
    // Start is called before the first frame update
    void Awake()
    {
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();
        /*
        Shader.SetGlobalVectorArray("centerss", solutionManager.GetPositions());
        Debug.Log(solutionManager.GetAtoms().Count);
        Shader.SetGlobalInt("nAtoms", solutionManager.GetAtoms().Count);
        Shader.SetGlobalFloat("redd", 1.0f);*/
    }

    // Update is called once per frame

    /*
    public void Project()
    {
        Shader.SetGlobalVectorArray("centerss", solutionManager.GetPositions());
        Shader.SetGlobalInt("nAtoms", solutionManager.GetAtoms().Count);
    }
    */
    private void Update()
    {
        Diffraction();
    }

    private void Ripple()
    {
        positions = solutionManager.GetPositions();

        for (int i = 0; i < solutionManager.GetAtoms().Count; i++)
        {
            centers[i] = new Vector4(positions[i].x, positions[i].y / 14f, 1f - ((positions[i].z + 10f) / -22f), 0f);
        }
        Shader.SetGlobalVectorArray("centerss", centers);
        Shader.SetGlobalInt("nAtoms", solutionManager.GetAtoms().Count);
    }

    private void Diffraction()
    {
        positions = solutionManager.GetPositions();
        Debug.Log(positions[0]);
        Shader.SetGlobalVectorArray("atomsPoss", positions);
        Shader.SetGlobalInt("nAtoms", solutionManager.GetAtoms().Count);
    }
}
