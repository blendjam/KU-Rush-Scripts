using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour
{
  [SerializeField] float doorSpeed;
  [SerializeField] float stopTime;
  [SerializeField] AudioClip doorOpenningSound;

  private Vector3 originalPosition;
  private Vector3 finalPosition;
  private bool canMove;
  private bool isButtonDoor;
  private float currentTime;
  private bool canOpen;

  private void Start()
  {
    originalPosition = transform.position;
    currentTime = 0;
    finalPosition = originalPosition + transform.right * 3.0f;
  }

  private void Update()
  {
    if (canOpen && Input.GetKeyDown(KeyCode.E))
    {
      OpenDoor(false);
      canOpen = false;
    }

    if (!isButtonDoor && transform.position == finalPosition)
      currentTime += Time.deltaTime;

    if (currentTime > stopTime)
    {
      currentTime = 0;
      canMove = false;
    }

    if (canMove)
      transform.position = Vector3.MoveTowards(transform.position, finalPosition, doorSpeed * Time.deltaTime);
    else
      transform.position = Vector3.MoveTowards(transform.position, originalPosition, doorSpeed * Time.deltaTime);

  }
  public void OpenDoor(bool buttonDoor)
  {
    isButtonDoor = buttonDoor;
    canMove = true;
    AudioSource.PlayClipAtPoint(doorOpenningSound, Camera.main.transform.position);
  }

  public void CloseDoor()
  {
    canMove = false;
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      UIManager.instance.HideInteract();
      canOpen = false;
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      UIManager.instance.TypeInteract("Press E to open.");
      canOpen = true;
    }
  }
}
