using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutscene : MonoBehaviour
{
  [SerializeField] PlayableDirector playableDirector;
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      playableDirector.Play();
      Object.Destroy(gameObject);
    }
  }
}
