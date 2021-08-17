using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Grabable : MonoBehaviour
{
  [SerializeField] Player player;
  private Rigidbody _rigidbody;

  private bool canGrab;

  private void Start()
  {
    _rigidbody = GetComponent<Rigidbody>();
  }

  private void Update()
  {
    if (player.isGrabbing)
    {
      Push();
      if (Input.GetKeyDown(KeyCode.E))
        LetGo();
    }
    else if (canGrab && Input.GetKeyDown(KeyCode.E))
    {
      UIManager.instance.TypeInteract("Press E to let go.");
      player.isGrabbing = true;
      _rigidbody.useGravity = false;
      canGrab = false;
    }
  }

  private void Push()
  {
    if (transform.position.y <= 0.125f)
      transform.position = Vector3.Lerp(transform.position, player.transform.position + player.transform.forward * 1.2f, 10f * Time.deltaTime);
    else
      transform.position = new Vector3(transform.position.x, 0.125f, transform.position.z);
  }

  private void LetGo()
  {
    UIManager.instance.HideInteract();
    player.isGrabbing = false;
    _rigidbody.useGravity = true;
    _rigidbody.velocity = Vector3.zero;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      if (!player.isGrabbing)
        UIManager.instance.ShowInteract();
      canGrab = true;
    }

  }
  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Player") && !player.isGrabbing)
      UIManager.instance.HideInteract();
    canGrab = false;
  }
}
