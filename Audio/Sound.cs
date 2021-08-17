using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string clipName;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0f,1f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    [Range(0f,1f)]
    public float spatialBlend;
}
