using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockActivator : MonoBehaviour
{
  [SerializeField] GameObject door;
  [SerializeField] BoxCollider boxCollider;
  Player player;

  private void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
  }

  private void OnTriggerStay(Collider other)
  {
    if (player.hasKey)
    {
      UIManager.instance.TypeInteract("Press E to open the gate.");
      if (Input.GetKeyDown(KeyCode.E))
      {
        OpenDoor();
        UIManager.instance.HideInteract();
      }
    }
  }

  private void OnTriggerExit(Collider other)
  {
    UIManager.instance.HideInteract();
  }

  private void OpenDoor()
  {
    door.transform.localEulerAngles = new Vector3(90, 0, -90);
    boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y, 0.9510078f);
    boxCollider.size = new Vector3(boxCollider.center.x, boxCollider.center.y, 1.218079f);
    Object.Destroy(gameObject);
  }
}
