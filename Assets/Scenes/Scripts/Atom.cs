using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    /*
    
    private float dragSpeed = 0.02f;
    Vector3 lastMousePos;

    void OnMouseDown()
    {
        lastMousePos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.position;
        pos.z += delta.x * dragSpeed;
        pos.y += delta.y * dragSpeed;
        transform.position = pos;
        lastMousePos = Input.mousePosition;
    }*/

    AtomsManager atomsManager;

    private void Awake()
    {

        atomsManager = GameObject.FindObjectOfType<AtomsManager>();
        atomsManager.AddAtom(this);
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7f);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objPosition.x = transform.position.x;
        //objPosition.z = transform.position.z;
        transform.position = objPosition;
        atomsManager.SetMyPosition(this);
    }
}
