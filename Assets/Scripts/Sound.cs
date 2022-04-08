using System;
using UnityEngine;

[Serializable] public enum SoundType
{
    Music,
    Sound
}

[Serializable] public class Sound
{
    [SerializeField] private SoundType soundType;
    [SerializeField] private string name;
    [SerializeField] private AudioClip clip;
    
    [Range(0f, 2f)]
    [SerializeField] private float volume;
    [Range(0.1f, 3f)]
    [SerializeField] private float pitch;

    private AudioSource source;

    public SoundType SoundType
    {
        get => soundType;
        set => soundType = value;
    }
    
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
