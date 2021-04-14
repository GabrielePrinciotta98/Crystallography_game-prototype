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
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        //impedisci all'ombra di non scendere sotto al pavimento
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = -1.8f;
        transform.position = clampedPosition;
        
    }
}
