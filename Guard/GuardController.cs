using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
  #region INITIALISATION
  public static event System.Action OnGuardHasSpottedPlayer;

  [SerializeField] Transform pathHolder;
  [SerializeField] bool isSitting;
  [SerializeField] float speed;
  [SerializeField] float runningSpeed;
  [SerializeField] float rotationSpeed;
  [SerializeField] float timeBetweenEachWayPoint;
  [SerializeField] GameObject exclamation;
  [SerializeField] Color defaultColor;
  [SerializeField] Color foundColor;

  private Player _player;
  private Animator myAnimator;
  private Quaternion currentRotation;
  private Vector3[] wayPoints;
  private GuardSensor guardSensor;
  private int wayPointIndex = 0;
  private float currentTime = 0;
  private bool isDying;
  private bool isRunning;
  private Vector3 finalPosition;
  private NavMeshAgent navMeshAgent;
  private bool pauseMenuOn;

  #endregion


  private void Awake()
  {
    myAnimator = GetComponent<Animator>();
    guardSensor = GetComponentInChildren<GuardSensor>();
    _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    navMeshAgent = GetComponent<NavMeshAgent>();
  }
  private void Start()
  {
    wayPoints = new Vector3[pathHolder.childCount];
    for (int i = 0; i < wayPoints.Length; i++)
    {
      wayPoints[i] = pathHolder.GetChild(i).position;
    }
    transform.position = wayPoints[0];

  }

  private void Update()
  {

    if (guardSensor.CanSeePlayer())
    {
      exclamation.SetActive(true);
      guardSensor.MeshColor = foundColor;
      if (!_player.isCaught && OnGuardHasSpottedPlayer != null)
      {
        OnGuardHasSpottedPlayer();
      }
    }
    else
    {
      if (isSitting && !isDying)
        myAnimator.Play("Sitting Talking");
      else if (isRunning)
        navMeshAgent.SetDestination(finalPosition);
      else
        PatrolAround();
      exclamation.SetActive(false);
      guardSensor.MeshColor = defaultColor;
    }
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

  public void Faint()
  {
    myAnimator.SetLayerWeight(1, 0);
    myAnimator.Play("Die");
    isDying = true;
  }

  private void PatrolAround()
  {
    if (wayPoints.Length <= 1) return;

    if (transform.position == wayPoints[wayPointIndex])
    {
      Vector3 direction = wayPoints[(wayPointIndex + 1) % wayPoints.Length] - wayPoints[wayPointIndex];
      currentRotation = Quaternion.LookRotation(direction);
      currentTime += Time.deltaTime;
      myAnimator.SetBool("isWalking", false);
    }
    else
    {
      myAnimator.SetBool("isWalking", true);
    }
    if (currentTime > timeBetweenEachWayPoint)
    {
      currentTime = 0;
      wayPointIndex = (wayPointIndex + 1) % (wayPoints.Length);
    }
    transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, rotationSpeed * Time.deltaTime);
    transform.position = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex], speed * Time.deltaTime);
  }

  // private void _MoveTowards()
  // {
  //   Vector3 direction = pos - transform.position;
  //   currentRotation = Quaternion.LookRotation(direction);
  //   transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, rotationSpeed * Time.deltaTime);
  //   transform.position = Vector3.MoveTowards(transform.position, pos, runningSpeed * Time.deltaTime);
  // }

  public void StopRunning()
  {
    isRunning = false;
    myAnimator.SetBool("isRunning", false);
    finalPosition = wayPoints[0];
  }

  public void MoveTowards(Vector3 pos)
  {
    finalPosition = pos - new Vector3(1, 0, 1);
    finalPosition.y = 0;
    isRunning = true;
    myAnimator.SetBool("isRunning", true);
  }

  //This version is same as PatrolAround but easier to understand but works a lil bid different
  private void PatrolAroundV2()
  {
    if (wayPoints.Length < 0) return;
    if (Vector3.Distance(wayPoints[wayPointIndex], transform.position) < 0.1f)
    {
      wayPointIndex++;
      if (wayPointIndex >= wayPoints.Length)
      {
        wayPointIndex = 0;
      }
      myAnimator.SetBool("isWalking", false);
    }
    else
      myAnimator.SetBool("isWalking", true);
    Vector3 direction = wayPoints[wayPointIndex] - transform.position;
    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    transform.Translate(0, 0, speed * Time.deltaTime);
  }


}
