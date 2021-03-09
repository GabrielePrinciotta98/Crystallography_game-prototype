using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomRep : MonoBehaviour
{
    public Material[] materials;
    new Renderer renderer;
    private Atom parent;
    private AtomsManager atomsManager;
    private CentralCell centralCell;
    private Vector3 originPos;
    private Transform atomFather;
    // Start is called before the first frame update
    void Start()
    {
        atomFather = transform.parent;
        atomsManager = FindObjectOfType<AtomsManager>();
        centralCell = FindObjectOfType<CentralCell>();
        originPos = transform.position;
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.sharedMaterial = materials[0];
        parent = GetComponentInParent<Atom>();
        //atomsManagerRep.AddAtom(this);
    }

    // Update is called once per frame
    
    void Update()
    {
        renderer.sharedMaterial = parent.GetSelected() ? materials[1] : materials[0];
        if (!atomsManager.GetCrystal()) return;
        if (!atomsManager.GetStop())
        {
            transform.parent = centralCell.transform;
        }
        else
        {
            transform.parent = atomFather;
        }
    }

    
}
