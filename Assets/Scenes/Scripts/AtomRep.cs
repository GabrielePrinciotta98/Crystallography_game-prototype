using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomRep : MonoBehaviour
{
    AtomsManager atomsManagerRep;
    private Vector3 rotationPointRep = new Vector3(20f, 6.6f, 10f);
    public Material[] materials;
    new Renderer renderer;
    private Atom parent;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.sharedMaterial = materials[0];
        atomsManagerRep = GameObject.FindObjectOfType<AtomsManager>();
        parent = GetComponentInParent<Atom>();
        //atomsManagerRep.AddAtom(this);
    }

    // Update is called once per frame
    
    void Update()
    {
        if (parent.GetSelected())
            renderer.sharedMaterial = materials[1];
        else
            renderer.sharedMaterial = materials[0];
    }

    
}
