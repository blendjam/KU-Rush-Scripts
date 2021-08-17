using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
  private Transform respawnPoint;

  private void Awake()
  {
    respawnPoint = GameObject.FindGameObjectWithTag("Checkpoint").transform;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
      respawnPoint.position = transform.position;
  }

}
