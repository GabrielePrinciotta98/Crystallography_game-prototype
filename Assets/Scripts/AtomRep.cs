using UnityEngine;

public class AtomRep : MonoBehaviour
{
    public Material[] materials;
    private Renderer _renderer;
    private Atom Parent { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;
    }

    // Update is called once per frame
    
    void Update()
    {
        _renderer.sharedMaterial = Parent.GetSelected() ? materials[1] : materials[0];
    }

    
}
