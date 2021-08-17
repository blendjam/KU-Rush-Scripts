using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
  [SerializeField] float minDistance = 1f;
  [SerializeField] float maxDistance = 4.0f;
  [SerializeField] float smooth = 5;
  [SerializeField] float offset = 0.9f;
  [SerializeField] bool checkCollision;

  Vector3 dollyDir;
  private float distance;

  private void Awake()
  {
    dollyDir = transform.localPosition.normalized;
    distance = transform.localPosition.magnitude;
  }

  private void Update()
  {
    CheckCollision();
  }

  private void CheckCollision()
  {
    Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
    int playerInverseMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ignore Raycast"));
    if (checkCollision && Physics.Linecast(transform.parent.position, desiredCameraPos, out RaycastHit hit, playerInverseMask))
    {
      distance = Mathf.Clamp(hit.distance * offset, minDistance, maxDistance);
    }
    else
    {
      distance = maxDistance;
    }
    transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
  }

}
