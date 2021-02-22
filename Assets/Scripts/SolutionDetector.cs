using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionDetector : MonoBehaviour
{
    private Renderer _renderer;
    private SolutionManager solutionManager;
    private Vector4[] positions;
    private Vector4[] centers = new Vector4[100];
    private float zoom = 4f;
    private float pwr = -3f;
    private bool pwrSetted;

    private Vector3 a, c;
    private float lambda = 0.5f;
    private EmitterConeSol emitter;
    private CustomRenderTexture crt;
    private static readonly int AtomsPoss = Shader.PropertyToID("atomsPoss");
    private static readonly int NAToms = Shader.PropertyToID("nAtoms");
    private static readonly int _Zoom = Shader.PropertyToID("_zoom");
    private static readonly int _K = Shader.PropertyToID("_K");
    private static readonly int _Pwr = Shader.PropertyToID("_pwr");
    private static readonly int _A = Shader.PropertyToID("_a");
    private static readonly int _C = Shader.PropertyToID("_c");
    private static readonly int _R = Shader.PropertyToID("_R");
    private static readonly int _M = Shader.PropertyToID("_M");
    private static readonly int _Lambda = Shader.PropertyToID("_lambda");
    private static readonly int MainTexx = Shader.PropertyToID("_MainTex");

    // Start is called before the first frame update
    void Awake()
    {
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();
        emitter = FindObjectOfType<EmitterConeSol>();

    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
        crt = (CustomRenderTexture) _renderer.material.GetTexture(MainTexx);
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
        
        if (!solutionManager.AnAtomIsMoving) return;
        Diffraction();
        crt.Update();
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
        a = solutionManager.GetCellRight() * solutionManager.GetK();
        c = solutionManager.GetCellForward() * solutionManager.GetK();
        Shader.SetGlobalVectorArray(AtomsPoss, positions);
        Shader.SetGlobalInt(NAToms, solutionManager.GetAtoms().Count);
        Shader.SetGlobalFloat(_Zoom, zoom);
        Shader.SetGlobalInt(_K, solutionManager.GetK());
        Shader.SetGlobalFloat(_Pwr, Mathf.Pow(2, pwr));
        Shader.SetGlobalVector(_A, a);
        Shader.SetGlobalVector(_C, c);
        Shader.SetGlobalInt(_R, solutionManager.GetR());
        Shader.SetGlobalInt(_M, solutionManager.GetM());
        Shader.SetGlobalFloat(_Lambda, lambda);

    }
    
    public void SetZoom(float z)
    {
        zoom = z;
        Diffraction();
        crt.Update();
    }

    public void SetPwr(float p)
    {
        pwr = p;
        pwrSetted = true;
        Diffraction();
        crt.Update();
    }
    
    public void SetLambda(float l)
    {
        lambda = l;
        Diffraction();
        crt.Update();
    }

    public void SetSolutionManager(SolutionManager sm)
    {
        solutionManager = sm;
    }
}
