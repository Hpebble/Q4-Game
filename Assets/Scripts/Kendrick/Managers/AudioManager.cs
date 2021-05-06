using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixerGroup mixer;
    public Sound[] sounds;
    public Sound[] tracks;
    //public AudioClip[] whooshSounds;
    private void Awake()
    {
        AudioListener.volume = 0.5f;
        instance = this.GetComponent<AudioManager>();
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = mixer;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in tracks)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = mixer;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    public void Update()
    {
        foreach(Sound s in tracks)
        {
            if (s.fadeIn)
            {
                s.source.volume = Mathf.Lerp(s.source.volume, s.volume, s.fadeSpeed * Time.deltaTime);
            }
            else if (!s.fadeIn)
            {
                s.source.volume = Mathf.Lerp(s.source.volume, 0, s.fadeSpeed * Time.deltaTime);
                if(s.source.volume < 0.1f)
                {
                    s.source.Pause();
                }
            }
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
    public void ToggleFadeIn(string name)
    {
        Sound s = Array.Find(tracks, Sound => Sound.name == name);
        if (s.fadeIn == true)
        {
            FadeOut(name);
        }
        else
        {
            FadeIn(name);
        }
    }
    public void FadeIn(string name)
    {
        Sound s = Array.Find(tracks, Sound => Sound.name == name);
        s.fadeIn = true;
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
        else
        {
            s.source.UnPause();
        }
    }
    public void FadeOut(string name)
    {
        Sound s = Array.Find(tracks, Sound => Sound.name == name);
        s.fadeIn = false;
    }
    public void SetMixerVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    public void PlayRandomWhoosh()
    {
        //int i = Random.Range(0, whooshSounds.Length);
        
    }
}
