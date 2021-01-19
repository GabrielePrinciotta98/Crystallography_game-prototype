using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private AtomsManager atomsManager;
    private Vector4[] positions;
    private readonly Vector4[] centers = new Vector4[100];
    private float zoom = 4f;
    private float pwr = 1f;
    private float lambda = 0.5f;
    private Vector3 a, c;
    private static readonly int AtomsPos = Shader.PropertyToID("atomsPos");
    private static readonly int NAtoms = Shader.PropertyToID("n_atoms");
    private static readonly int Zoom = Shader.PropertyToID("zoom");
    private static readonly int K = Shader.PropertyToID("K");
    private static readonly int Pwr = Shader.PropertyToID("pwr");
    private static readonly int Centers = Shader.PropertyToID("centers");
    private static readonly int A = Shader.PropertyToID("a");
    private static readonly int C = Shader.PropertyToID("c");
    private static readonly int R = Shader.PropertyToID("R");
    private static readonly int M = Shader.PropertyToID("M");
    private static readonly int Lambda = Shader.PropertyToID("lambda");

    private void Awake()
    {
        atomsManager = GameObject.FindObjectOfType<AtomsManager>();
    }

    private void Start()
    {
        
        //atomsManager = GameObject.FindObjectOfType<AtomsManager>();

        //atomsManager.GetCubeVectors(0);
        //atomsManager.GetCubeVectors(1);
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
        Shader.SetGlobalVectorArray(Centers, centers);
        Shader.SetGlobalInt(NAtoms, atomsManager.GetAtoms().Count);
    }


    private void Diffraction()
    {
        positions = atomsManager.GetPositions();
        //Debug.Log(positions[0]);
        a = atomsManager.GetCellRight() * atomsManager.GetK();
        c = atomsManager.GetCellForward() * atomsManager.GetK();
        
        Shader.SetGlobalVectorArray(AtomsPos, positions);
        Shader.SetGlobalInt(NAtoms, atomsManager.GetAtoms().Count);
        Shader.SetGlobalFloat(Zoom, zoom);
        Shader.SetGlobalInt(K, atomsManager.GetK());
        Shader.SetGlobalFloat(Pwr, pwr);
        Shader.SetGlobalVector(A, a);
        Shader.SetGlobalVector(C, c);
        Shader.SetGlobalInt(R, atomsManager.GetR());
        Shader.SetGlobalInt(M, atomsManager.GetM());
        Shader.SetGlobalFloat(Lambda, lambda);

    }

    public void SetZoom(float z)
    {
        zoom = z;
    }

    public float GetZoom()
    {
        return this.zoom;
    }

    public void SetPwr(float p)
    {
        pwr = p;
    }

    public float GetPwr()
    {
        return this.pwr;
    }

    public void SetLambda(float l)
    {
        lambda = l;
    }

    public void SetAtomsManager(AtomsManager am)
    {
        this.atomsManager = am;
    }
}
