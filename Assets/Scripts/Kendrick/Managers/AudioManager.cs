using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixerGroup mixer;
    public Sound[] sounds;
    //public AudioClip[] whooshSounds;
    private void Awake()
    {
        instance = this.GetComponent<AudioManager>();
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = mixer;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.Play();
    }
    public void Play(string name, float pitchMin, float pitchMax)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.pitch = Random.Range(pitchMin, pitchMax);
        s.source.Play();
    }
    public void PlayOneshot(string name, float pitchMin, float pitchMax)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.pitch = Random.Range(pitchMin, pitchMax);
        s.source.PlayOneShot(s.clip);
    }
    public void PlayRandomWhoosh()
    {
        //int i = Random.Range(0, whooshSounds.Length);
        
    }
}
