using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Atom : MonoBehaviour
{
    public Material[] materials;
    new Renderer renderer;
    private int id;
    AtomsManager atomsManager;
    SolutionManager solutionManager;
    private Vector3 rotationPoint = new Vector3(22f, 10f, 11f);
    private float dragSpeed = 0.05f;
    Vector3 lastMousePos;
    

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.sharedMaterial = materials[0];
        atomsManager = GameObject.FindObjectOfType<AtomsManager>();
        solutionManager = GameObject.FindObjectOfType<SolutionManager>();
        atomsManager.AddAtom(this);
    }
    
    private void Update()
    {
        if (!atomsManager.GetStop())
            transform.RotateAround(rotationPoint, Vector3.up, 10 * Time.fixedDeltaTime);
        atomsManager.SetMyPosition(this);
        //Debug.Log("Atomo: " + transform.position);
    }
    
    void OnMouseDown()
    {
        
        atomsManager.SetStopTrue();
        solutionManager.SetStopTrue();
        renderer.sharedMaterial = materials[1];
        lastMousePos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.position;
        //pos.y = Mathf.Clamp(pos.y, Mathf.Max(pos.y - 2f, 0.5f), Mathf.Min(pos.y + 2f, 4.5f));
        //pos.z = Mathf.Clamp(pos.z, Mathf.Max(pos.z - 2f, 5.5f), Mathf.Min(pos.z + 2f, 9.5f));
        pos.z += delta.x * dragSpeed;
        pos.y += delta.y * dragSpeed;
        transform.position = pos;
        atomsManager.SetMyPosition(this);
        lastMousePos = Input.mousePosition;
    }
    

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