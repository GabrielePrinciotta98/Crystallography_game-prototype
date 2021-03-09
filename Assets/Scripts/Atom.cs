﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Atom : MonoBehaviour
{
    public Material[] materials;
    private ControlPlane controlPlane;
    private Renderer controlPlaneRenderer;
    private DottedLine dottedLine;
    private Renderer dottedLineRenderer;
    private Vector3 controlPlanePosition;
    new Renderer renderer;
    AtomsManager atomsManager;
    SolutionManager solutionManager;
    private Collider _collider;
    
    private readonly Vector3 rotationPoint = new Vector3(25f, 6.6f, 10f);
    private float dragSpeed = 0.05f;
    Vector3 lastMousePos;
    private AtomRep[] rep;
    private bool selected = false;
    
    [SerializeField] private LayerMask layerMask;
    private Collider[] collidersBuffer = new Collider[100];
    
    private Vector3 curPos;
    private Quaternion originPlaneRotation;
    private float rotationAngle;
    
    public float RotationAngle
    {
        get => rotationAngle;
        set => rotationAngle = value;
    }

    private static readonly Vector2 v = Vector3.Normalize(new Vector3(-1, 1, 0));
    private static readonly Vector2 u = Vector3.Normalize(new Vector3(1, 1, 0));

    private bool solved = false;
    private bool dragged = false;
    
    private void Start()
    {
        
        
        curPos = transform.localPosition;
        
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        ChangeMaterial(0);

        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        
        atomsManager = FindObjectOfType<AtomsManager>();
        solutionManager = FindObjectOfType<SolutionManager>();
        
        controlPlane = FindObjectOfType<ControlPlane>();
        originPlaneRotation = controlPlane.transform.localRotation;
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
        if (atomsManager.GameStart)
            _collider.enabled = true;
        if (isColliding())
        {
            
        }
        if (!atomsManager.GetStop())
        {
            Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);
            transform.localPosition = Matrix4x4.Rotate(rotation).MultiplyPoint3x4(curPos);
        }
        else
            curPos = transform.localPosition;
        //CLAMP DELLA POSIZIONE DEGLI ATOMI
        var pos = transform.localPosition;
        pos = new Vector3(Mathf.Clamp(pos.x, -9, 9),
                        Mathf.Clamp(pos.y, -9, 9),
                        Mathf.Clamp(pos.z, -9, 9));
        
        transform.localPosition = pos;



        //INVIO DELLA POSIZIONE CORRENTE ALL'ATOMS MANAGER
        atomsManager.SetMyPosition(this);
        
    }
    
    private bool isColliding()
    {
        int overlapSphereNonAlloc = Physics.OverlapSphereNonAlloc(transform.position, 0.5f, collidersBuffer, layerMask);
        return overlapSphereNonAlloc > 1;
    }

    private void OnMouseOver()
    {
        if (!dragged)
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
        if (atomsManager.Plane.Equals("XZ"))
        {   
            //Debug.Log("prima: " + controlPlane.transform.rotation);
            controlPlane.transform.Rotate(90, 0, 0, Space.Self);
            //controlPlane.transform.localEulerAngles = new Vector3(-90, 0, 0);
            //Debug.Log("piano ruotato: " + controlPlane.transform.rotation);
        }
        
        controlPlaneRenderer.enabled = true;
        
        ChangeMaterial(2);
        SetSelected(true);
        atomsManager.AnAtomIsMoving = true;
        lastMousePos = Input.mousePosition;

    }
    
    void OnMouseDrag()
    {
        if (solved) return;
        //if (isColliding()) return;
        ChangeMaterial(1);
        dragged = true;
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.position;
        //pos.z += delta.x * Mathf.Abs(Vector3.Dot(Vector3.Normalize(delta), v)) * dragSpeed;

        if (!atomsManager.Plane.Equals("XZ"))
        {
            pos.z += delta.x * dragSpeed;       
            pos.y += delta.y * dragSpeed;
        }
        else
        {
            pos.z -= Vector3.Magnitude(delta) * Vector3.Dot(Vector3.Normalize(delta), v) * dragSpeed;
            pos.x -= Vector3.Magnitude(delta) * Vector3.Dot(Vector3.Normalize(delta), u) * dragSpeed;

        }

        if (atomsManager.Plane.Equals("XYZ"))
        {
            var mouseScroll = Input.mouseScrollDelta.y;
            //Debug.Log(mouseScroll);
            if (mouseScroll != 0)
            {
                pos.x -= mouseScroll * dragSpeed * 8;
                if (!atomsManager.Plane.Equals("XZ"))
                    dottedLine.SetAtom(this, true);
                dottedLineRenderer.enabled = true;
            }
            else
            {
                dottedLine.SetAtom(null, false);
            }
        }

        transform.position = pos;
        //atomsManager.SetMyPosition(this);
        atomsManager.SetDraggingAtom(this);
        lastMousePos = Input.mousePosition;
    }
    
    
    private void OnMouseUp()
    {
        if (atomsManager.Plane.Equals("XZ"))
            controlPlane.transform.localRotation = originPlaneRotation;
        SetSelected(false);
        dragged = false;
        atomsManager.AnAtomIsMoving = false;
        controlPlaneRenderer.enabled = false;
        controlPlane.SetAtom(null, false);


        if (!solved)
            ChangeMaterial(0);
    }


    public void SetSelected(bool flag)
    {
        selected = flag;
    }

    public bool GetSelected()
    {
        return selected;
    }
    

    public void ChangeMaterial(int i)
    {
        renderer.sharedMaterial = materials[i];
    }

    public void SetSolved(bool flag)
    {
        solved = flag;
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