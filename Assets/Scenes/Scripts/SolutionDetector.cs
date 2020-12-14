using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionDetector : MonoBehaviour
{
    private SolutionManager solutionManager;
    
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
    public void Project()
    {
        Shader.SetGlobalVectorArray("centerss", solutionManager.GetPositions());
        Shader.SetGlobalInt("nAtoms", solutionManager.GetAtoms().Count);
    }
}
