using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineActivator : MonoBehaviour
{
  [SerializeField] PlayableDirector playableDirector;

  private void OnTriggerStay(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      if (Input.GetKeyDown(KeyCode.E))
      {
        playableDirector.Play();
        Object.Destroy(GetComponent<BoxCollider>());
      }
    }
  }
}
