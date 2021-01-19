using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EmitterCone : MonoBehaviour
{
    private Vector3 newScale;
    private float zoom = 4f, power = 1f, lambda = 0.5f;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private Renderer _renderer;
    private static readonly int _Color = Shader.PropertyToID("_Color");

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    public void SetZoom(float z)
    {
        zoom = z;
    }

    public void SetPwr(float p)
    {
        power = p;
    }

    public void SetLambda(float l)
    {
        lambda = l;
        Debug.Log(lambda);
    }
    
    
    private void Update()
    {
        newScale = transform.localScale;
        
        //DA SISTEMARE DOPO AVER FATTO IL MODELLO DELL'EMITTER
        newScale.x = power/4 * Mathf.Sqrt(zoom);
        newScale.z = power/4 * Mathf.Sqrt(zoom);
        newScale.y = power/4 / Mathf.Sqrt(zoom);
            
         
        
        /*
        newScale.x = 1f + 5f / 29f * (xz - 1f); 
        newScale.y = 1.5f + 1.5f / 10f * y;
        newScale.z = 1f + 5f / 29f * (xz - 1f); */
        transform.localScale = newScale;
        SetEmissionColor();
    }

    private void SetEmissionColor()
    {
        _renderer.material.SetColor(_Color, 
            Color.HSVToRGB(0.75f + -0.75f/1.7f*(lambda-0.3f), 1f, 1f, true));
        _renderer.material.SetColor(EmissionColor, 
            Color.HSVToRGB(0.75f + -0.75f/1.7f*(lambda-0.3f), 1f, 1f, true));
    }
}
