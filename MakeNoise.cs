using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MakeNoise : MonoBehaviour
{
  [SerializeField] Transform hand;
  [SerializeField] float maxAngle;
  [SerializeField] float speed;
  [SerializeField] float radius;
  [SerializeField] LayerMask enemyMask;

  private bool canRotate;
  private bool canTrunOn;
  private AudioSource audioSource;
  private GuardController guardController;

  private void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }

  private void Update()
  {
    if (canRotate && guardController != null)
    {
      hand.RotateAround(hand.transform.position, Vector3.up, Mathf.Sin(Time.time * speed) * maxAngle * Time.deltaTime);
      float distance = Vector3.Distance(transform.position, guardController.transform.position);
      if (distance < 2f)
      {
        guardController.StopRunning();
        StopRotate();
      }
    }
    else if (canTrunOn && Input.GetKeyDown(KeyCode.E))
    {
      TurnOn();
      canTrunOn = false;
    }
  }


  public void StopRotate()
  {
    canRotate = false;
    audioSource.Stop();
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(transform.position, radius);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      UIManager.instance.TypeInteract("Press E to make sound.");
      canTrunOn = true;
    }
  }
  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      UIManager.instance.HideInteract();
    }
  }

  private void TurnOn()
  {
    canRotate = true;
    Collider[] enemyHit = Physics.OverlapSphere(transform.position, radius, enemyMask);
    float minDistance = 0;
    int minIndex = 0;
    foreach (Collider hit in enemyHit)
    {
      float distance = Vector3.Distance(transform.position, hit.transform.position);
      if (distance < minDistance)
      {
        minDistance = distance;
      }
    }
    guardController = enemyHit[minIndex].GetComponent<GuardController>();
    guardController.MoveTowards(transform.position);
    audioSource.Play();
  }
}
