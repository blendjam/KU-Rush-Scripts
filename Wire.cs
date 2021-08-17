using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
  [SerializeField] LineRenderer lineRenderer;
  public Transform endWire;

  private bool isDragging;
  private bool isConnected;
  private Vector3 originalPosition;
  private Camera wireCam;
  private Camera mainCam;

  [Header("Audio Clips")]
  [SerializeField] AudioClip selectSound;
  [SerializeField] AudioClip errorSound;
  [SerializeField] AudioClip connectSound;

  private void Awake()
  {
    wireCam = GameObject.FindGameObjectWithTag("WireCamera").GetComponent<Camera>();
    mainCam = Camera.main;
  }

  private void Start()
  {
    originalPosition = transform.position;
  }

  private void Update()
  {
    if (isDragging)
    {
      Vector3 mousePos = wireCam.ScreenToWorldPoint(Input.mousePosition);
      float distance = Vector2.Distance(mousePos, endWire.position);
      MoveToPos(mousePos);
      if (distance < 1f)
      {
        isConnected = true;
        MoveToPos(endWire.position - new Vector3(0.1f, 0, 0));
      }
      else
        isConnected = false;
    }
    else if (Vector2.Distance(transform.position, endWire.position) > 1f)
    {
      ResetPosition();
    }
  }

  private void MoveToPos(Vector3 newPos)
  {
    newPos.z = transform.position.z;
    transform.position = newPos;
    Vector3 posDiff = newPos - lineRenderer.transform.position;
    posDiff.z = 0;
    lineRenderer.SetPosition(2, posDiff - new Vector3(0.7f, 0, 0));
    lineRenderer.SetPosition(3, posDiff - new Vector3(0.2f, 0, 0));
  }

  public void ResetPosition()
  {
    MoveToPos(originalPosition);
    isConnected = false;
  }

  private void OnMouseUp()
  {
    isDragging = false;
    if (isConnected)
      AudioSource.PlayClipAtPoint(connectSound, mainCam.transform.position);
    else
      AudioSource.PlayClipAtPoint(errorSound, mainCam.transform.position);
  }

  private void OnMouseDown()
  {
    isDragging = true;
    AudioSource.PlayClipAtPoint(selectSound, mainCam.transform.position);
  }

  public bool IsConnected()
  {
    return isConnected;
  }

}
