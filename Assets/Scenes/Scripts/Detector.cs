using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private AtomsManager atomsManager;
    private Vector4[] positions;
    private Vector4[] centers = new Vector4[100];

    private void Awake()
    {
        atomsManager = GameObject.FindObjectOfType<AtomsManager>();
    }
    
    private void Update()
    {
        Diffraction();

    }

    private void Ripple()
    {
        positions = atomsManager.GetPositions();

        for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
        {
            centers[i] = new Vector4(positions[i].x, positions[i].y / 14f, positions[i].z / 22f, 0f);
        }
        Shader.SetGlobalVectorArray("centers", centers);
        Shader.SetGlobalInt("n_atoms", atomsManager.GetAtoms().Count);
    }


    private void Diffraction()
    {
        positions = atomsManager.GetPositions();
        Shader.SetGlobalVectorArray("atomsPos", positions);
        Shader.SetGlobalInt("n_atoms", atomsManager.GetAtoms().Count);

    }

}
