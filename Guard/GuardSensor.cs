using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GuardSensor : MonoBehaviour
{
  [SerializeField] float height;
  [SerializeField] [Range(1, 100)] int subdivisions = 8;
  [SerializeField] LayerMask viewMask;
  [SerializeField] GameObject cylinder;
  [SerializeField] Transform guardHead;

  public float viewAngle = 60f;
  public float viewDistance = 10f;
  public float minViewDistance = 0.5f;

  private Color meshColor;
  public Color MeshColor
  {
    get => meshColor;
    set => meshColor = value;
  }
  private Transform playerTransform;
  private Transform temp;
  private MeshFilter meshFilter;
  private MeshRenderer meshRenderer;
  private GuardSensor instance;
  private void Awake()
  {

    playerTransform = GameObject.FindGameObjectWithTag("Player Head").transform;
    meshFilter = GetComponent<MeshFilter>();
    meshRenderer = GetComponent<MeshRenderer>();
  }

  private void Start()
  {
    cylinder.GetComponent<MeshRenderer>().material = meshRenderer.material;
    cylinder.transform.localScale = new Vector3(minViewDistance, height, minViewDistance);
    if (meshFilter)
      meshFilter.mesh = CreateWedgeMesh();
  }

  private void Update()
  {
    if (meshRenderer)
    {
      meshRenderer.material.color = meshColor;
      meshRenderer.material.SetColor("_EmissionColor", meshColor);
    }
  }
  private void OnDrawGizmos()
  {
    Gizmos.DrawMesh(CreateWedgeMesh(), transform.position, transform.rotation, Vector3.one);
    cylinder.transform.localScale = new Vector3(minViewDistance, height, minViewDistance);
    // Gizmos.DrawLine(guardHead.position, playerTransform.position);
  }

  private Mesh CreateWedgeMesh()
  {
    Mesh mesh = new Mesh();
    int numTriangle = (subdivisions * 4) + 2 + 2;
    int numVertices = numTriangle * 3;

    Vector3[] vertices = new Vector3[numVertices];
    int[] trinagles = new int[numVertices];

    Vector3 bottomCenter = Vector3.zero;
    Vector3 bottomRight = Quaternion.Euler(0, viewAngle, 0) * Vector3.forward * viewDistance;
    Vector3 bottomLeft = Quaternion.Euler(0, -viewAngle, 0) * Vector3.forward * viewDistance;

    Vector3 topCenter = bottomCenter + Vector3.up * height;
    Vector3 topRight = bottomRight + Vector3.up * height;
    Vector3 topLeft = bottomLeft + Vector3.up * height;

    int vert = 0;

    //Left Side
    vertices[vert++] = bottomCenter;
    vertices[vert++] = bottomLeft;
    vertices[vert++] = topLeft;

    vertices[vert++] = topLeft;
    vertices[vert++] = topCenter;
    vertices[vert++] = bottomCenter;

    //Right Side
    vertices[vert++] = bottomCenter;
    vertices[vert++] = topCenter;
    vertices[vert++] = topRight;

    vertices[vert++] = topRight;
    vertices[vert++] = bottomRight;
    vertices[vert++] = bottomCenter;


    float currentAngle = -viewAngle;
    float deltaAngle = (viewAngle * 2) / subdivisions;
    for (int i = 0; i < subdivisions; i++)
    {
      bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * viewDistance;
      bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * viewDistance;

      topRight = bottomRight + Vector3.up * height;
      topLeft = bottomLeft + Vector3.up * height;

      //Far Side
      vertices[vert++] = bottomLeft;
      vertices[vert++] = bottomRight;
      vertices[vert++] = topRight;

      vertices[vert++] = topRight;
      vertices[vert++] = topLeft;
      vertices[vert++] = bottomLeft;

      //Top Side
      vertices[vert++] = topCenter;
      vertices[vert++] = topLeft;
      vertices[vert++] = topRight;

      //Bottom
      vertices[vert++] = bottomCenter;
      vertices[vert++] = bottomRight;
      vertices[vert++] = bottomLeft;

      currentAngle += deltaAngle;
    }


    for (int i = 0; i < numVertices; i++)
    {
      trinagles[i] = i;
    }
    mesh.vertices = vertices;
    mesh.triangles = trinagles;
    mesh.RecalculateNormals();
    return mesh;
  }

  public bool CanSeePlayer()
  {
    float offset = 0.5f;
    float distance = Vector3.Distance(playerTransform.position, transform.position);
    if (distance < minViewDistance + offset)
    {
      if (!Physics.Linecast(guardHead.position, playerTransform.position, viewMask))
      {
        return true;
      }
    }
    if (distance < viewDistance + offset)
    {
      Vector3 directionToPlayer = (playerTransform.position - guardHead.position).normalized;
      float angleBetnGuardAndPlayer = Vector3.Angle(guardHead.forward, directionToPlayer);
      if (angleBetnGuardAndPlayer < viewAngle + 4)
      {
        if (!Physics.Linecast(guardHead.position, playerTransform.position, viewMask))
        {
          return true;
        }
      }
    }
    return false;
  }
}
