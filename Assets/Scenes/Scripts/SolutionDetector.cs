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
        Shader.SetGlobalVectorArray("centers", solutionManager.GetPositions());
        Shader.SetGlobalInt("n_atoms", solutionManager.GetAtoms().Count);
        Shader.SetGlobalFloat("redd", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}
