using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Material[] materials;
    private bool swapped;
    private bool isDirty = true;
    private Renderer _renderer;
    private AtomsManager atomsManager;
    private SolutionManager solutionManager;
    private LevelManager _levelManager;
    private EmitterCone emitter;
    private Vector4[] positions;
    private readonly Vector4[] centers = new Vector4[100];
    private float zoom = 4f;
    private float pwr = -3f;
    private bool pwrSetted;
    private float lambda = 0.5f;
    private CustomRenderTexture crt;
    private Vector3 a, c;
    private GameObject arrows;
    
    private static readonly int AtomsPos = Shader.PropertyToID("atomsPos");
    private static readonly int NAtoms = Shader.PropertyToID("n_atoms");
    private static readonly int Zoom = Shader.PropertyToID("zoom");
    private static readonly int K = Shader.PropertyToID("K");
    private static readonly int Pwr = Shader.PropertyToID("pwr");
    private static readonly int A = Shader.PropertyToID("a");
    private static readonly int C = Shader.PropertyToID("c");
    private static readonly int R = Shader.PropertyToID("R");
    private static readonly int M = Shader.PropertyToID("M");
    private static readonly int Lambda = Shader.PropertyToID("lambda");
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    public bool TransitionHappened { get; set; }

    private void Awake()
    {
        //atomsManager = FindObjectOfType<AtomsManager>();
        //solutionManager = FindObjectOfType<SolutionManager>();
        _levelManager = FindObjectOfType<LevelManager>();
        emitter = FindObjectOfType<EmitterCone>();
        arrows = GameObject.Find("Arrows");
    }

    private void Start()
    {
        arrows.SetActive(false);

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
            UpdatePattern();
        }
        //Debug.Log("move: " + atomsManager.AnAtomIsMoving);
        if (isDirty) UpdatePattern();
        //Debug.Log("update");
        isDirty = false;
    }
    
    public void UpdatePattern()
    {
        Diffraction();
        crt.Update();
    }
    
    
    private void Diffraction()
    {
        positions = atomsManager.GetPositions();
        //uso i metodi del solutionManager per avere i vettori a,c corretti della soluzione
        a = solutionManager.GetCellRight() * atomsManager.GetK();
        c = solutionManager.GetCellForward() * atomsManager.GetK();

        
        
        Debug.Log("A: " + a);
        Debug.Log("C: " + c);
        
        Shader.SetGlobalVectorArray(AtomsPos, positions);
        Shader.SetGlobalInt(NAtoms, atomsManager.GetAtoms().Count);
        Shader.SetGlobalFloat(Zoom, zoom);
        Shader.SetGlobalInt(K, atomsManager.GetK());
        if (atomsManager.isCrystal) 
        {
            Shader.SetGlobalFloat(Pwr, Mathf.Pow(2, pwr + 2));
            Shader.SetGlobalInt(R, TransitionR()); // 1 sara poi una variabile che varra 1 PRIMA della transizione, e atomsManager.GetR() DOPO
            Shader.SetGlobalInt(M, TransitionM()); // 1 sara poi una variabile che varra 1 PRIMA della transizione, e atomsManager.GetM() DOPO
        }
        else
        {
            Shader.SetGlobalFloat(Pwr, Mathf.Pow(2, pwr));        
            Shader.SetGlobalInt(R, atomsManager.GetR());
            Shader.SetGlobalInt(M, atomsManager.GetM());
        }

        Shader.SetGlobalVector(A, a);
        Shader.SetGlobalVector(C, c);
        //Debug.Log("R: " + atomsManager.GetR());
        Shader.SetGlobalFloat(Lambda, lambda);
    }

    private int TransitionR()
    {
        return TransitionHappened ? atomsManager.GetR() : 1;
    }

    private int TransitionM()
    {
        return TransitionHappened ? atomsManager.GetM() : 1;
    }

    public void SetZoom(float z)
    {
        zoom = z;
        UpdatePattern();
    }

    public float GetZoom()
    {
        return this.zoom;
    }

    public void SetPwr(float p)
    {
        pwr = p;
        pwrSetted = true;
        UpdatePattern();
    }

    public float GetPwr()
    {
        return this.pwr;
    }

    public void SetLambda(float l)
    {
        lambda = l;
        UpdatePattern();
    }

    public void SetAtomsManager(AtomsManager am)
    {
        this.atomsManager = am;
    }

    public void Compare()
    {
        if (swapped)
            UnSwap();
        else
            Swap();
    }

    public void Swap()
    {
        _renderer.sharedMaterial = materials[1];
        arrows.SetActive(true);
        emitter.gameObject.SetActive(false);
        swapped = true;
    }
    
    public void UnSwap()
    {
        _renderer.sharedMaterial = materials[0];
        arrows.SetActive(false);
        emitter.gameObject.SetActive(true);
        swapped = false;

    }

    public void SetDirty()
    {
        isDirty = true;
    }
    
    public void SetSolutionManager(SolutionManager sm)
    {
        solutionManager = sm;
    }
}
