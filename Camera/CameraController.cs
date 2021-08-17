using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [Header("Camera Controls")]
  public float mouseSensitivity = 1f;
  public float rotationSmoothTime = 120f;
  public Vector2 pitchMinMax = new Vector2(-48, 85);

  private Transform target;

  private Vector3 rotationSmoothVelocity;
  private Vector3 currentRotation;

  private float yaw;
  private float pitch;


  private void Awake()
  {
    target = GameObject.FindGameObjectWithTag("Camera Target").transform;
    yaw = target.eulerAngles.y;
  }

  private void Start()
  {
    HideAndLockCursor();
  }

  private void HideAndLockCursor()
  {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  public void SetSensitivity(float value)
  {
    mouseSensitivity = value;
  }

  private void Update()
  {
    yaw += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
    pitch -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
    pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

    currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), rotationSmoothTime * Time.deltaTime);
    transform.eulerAngles = currentRotation;

  }
  private void LateUpdate()
  {
    transform.position = Vector3.Lerp(transform.position, target.position, rotationSmoothTime * Time.deltaTime);
  }
}


