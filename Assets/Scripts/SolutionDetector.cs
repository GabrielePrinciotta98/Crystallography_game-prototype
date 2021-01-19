using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionDetector : MonoBehaviour
{
    private SolutionManager solutionManager;
    private Vector4[] positions;
    private Vector4[] centers = new Vector4[100];
    private float zoom = 4f;
    private float pwr = 1f;
    private Vector3 a, c;
    private float lambda = 0.5f;
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
        a = solutionManager.GetCellRight() * solutionManager.GetK();
        c = solutionManager.GetCellForward() * solutionManager.GetK();
        
        Shader.SetGlobalVectorArray(AtomsPoss, positions);
        Shader.SetGlobalInt(NAToms, solutionManager.GetAtoms().Count);
        Shader.SetGlobalFloat(_Zoom, zoom);
        Shader.SetGlobalInt(_K, solutionManager.GetK());
        Shader.SetGlobalFloat(_Pwr, pwr);
        Shader.SetGlobalVector(_A, a);
        Shader.SetGlobalVector(_C, c);
        Shader.SetGlobalInt(_R, solutionManager.GetR());
        Shader.SetGlobalInt(_M, solutionManager.GetM());
        Shader.SetGlobalFloat(_Lambda, lambda);

    }
    
    public void SetZoom(float z)
    {
        zoom = z;
    }

    public void SetPwr(float p)
    {
        pwr = p;
    }
    
    public void SetLambda(float l)
    {
        lambda = l;
    }

    public void SetSolutionManager(SolutionManager sm)
    {
        solutionManager = sm;
    }
}
