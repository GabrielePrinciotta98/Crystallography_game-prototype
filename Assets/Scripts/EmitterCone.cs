using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EmitterCone : MonoBehaviour
{
    public bool GameStart { get; set; }

    private Vector3 newScale, newPos;
    private float zoom = 4f, power = 1f, lambda = 0.5f;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private Renderer _renderer;
    private Collider _collider;
    private static readonly int _Color = Shader.PropertyToID("_Color");
    private bool PowerOn = false;
    private float PowerLevel;
    public Tripod tripod;
    
    private void Start()
    {
        
        _renderer = GetComponentInChildren<Renderer>();
        _renderer.enabled = false;

        _collider = GetComponent<Collider>();
        _collider.enabled = false;

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
    }
    
    
    private void Update()
    {
        if (GameStart)
        {
            _collider.enabled = true;
        }
        
        if (!PowerOn) return;
        if (Math.Abs(PowerLevel - 0.5f) > 0.0001f)
        {
            PowerLevel += 0.002f;
        }

        {
            newScale = transform.localScale;
            //newPos = transform.localPosition;
            //newPos.z = -0.25f - 0.4f / 9f * (power - 1f);

            newScale.x = 1.5f - 2 * (0.30f + 0.35f / 9f * (zoom - 1f));// + 0.05f / 9f * (power - 1f);
            //newScale.y = 0.3f + 0.25f + 0.35f / 9f * (power - 1f);
            newScale.z = 1.5f - 2 * (0.30f + 0.35f / 9f * (zoom - 1f));//+ 0.05f / 9f * (power - 1f);
            transform.localScale = newScale;
            //transform.localPosition = newPos;
        }

        SetEmissionColor(PowerLevel);

    }

    private void SetEmissionColor(float alpha)
    {
        Material mat = _renderer.material;
        mat.SetColor(_Color, 
            Color.HSVToRGB(0.75f/1.7f*(lambda-0.3f), 0.2f+alpha, 1f, true));
        mat.SetColor(EmissionColor, 
            Color.HSVToRGB(0.75f/1.7f*(lambda-0.3f), 0.4f+alpha, 1f, true));
        var color = mat.color;
        mat.SetColor(_Color,
            new Color(color.r, color.g, color.b, 0.4f + 0.6f*(power-1)/9));
    }

    private void OnMouseDown()
    {
        PowerOn = true;
        _renderer.enabled = true;
        tripod.Click();
        Debug.Log("click");
    }

    private void OnMouseEnter()
    {
        tripod.ChangeMaterial(1);
    }

    private void OnMouseExit()
    {
        tripod.ChangeMaterial(0);
        
    }

    public bool GetPowerOn()
    {
        return PowerOn;
    }
}
