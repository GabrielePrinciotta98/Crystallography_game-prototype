using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Sound
{

    [SerializeField] private string name;
    [SerializeField] private AudioClip clip;
    
    [Range(0f, 1f)]
    [SerializeField] private float volume;
    [Range(0.1f, 3f)]
    [SerializeField] private float pitch;

    private AudioSource source;

    public AudioClip Clip
    {
        get => clip;
        set => clip = value;
    }

    public float Volume
    {
        get => volume;
        set => volume = value;
    }

    public float Pitch
    {
        get => pitch;
        set => pitch = value;
    }

    public AudioSource Source
    {
        get => source;
        set => source = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }
}
