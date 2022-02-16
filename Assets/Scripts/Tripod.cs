using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripod : MonoBehaviour
{
    public Material[] materials;
    private Renderer _renderer;
    private bool clicked = false;
    private AudioManager audioManager;
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void ChangeMaterial(int i)
    {
        _renderer.sharedMaterial = !clicked ? materials[i] : materials[0];
    }

    public void Click()
    {
        audioManager.PlayOneShot("EmitterOn");
        clicked = true;
    }
}
