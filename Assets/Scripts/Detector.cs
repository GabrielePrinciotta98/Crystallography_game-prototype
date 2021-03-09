using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private Renderer _renderer;
    private AtomsManager atomsManager;
    private LevelManager2 levelManager2;
    private EmitterCone emitter;
    private Vector4[] positions;
    private readonly Vector4[] centers = new Vector4[100];
    private float zoom = 4f;
    private float pwr = -3f;
    private bool pwrSetted;
    private float lambda = 0.5f;
    private CustomRenderTexture crt;
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
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private void Awake()
    {
        atomsManager = FindObjectOfType<AtomsManager>();
        levelManager2 = FindObjectOfType<LevelManager2>();
        emitter = FindObjectOfType<EmitterCone>();
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
        crt = (CustomRenderTexture) _renderer.material.GetTexture(MainTex);
        crt.Initialize();
    }

    private void Update()
    {
        if (!emitter.GetPowerOn()) return;
        _renderer.enabled = true; 
        
        if (pwr < 0 && !pwrSetted)
        {
            pwr += 0.02f;
            Diffraction();
            crt.Update();
        }
        //Debug.Log("move: " + atomsManager.AnAtomIsMoving);
        if (!atomsManager.AnAtomIsMoving && !levelManager2.GetOver()) return;
        Diffraction();
        crt.Update();
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
        Shader.SetGlobalFloat(Pwr, Mathf.Pow(2, pwr));
        Shader.SetGlobalVector(A, a);
        Shader.SetGlobalVector(C, c);
        Shader.SetGlobalInt(R, atomsManager.GetR());
        Shader.SetGlobalInt(M, atomsManager.GetM());
        Shader.SetGlobalFloat(Lambda, lambda);
    }

    public void SetZoom(float z)
    {
        zoom = z;
        Diffraction();
        crt.Update();
    }

    public float GetZoom()
    {
        return this.zoom;
    }

    public void SetPwr(float p)
    {
        pwr = p;
        pwrSetted = true;
        Diffraction();
        crt.Update();
    }

    public float GetPwr()
    {
        return this.pwr;
    }

    public void SetLambda(float l)
    {
        lambda = l;
        Diffraction();
        crt.Update();
    }

    public void SetAtomsManager(AtomsManager am)
    {
        this.atomsManager = am;
    }
}
