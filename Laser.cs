using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
  [SerializeField] Transform startPoint;
  [SerializeField] Transform target;
  [SerializeField] float laserDistance;
  [SerializeField] float laserOffTime;
  [SerializeField] float laserOnTime;

  private LineRenderer lineRenderer;
  private bool isLaserOn;
  private float currentOffTime;
  private float currentOnTime;

  private void Start()
  {
    lineRenderer = GetComponent<LineRenderer>();
    isLaserOn = true;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Vector3 offsetY = new Vector3(0, 3, 0);
    Gizmos.DrawRay(startPoint.position, startPoint.forward * laserDistance);
  }

  private void Update()
  {
    if (isLaserOn)
    {
      lineRenderer.SetPosition(0, startPoint.position);
      lineRenderer.SetPosition(1, startPoint.position + startPoint.forward * laserDistance);
      Ray ray = new Ray(startPoint.position, startPoint.forward);
      if (Physics.Raycast(ray, out RaycastHit hit, laserDistance))
      {
        lineRenderer.SetPosition(1, hit.point);
        if (hit.transform.CompareTag("Player"))
        {
          hit.collider.GetComponent<Player>().OnLaserHit();
        }
      }
    }
    if (currentOffTime < laserOffTime)
    {
      currentOffTime += Time.deltaTime;
      isLaserOn = false;
      lineRenderer.enabled = false;
    }
    else if (currentOnTime < laserOnTime)
    {
      currentOnTime += Time.deltaTime;
      isLaserOn = true;
      lineRenderer.enabled = true;
    }
    else
    {
      currentOnTime = 0;
      currentOffTime = 0;
    }
  }
}
