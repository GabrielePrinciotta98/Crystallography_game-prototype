using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomRep : MonoBehaviour
{
    public Material[] materials;
    private Renderer _renderer;
    public Atom Parent { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;
        //renderer.sharedMaterial = materials[0];
    }

    // Update is called once per frame
    
    void Update()
    {
        _renderer.sharedMaterial = Parent.GetSelected() ? materials[1] : materials[0];
        //if (Parent.GetSelected()) renderer.sharedMaterial = materials[1];
        

    }

    
}
