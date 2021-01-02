using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotAtom : MonoBehaviour
{
    
    AtomsManager atomsManager;
    private Vector3 rotationPoint = new Vector3(20f, 10f, 10f);


    // Start is called before the first frame update
    void Start()
    {
        atomsManager = GameObject.FindObjectOfType<AtomsManager>();

    }

    private void Update()
    {
        if (!atomsManager.GetStop())
            transform.RotateAround(rotationPoint, Vector3.up, 10 * Time.fixedDeltaTime);
        //Debug.Log("Atomo: " + transform.position);
    }
}
