using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomRep : MonoBehaviour
{
    public Material[] materials;
    new Renderer renderer;
    private Atom parent;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.sharedMaterial = materials[0];
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
