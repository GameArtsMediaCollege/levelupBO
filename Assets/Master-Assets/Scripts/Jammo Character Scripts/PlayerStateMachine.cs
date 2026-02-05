using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    JammoInput playerinput;
    CharacterController charactercontroller;
    private Animator animator;

    int isWalkingHash;
    int isRunningHash;
    int isFallingHash;

    Vector2 currentmovementinput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    Vector3 cameraRelativeMovement;

    // --- Moving platform support (delta meenemen) ---
    private Transform currentPlatform;
    private Vector3 lastPlatformPos;
    private Vector3 accumulatedPlatformDelta;

    [SerializeField] private float platformStickDown = 2.0f;   // extra naar beneden om contact te houden
    [SerializeField] private float platformMaxStep = 1.0f;      // safety clamp per frame (optioneel)


    //movement varuiables
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;

    // constants
    float rotationFactorPerFrame = 15f;
    [Header("beweging")]
    [Range(1.0f, 10.0f)]
    [SerializeField] private float walkMultiplier = 3.0f;
    [Range(1.0f, 10.0f)]
    [SerializeField] private float runMultiplier = 5.0f;
    [Header("sprong")]
    [Range(0f, 100f)]
    [SerializeField] private float fallMultiplier = 2.0f;
    //gravity
    float gravity = -9.8f;
    float groundedgravity = -2f;

    //jump variables
    bool isJumpedPressed = false;
    float initialJumpVelocity;
    [Range(1.0f, 25.0f)]
    [SerializeField] float maxJumpHeight = 10f;

    [Range(0.25f, 5f)]
    [SerializeField] private float maxJumpTime = 0.75f;

    bool isJumping = false;
    int isJumpingHash;
    bool requirenewJumpPress = false;
    int jumpCount = 0;
    int jumpCountHash;

    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();

    Coroutine currentJumpResetRoutine = null;

    //state variables
    PlayerBaseState currentState;
    PlayerStateFactory states;

    //getters and setters
    public CharacterController CharacterController { get {  return charactercontroller; } }
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public Animator Animator { get { return animator; } }
    public Coroutine CurrentJumpResetRoutine { get { return currentJumpResetRoutine; } set { currentJumpResetRoutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities { get { return initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities {  get { return jumpGravities;  } }

    public Vector2 MovementInput { get { return currentmovementinput; } }
    public int JumpCount { get { return jumpCount; } set { jumpCount = value; } }
    public int IsWalkingHash { get { return isWalkingHash; } }
    public int IsRunningHash { get { return isRunningHash; } }
    public int IsJumpingHash { get { return isJumpingHash; } }
    public int IsFallingHash {  get { return isFallingHash; } }
    public int JumpCountHash {  get { return jumpCountHash; } }
    public bool RequireNewJumpPress { get { return requirenewJumpPress; } set {  requirenewJumpPress = value; } }
    public bool IsJumping { set { isJumping = value; } } 
    public bool IsJumpPressed { get { return  isJumpPressed; } }
    public float MaxJumpTie {  get { return maxJumpTime; } }
    public float MaxJumpHeight {  get { return maxJumpHeight; } }
    public bool IsRunPressed { get { return isRunPressed; } }
    public bool IsMovementPressed {  get { return isMovementPressed; } }
    public float GroundedGravity { get { return groundedgravity; } }
    public float Gravity { get { return gravity; } }
    public float CurrentMovementY {  get { return currentMovement.y;  } set { currentMovement.y = value; } }
    public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; } }
    public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; } }
    public float RunMultiplier { get { return runMultiplier; } }
    public float WalkMultiplier { get { return walkMultiplier; } }
    public float FallMultiplier { get { return fallMultiplier; } }
    public Vector2 CurrentMovementInput {  get { return currentmovementinput; } }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerinput = new JammoInput();
        charactercontroller = GetComponent<CharacterController>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isFallingHash = Animator.StringToHash("isFalling");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        //setup the states
        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        playerinput.CharacterControls.Move.started += OnMovementInput;
        playerinput.CharacterControls.Move.canceled += OnMovementInput;
        playerinput.CharacterControls.Move.performed += OnMovementInput;
        playerinput.CharacterControls.Run.started += OnRun;
        playerinput.CharacterControls.Run.canceled += OnRun;
        playerinput.CharacterControls.Jump.started += OnJump;
        playerinput.CharacterControls.Jump.canceled += OnJump;

        setupJumpVariables();
    }

    void setupJumpVariables()
    {
        float timetoApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timetoApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timetoApex;
        float secondJumpGravity = (-2 * (maxJumpHeight * 1.5f)) / Mathf.Pow((timetoApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight * 1.5f)) / (timetoApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight * 2f)) / Mathf.Pow((timetoApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight * 2f)) / (timetoApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        JumpGravities.Add(0, gravity);
        JumpGravities.Add(1, gravity);
        JumpGravities.Add(2, secondJumpGravity);
        JumpGravities.Add(3, thirdJumpGravity);
    }

    void Update()
    {
        // 1) states + rotatie (zoals jij al deed)
        handleRotation();
        CurrentState.UpdateStates();

        // 2) Jouw bestaande movement: appliedMovement -> camera relative
        cameraRelativeMovement = ConvertToCameraSpace(appliedMovement);

        // 4) Totale beweging in 1 Move call
        charactercontroller.Move(cameraRelativeMovement * Time.deltaTime);

        // 6) Als we écht los zijn: platform loslaten
        if (!charactercontroller.isGrounded)
        {
            // je kunt dit strenger/soepeler maken met een grace timer,
            // maar dit is de simpele versie
            currentPlatform = null;
            accumulatedPlatformDelta = Vector3.zero;
        }
    }







    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraforward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraforward.y = 0;
        cameraRight.y = 0;

        cameraforward = cameraforward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZproduct = vectorToRotate.z * cameraforward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZproduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requirenewJumpPress = false;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentmovementinput = context.ReadValue<Vector2>();

        currentMovement.x = currentmovementinput.x * walkMultiplier;
        currentMovement.z = currentmovementinput.y * walkMultiplier;
        currentRunMovement.x = currentmovementinput.x * runMultiplier;
        currentRunMovement.z = currentmovementinput.y * runMultiplier;
        isMovementPressed = currentmovementinput.x != 0 || currentmovementinput.y != 0;
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        //this is the location we are going toward but without the y axis
        positionToLookAt.x = cameraRelativeMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = cameraRelativeMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Alleen als we echt "bovenop" iets staan
        if (hit.moveDirection.y < -0.5f)
        {
            if(hit.transform.tag == "MovingPlatform")
            {
                Debug.Log("Jammo is op een platform");
                if (currentPlatform != hit.transform)
                {
                    currentPlatform = hit.transform;
                    lastPlatformPos = currentPlatform.position;
                    accumulatedPlatformDelta = Vector3.zero;
                }
            }
        }
    }


    private void OnEnable()
    {
        playerinput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerinput.CharacterControls.Disable();
    }
}
