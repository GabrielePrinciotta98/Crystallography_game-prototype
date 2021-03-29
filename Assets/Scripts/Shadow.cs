using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    //private float dragSpeed = 0.002f;
    //Vector3 lastMousePos;

    public Color color;
    public Color selectedColor;
    private new Renderer renderer;
    //private AtomsManager atomsManager;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.material.color = color;
        //transform.localPosition = new Vector3(0, transform.parent.position.y-0.1f, 0);
        transform.localPosition = Vector3.zero;
        //atomsManager = GameObject.FindObjectOfType<AtomsManager>();
    }

    private void Update()
    {
        //impedisci all'ombra di non scendere sotto al pavimento
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = -1.8f;
        //clampedPosition.x = Mathf.Clamp(clampedPosition.x, clampedPosition.x - 2f, clampedPosition.x + 2f);
        transform.position = clampedPosition;
        
    }
/*
    void OnMouseDown()
    {
        atomsManager.SetStopTrue();
        renderer.material.color = selectedColor;
        lastMousePos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.position;
        pos.x -= delta.x * dragSpeed;
        transform.position = pos;
        transform.parent.position = new Vector3(pos.x, 
                                                transform.parent.position.y,
                                                transform.parent.position.z);
        lastMousePos = Input.mousePosition;
    }
    

    private void OnMouseUp()
    {
        
        renderer.material.color = color;
    }
    /*
    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7f);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //objPosition.x = transform.position.x;
        objPosition.z = transform.position.z;
        transform.position = objPosition;
        transform.parent.position = new Vector3(transform.position.x,
                                                transform.parent.position.y,
                                                transform.parent.position.z);
    }
    */
}
