using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    
    
    private float dragSpeed = 0.05f;
    Vector3 lastMousePos;


    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.sharedMaterial = materials[0];
        atomsManager = GameObject.FindObjectOfType<AtomsManager>();
        atomsManager.AddAtom(this);
    }


    void OnMouseDown()
    {
        renderer.sharedMaterial = materials[1];
        lastMousePos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.position;
        pos.z += delta.x * dragSpeed;
        pos.y += delta.y * dragSpeed;
        transform.position = pos;
        atomsManager.SetMyPosition(this);
        lastMousePos = Input.mousePosition;
    }
    public Material[] materials;
    new Renderer renderer;

    AtomsManager atomsManager;

    private void OnMouseUp()
    {
        renderer.sharedMaterial = materials[0];
    }

    /*
    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7f);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objPosition.x = transform.position.x;
        //objPosition.z = transform.position.z;
        transform.position = objPosition;
        atomsManager.SetMyPosition(this);
    }*/
}