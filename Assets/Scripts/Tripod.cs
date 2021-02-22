using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripod : MonoBehaviour
{
    public Material[] materials;
    private Renderer _renderer;
    private bool clicked = false;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void ChangeMaterial(int i)
    {
        if (!clicked)
            _renderer.sharedMaterial = materials[i];
        else
        {
            _renderer.sharedMaterial = materials[0];
        }
    }

    public void Click()
    {
        clicked = true;
    }
}
