using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private AtomsManager atomsManager;

    private void Awake()
    {
        atomsManager = GameObject.FindObjectOfType<AtomsManager>();
    }
    
    private void Update()
    {
        Shader.SetGlobalVectorArray("centers", atomsManager.GetPositions());
        Shader.SetGlobalInt("n_atoms", atomsManager.GetAtoms().Count);
    

    }
    

}
