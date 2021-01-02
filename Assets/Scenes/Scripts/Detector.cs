using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private AtomsManager atomsManager;
    private Vector4[] positions;
    private Vector4[] centers = new Vector4[100];
    private float zoom = 4.0f; 
    private static readonly int AtomsPos = Shader.PropertyToID("atomsPos");
    private static readonly int NAtoms = Shader.PropertyToID("n_atoms");
    private static readonly int Zoom = Shader.PropertyToID("zoom");
    private static readonly int K = Shader.PropertyToID("K");

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
        Shader.SetGlobalVectorArray(AtomsPos, positions);
        Shader.SetGlobalInt(NAtoms, atomsManager.GetAtoms().Count);
        Shader.SetGlobalFloat(Zoom, zoom);
        Shader.SetGlobalInt(K, atomsManager.GetK());
    }

    public void SetZoom(float z)
    {
        zoom = z;
    }
    
}
