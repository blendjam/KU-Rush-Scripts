using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
  [SerializeField] float speed;
  [SerializeField] float timeBetweenEachWayPoint;
  [SerializeField] Transform pathHolder;

  private Rigidbody _rigidbody;
  private int wayPointIndex = 0;
  private float currentTime = 0;
  private Vector3[] wayPoints;
  private CharacterController characterController;

  private void Start()
  {
    _rigidbody = GetComponent<Rigidbody>();
    wayPoints = new Vector3[pathHolder.childCount];
    for (int i = 0; i < wayPoints.Length; i++)
    {
      wayPoints[i] = pathHolder.GetChild(i).position;
    }
    transform.position = wayPoints[0];
  }

  private void PatrolAround()
  {
    if (wayPoints.Length <= 1) return;

    if (Vector3.Distance(_rigidbody.position, wayPoints[wayPointIndex]) < 0.5)
    {
      Vector3 direction = wayPoints[(wayPointIndex + 1) % wayPoints.Length] - wayPoints[wayPointIndex];
      currentTime += Time.deltaTime;
    }
    if (currentTime > timeBetweenEachWayPoint)
    {
      currentTime = 0;
      wayPointIndex = (wayPointIndex + 1) % (wayPoints.Length);
    }
    Vector3 currentPos = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex], speed * Time.deltaTime);
    _rigidbody.MovePosition(currentPos);
  }

  private void FixedUpdate()
  {
    PatrolAround();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      characterController = other.GetComponent<CharacterController>();
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.tag == "Player")
      characterController.Move(_rigidbody.velocity * Time.deltaTime * 0.35f);
  }

  private void OnDrawGizmos()
  {
    Vector3 startPosition = pathHolder.GetChild(0).position;
    Vector3 previousPosition = startPosition;
    foreach (Transform waypoint in pathHolder)
    {
      Gizmos.DrawSphere(waypoint.position, 0.3f);
      Gizmos.DrawLine(previousPosition, waypoint.position);
      previousPosition = waypoint.position;
    }
    Gizmos.DrawLine(previousPosition, startPosition);
  }
}
