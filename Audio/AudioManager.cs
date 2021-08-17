using UnityEngine.Audio;
using UnityEngine;
using System;


public class AudioManager : MonoBehaviour
{
  public Sound[] sounds;


  public static AudioManager instance;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(instance);
    }
    else
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);

    foreach (Sound s in sounds)
    {
      MakeAudioSource(s);
    }

  }

  private AudioSource MakeAudioSource(Sound s)
  {

    s.source = gameObject.AddComponent<AudioSource>();
    s.source.playOnAwake = false;
    s.source.clip = s.clip;
    s.source.volume = s.volume;
    s.source.pitch = s.pitch;
    s.source.loop = s.loop;
    s.source.spatialBlend = s.spatialBlend;
    return s.source;
  }

  public void Play(string soundName)
  {
    Sound s = Array.Find(sounds, sound => sound.clipName == soundName);

    if (s != null && s.source != null)
    {
      s.source.Play();
    }
    else
    {
      Debug.LogWarning("Sound: " + soundName + " not found!");
      return;
    }
  }

  public void Stop(string soundName)
  {
    Sound s = Array.Find(sounds, sound => sound.clipName == soundName);

    if (s != null)
    {
      s.source.Stop();
    }
    else
    {
      Debug.LogWarning("Sound: " + soundName + " not found!");
      return;
    }
  }

  public float GetVolume(string soundName)
  {
    Sound s = Array.Find(sounds, sound => sound.clipName == soundName);
    return s.source.volume;
  }

  public void SetVolume(string soundName, float newVolume)
  {
    Sound s = Array.Find(sounds, sound => sound.clipName == soundName);
    s.source.volume = newVolume;
  }

  public bool isPlaying(string soundName)
  {
    Sound s = Array.Find(sounds, sound => sound.clipName == soundName);
    return s.source.isPlaying;
  }
}
