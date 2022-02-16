using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager audioManagerInstance;
    public static bool musicOn = true;
    public static bool sfxOn = true;
    [SerializeField] private Sound[] sounds;
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (audioManagerInstance == null)
            audioManagerInstance = this;
        else
            Destroy(gameObject);


        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            
        }
    }
    

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s.SoundType == SoundType.Music && musicOn || s.SoundType == SoundType.Sound && sfxOn)
            s.Source.Play();
    }
    
    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s.SoundType == SoundType.Music && musicOn || s.SoundType == SoundType.Sound && sfxOn)
            s.Source.PlayOneShot(s.Clip);
    }
    
    
    
    public void PlayInLoop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        s.Source.loop = enabled;
        if (s.SoundType == SoundType.Music && musicOn || s.SoundType == SoundType.Sound && sfxOn)
            s.Source.Play();
    }

    public void PlayAll(SoundType soundType)
    {
        if (soundType == SoundType.Music)
            musicOn = true;
        else
            sfxOn = true;
    }
    
    
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        s.Source.Stop();
    }

    public void StopAll(SoundType soundType)
    {
        if (soundType == SoundType.Music)
            musicOn = false;
        else
            sfxOn = false;
        foreach (var s in sounds)
        {
            if (s.SoundType == soundType && s.Source.isPlaying)
                s.Source.Stop();
        }
        
    }
}
