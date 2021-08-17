using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRunning : MonoBehaviour
{
  [SerializeField] private AudioClip[] runningClips;
  private AudioSource audioSource;


  private void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  public bool isPlaying()
  {
    return audioSource.isPlaying;
  }
  public void running()
  {
    audioSource.clip = runningClips[Random.Range(0, runningClips.Length)];
    audioSource.Play();
    //audioSource.PlayOneShot(clips,1);
  }
  public void Jump()
  {
    AudioManager.instance.Play("Jump");
  }
  public void idle()
  {
    audioSource.Stop();
  }
}
