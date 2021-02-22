using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Atom : MonoBehaviour
{
    public Material[] materials;
    private ControlPlane controlPlane;
    private Renderer controlPlaneRenderer;
    //private GameObject newControlPlane;
    private DottedLine dottedLine;
    private Renderer dottedLineRenderer;
 
    //private GameObject newDottedLine;
    private Vector3 controlPlanePosition;
    new Renderer renderer;
    AtomsManager atomsManager;
    SolutionManager solutionManager;
    private readonly Vector3 rotationPoint = new Vector3(25f, 6.6f, 10f);
    private float dragSpeed = 0.05f;
    Vector3 lastMousePos;
    private AtomRep[] rep;
    private bool selected = false;
    private Vector3 curPos;
    private bool solved = false;

    
    private void Start()
    {
        
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        ChangeMaterial(0);
        
        atomsManager = FindObjectOfType<AtomsManager>();
        solutionManager = FindObjectOfType<SolutionManager>();
        
        controlPlane = FindObjectOfType<ControlPlane>();
        controlPlaneRenderer = controlPlane.GetComponent<Renderer>();
        controlPlaneRenderer.enabled = false;
        controlPlanePosition = controlPlane.transform.position;
        
        dottedLine = FindObjectOfType<DottedLine>();
        dottedLineRenderer = dottedLine.GetComponent<Renderer>();
        dottedLineRenderer.enabled = false;

        atomsManager.AddAtom(this);
    }
    
    private void Update()
    {
        if (!atomsManager.GetStop())
            transform.RotateAround(rotationPoint, Vector3.up, 10 * Time.deltaTime);
        atomsManager.SetMyPosition(this);
        Debug.Log(SystemInfo.graphicsDeviceType);
    }

    private void OnMouseOver()
    {
        ChangeMaterial(2);
    }

    private void OnMouseExit()
    {
        if (!solved)
          ChangeMaterial(0);
    }


    private void OnMouseDown()
    {
        if (solved) return;
        controlPlane.SetAtom(this, true);
        
        controlPlaneRenderer.enabled = true;
        //newDottedLine = Instantiate(dottedLine, this.transform.position, dottedLine.transform.rotation * Quaternion.Euler(90,0,90), this.transform);
        atomsManager.SetStopTrue();
        solutionManager.SetStopTrue();
        ChangeMaterial(2);
        SetSelected(true);
        atomsManager.AnAtomIsMoving = true;
        lastMousePos = Input.mousePosition;

    }
    
    void OnMouseDrag()
    {
        if (solved) return;
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.position;
        //pos.y = Mathf.Clamp(pos.y, pos.y -2f - atomsManager.GetK()-1, pos.y + 2f + atomsManager.GetK()-1);
        //pos.z = Mathf.Clamp(pos.z, Mathf.Max(pos.z - 2f, 5.5f), Mathf.Min(pos.z + 2f, 9.5f));
        pos.z += delta.x * dragSpeed;
        pos.y += delta.y * dragSpeed;
        var mouseScroll = Input.mouseScrollDelta.y;
        //Debug.Log(mouseScroll);
        if (mouseScroll != 0)
        {
            pos.x -= mouseScroll * dragSpeed * 8;
            dottedLine.SetAtom(this, true);
            dottedLineRenderer.enabled = true;
            //newDottedLine = Instantiate(dottedLine, this.transform.position, dottedLine.transform.rotation * Quaternion.Euler(90,0,90), this.transform);
        }
        else
        {
            dottedLine.SetAtom(null, false);

        }
        
        transform.position = pos;
        atomsManager.SetMyPosition(this);
        atomsManager.SetDraggingAtom(this);
        lastMousePos = Input.mousePosition;
    }
    private void OnMouseUp()
    {
        SetSelected(false);
        atomsManager.AnAtomIsMoving = false;
        controlPlaneRenderer.enabled = false;
        controlPlane.SetAtom(null, false);

        //Destroy(newControlPlane);
        //Destroy(newDottedLine);

        if (!solved)
            ChangeMaterial(0);
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
    
    public void SetSelected(bool flag)
    {
        selected = flag;
    }

    public bool GetSelected()
    {
        return selected;
    }

    public void Rotate(float angle)
    {
        transform.RotateAround(rotationPoint, Vector3.up, angle);
        atomsManager.AnAtomIsMoving = true;
    }

    public void ChangeMaterial(int i)
    {
        renderer.sharedMaterial = materials[i];
    }

    public void SetSolved(bool flag)
    {
        solved = flag;
    }
}