using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
  [SerializeField] float speed;
  [SerializeField] float distance;

  private Vector3 newPos;
  private void Start()
  {
    newPos = transform.position;
  }

  private void Update()
  {
    newPos.y = transform.position.y + Mathf.Sin(Time.time * speed) * distance * Time.deltaTime;
    transform.position = newPos;
  }
}
