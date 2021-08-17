using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
  [SerializeField] Door door;
  [SerializeField] Material material;

  private bool buttonPressed;

  private void Start()
  {
    material.color = Color.red;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!other.isTrigger)
      door.OpenDoor(true);
  }

  private void OnTriggerStay(Collider other)
  {
    if (!other.isTrigger)
      material.color = Color.green;
  }
  private void OnTriggerExit(Collider other)
  {
    if (!other.isTrigger)
    {
      door.CloseDoor();
      material.color = Color.red;
    }
  }
}
