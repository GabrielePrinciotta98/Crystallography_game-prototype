using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    //private float dragSpeed = 0.02f;
    //Vector3 lastMousePos;

    private void Awake()
    {
        transform.localPosition = new Vector3(0, -1.9f, 0);
    }

    private void Update()
    {
        //impedisci all'ombra di non scendere sotto al pavimento
        transform.position = new Vector3(transform.position.x, 0.1f,
                                         transform.position.z);
    }
    /*
    void OnMouseDown()
    {
        lastMousePos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.position;
        pos.x += delta.x * dragSpeed;
        transform.position = pos;
        transform.parent.position = new Vector3(pos.x, 
                                                transform.parent.position.y,
                                                transform.parent.position.z);
        lastMousePos = Input.mousePosition;
    }
    */
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

}
