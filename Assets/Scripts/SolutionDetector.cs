using UnityEngine;

public class SolutionDetector : MonoBehaviour
{
    private Renderer _renderer;
    private SolutionManager solutionManager;
    private Vector4[] positions;
    private float zoom = 4f;
    private float pwr = -3f;
    private bool pwrSetted;
    private bool isDirty = true;
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
            UpdatePattern();
        }
        
        if (isDirty) UpdatePattern();

        isDirty = false;
    }

    public void UpdatePattern()
    {
        Diffraction();
        crt.Update();
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
        Shader.SetGlobalFloat(_Pwr, solutionManager.GetCrystal() ? Mathf.Pow(2, pwr + 2) : Mathf.Pow(2, pwr));
        Shader.SetGlobalVector(_A, a);
        Shader.SetGlobalVector(_C, c);
        Shader.SetGlobalInt(_R, solutionManager.GetR());
        Shader.SetGlobalInt(_M, solutionManager.GetM());
        Shader.SetGlobalFloat(_Lambda, lambda);

    }
    
    public void SetZoom(float z)
    {
        zoom = z;
        UpdatePattern();
    }

    public void SetPwr(float p)
    {
        pwr = p;
        pwrSetted = true;
        UpdatePattern();
    }
    
    public void SetLambda(float l)
    {
        lambda = l;
        UpdatePattern();
    }

    public void SetSolutionManager(SolutionManager sm)
    {
        solutionManager = sm;
    }

    public void SetDirty()
    {
        isDirty = true;
    }
    
}
