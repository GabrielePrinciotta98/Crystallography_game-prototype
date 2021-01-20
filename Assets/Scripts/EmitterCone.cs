using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EmitterCone : MonoBehaviour
{
    private Vector3 newScale, newPos;
    private float zoom = 4f, power = 1f, lambda = 0.5f;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private Renderer _renderer;
    private static readonly int _Color = Shader.PropertyToID("_Color");
    private float zPos;
    
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
        newPos = transform.localPosition;
        /*
        //DA SISTEMARE DOPO AVER FATTO IL MODELLO DELL'EMITTER
        newScale.x = power/10 * Mathf.Sqrt(zoom);
        newScale.z = power/10 * Mathf.Sqrt(zoom);
        newScale.y = power/10 / Mathf.Sqrt(zoom);
        */

        newPos.z = 1f + 0.6f / 9f * (power - 1f); 
        
        newScale.x = 1 + 0.7f / 9f * (zoom - 1f) + 0.7f / 9f * (power - 1f); 
        newScale.y = 1 + 0.7f / 9f * (power - 1f);
        newScale.z = 1 + 0.7f / 9f * (zoom - 1f) + 0.7f / 9f * (power - 1f);
        transform.localScale = newScale;
        transform.localPosition = newPos;
        SetEmissionColor();
    }

    private void SetEmissionColor()
    {
        _renderer.material.SetColor(_Color, 
            Color.HSVToRGB(0.75f/1.7f*(lambda-0.3f), 1f, 1f, true));
        _renderer.material.SetColor(EmissionColor, 
            Color.HSVToRGB(0.75f/1.7f*(lambda-0.3f), 1f, 1f, true));
    }
}
