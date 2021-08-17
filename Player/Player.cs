using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
  [Header("Movement Options")]
  [SerializeField] float normalSpeed;
  [SerializeField] float runningSpeed;
  [SerializeField] float crouchSpeed;
  [SerializeField] float grabSpeed;
  [SerializeField] float idleJumpHeight = 5f;
  [SerializeField] float runningJumpHeight = 10f;
  [SerializeField] float gravity = 50f;

  [Header("Extra Options")]
  [SerializeField] Transform UFO;
  [SerializeField] float sphereRadius;
  [SerializeField] LayerMask enemyMask;
  [SerializeField] GameObject lightThing;

  [Header("Animation Options")]
  [SerializeField] float acceleration = 5f;
  [SerializeField] float deceleration = 20f;

  private Animator myAnimator;
  private CapsuleCollider _collider;
  private CharacterController controller;
  private Camera mainCam;
  private Vector3 _velocity;
  private Vector3 inputDirection;
  private Transform playerHead;
  private Transform checkpointTransform;
  private Vector3 wrapPosition;
  private float currentSpeed;

  //Bool Variables
  public bool hasKey;
  public bool hasLightSaber;
  public bool isCaught;
  private bool startFlying;
  public bool isGrabbing;
  private bool isWalking;
  private bool isRunning;
  private bool isCrouching;
  private bool canJump;
  private bool canPlayJumpLandingSound;

  //Hash
  private int speedID;
  private int jumpID;
  private int crouchID;
  private int grabID;
  private Vector2Int velocityID;


  private void Awake()
  {
    speedID = Animator.StringToHash("Speed");
    jumpID = Animator.StringToHash("Jump");
    crouchID = Animator.StringToHash("isCrouching");
    grabID = Animator.StringToHash("isGrabbing");
    velocityID = new Vector2Int(Animator.StringToHash("VelocityX"), Animator.StringToHash("VelocityY"));
  }

  private void Start()
  {
    myAnimator = GetComponentInChildren<Animator>();
    _collider = GetComponent<CapsuleCollider>();
    controller = GetComponent<CharacterController>();
    playerHead = GameObject.FindGameObjectWithTag("Player Head").transform;
    checkpointTransform = GameObject.FindGameObjectWithTag("Checkpoint")?.transform;

    currentSpeed = normalSpeed;
    _velocity = Vector3.zero;
    mainCam = Camera.main;

    GuardController.OnGuardHasSpottedPlayer += GuardFound;
  }

  private void Update()
  {
    _velocity.y -= gravity * Time.deltaTime;
    if (controller.enabled == true)
      controller.Move(_velocity * Time.deltaTime);

    if (isCaught) return;

    if (controller.isGrounded)
    {
      GetInput();
      if (isWalking)
      {
        if (isRunning && !isCrouching && !isGrabbing)
          Run();
        else if (!isGrabbing && isCrouching)
          Crouch();
        else if (isGrabbing)
          Grab();
        else
          Walk();
      }
      else
      {
        currentSpeed = Mathf.Lerp(currentSpeed, 0.0f, deceleration * Time.deltaTime);
      }

      if (isCrouching)
      {
        controller.height = 1.4f;
        controller.center = new Vector3(0f, 0.67f, 0f);
        playerHead.localPosition = new Vector3(playerHead.localPosition.x, 1.2f, playerHead.localPosition.z);
      }
      else
      {
        controller.height = 1.89f;
        controller.center = new Vector3(0f, 0.92f, 0f);
        playerHead.localPosition = new Vector3(playerHead.localPosition.x, 1.6875f, playerHead.localPosition.z);
      }

      if (!isCrouching && !isGrabbing && canJump && idleJumpHeight > 0f && runningJumpHeight > 0f)
        Jump();
    }
    RotateTowardsCam();


    myAnimator.SetFloat(velocityID.x, inputDirection.x, 0.1f, Time.deltaTime);
    myAnimator.SetFloat(velocityID.y, inputDirection.z, 0.1f, Time.deltaTime);
    myAnimator.SetFloat(speedID, currentSpeed / runningSpeed);
    myAnimator.SetBool(crouchID, isCrouching);
    myAnimator.SetBool(grabID, isGrabbing);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Ground") && !controller.isGrounded)
    {
      canPlayJumpLandingSound = true;
      if (!AudioManager.instance.isPlaying("Jump") && canPlayJumpLandingSound)
      {
        canPlayJumpLandingSound = false;
        AudioManager.instance.Play("Jump");
      }
    }

  }

  private void GetInput()
  {
    inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;
    _velocity = inputDirection * currentSpeed;
    _velocity = transform.TransformDirection(_velocity);

    isWalking = inputDirection.magnitude > 0;
    isRunning = Input.GetKey(KeyCode.LeftShift);
    isCrouching = Input.GetKey(KeyCode.C);
    canJump = Input.GetKeyDown(KeyCode.Space);
  }

  private void RotateTowardsCam()
  {
    if (mainCam)
    {
      Vector3 e = mainCam.transform.eulerAngles;
      e.x = 0;
      transform.eulerAngles = e;
    }
  }

  private void Walk()
  {
    currentSpeed = Mathf.Lerp(currentSpeed, normalSpeed, acceleration * Time.deltaTime);
  }

  private void Run()
  {
    currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, acceleration * Time.deltaTime);
  }

  private void Crouch()
  {
    currentSpeed = Mathf.Lerp(currentSpeed, crouchSpeed, acceleration * Time.deltaTime);
  }
  private void Grab()
  {
    currentSpeed = Mathf.Lerp(currentSpeed, grabSpeed, acceleration * Time.deltaTime);
    _velocity.y = 0.0f;
  }

  private void Jump()
  {
    _velocity.y = isWalking ? runningJumpHeight : idleJumpHeight;
    myAnimator.SetTrigger(jumpID);
  }

  public void StartFlying()
  {
    startFlying = true;
    controller.enabled = false;
    transform.position = Vector3.MoveTowards(transform.position, UFO.transform.position, normalSpeed * Time.deltaTime);
  }

  public void PlayAnimation(string animationName)
  {
    myAnimator.Play(animationName);
  }

  public float GetAnimationLength(string animationName)
  {
    var clips = myAnimator.runtimeAnimatorController.animationClips;
    foreach (AnimationClip clip in clips)
    {
      if (clip.name == animationName)
        return clip.length;
    }
    return 0;
  }

  public void ShowLightThing()
  {
    GameObject.FindGameObjectWithTag("Finish").SetActive(false);
    GameObject trigger = GameObject.FindGameObjectWithTag("Trigger");
    for (int i = 0; i < trigger.transform.childCount; i++)
    {
      trigger.transform.GetChild(i).gameObject.SetActive(true);
    }
    hasLightSaber = true;
    lightThing.SetActive(true);
  }


  public void StopMove()
  {
    isCaught = true;
    myAnimator.SetFloat(speedID, 0);
    _velocity.x = 0;
    _velocity.z = 0;
  }

  public void StartMove()
  {
    isCaught = false;
    myAnimator.Play("Movement");
  }

  public void Attack(GameObject searchDialogueBox)
  {
    myAnimator.Play("Attack");
    StartCoroutine(WaitForTime(1.5f, searchDialogueBox));
    Collider[] hitEnemies = Physics.OverlapSphere(transform.position, sphereRadius, enemyMask);
    foreach (Collider hit in hitEnemies)
    {
      GuardController guard = hit.GetComponent<GuardController>();
      guard.Faint();
    }
  }

  IEnumerator WaitForTime(float sec, GameObject searchBox)
  {
    yield return new WaitForSeconds(sec);
    searchBox.SetActive(true);
  }

  private void LateUpdate()
  {
    if (wrapPosition != Vector3.zero)
    {
      transform.position = wrapPosition;
      wrapPosition = Vector3.zero;
      controller.enabled = true;
      if (Vector3.Distance(transform.position, checkpointTransform.position) < 0.5f)
        StartMove();
    }
    if (startFlying)
      StartFlying();
  }

  private void GuardFound()
  {
    StopMove();
    myAnimator.Play("Scared");
  }


  public void OnLaserHit()
  {
    StopMove();
    myAnimator.Play("Die");
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    UIManager.instance.showUIPages(UIManager.UIPages.RespawnMenu);
  }

  public void Respawn()
  {
    controller.enabled = false;
    isGrabbing = false;
    wrapPosition = checkpointTransform.position;
  }

  private void OnDestroy()
  {
    GuardController.OnGuardHasSpottedPlayer -= GuardFound;
  }

  /*	public void RespawnGame(){
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      SendPlayerToCheckPoint();

      }
    public void SendPlayerToCheckPoint(){
      var checkpointManager = FindObjectOfType <CheckpointManager>();
          var checkpoint = checkpointManager.LastCheckpoint();
          var player = FindObjectOfType<Player>();

          player.transform.position = checkpoint.transform.position;

    }
    */
}
