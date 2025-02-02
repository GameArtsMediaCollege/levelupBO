using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; 
public class CharacterMovementScript : MonoBehaviour
{
    JammoInput playerinput;
    CharacterController charactercontroller;
    public Animator animator;

    int isWalkingHash;
    int isRunningHash;

    Vector2 currentmovementinput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
     
    //movement varuiables
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed; 

    // constants
    float rotationFactorPerFrame = 15f;
    float runMultiplier = 3.0f;

    //gravity
    float gravity = -9.8f;
    float groundedgravity = -0.05f;

    //jump variables
    bool isJumpedPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 10f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    int isJumpingHash;
    bool isJumpAnimating = false;
    int jumpCount = 0;
    int jumpCountHash;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> JumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetRoutine = null;

    private void Awake()
    {
        playerinput = new JammoInput();
        charactercontroller = GetComponent<CharacterController>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

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
        float secondJumpGravity = (-2 *(maxJumpHeight*1.5f)) / Mathf.Pow((timetoApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight*1.5f)) / (timetoApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight*2f)) / Mathf.Pow((timetoApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight*2f)) / (timetoApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        JumpGravities.Add(0, gravity);
        JumpGravities.Add(1, gravity);
        JumpGravities.Add(2, secondJumpGravity);
        JumpGravities.Add(3, thirdJumpGravity);
    }

    void handleJump()
    {
        if(!isJumping && charactercontroller.isGrounded && isJumpPressed)
        {
            if(jumpCount < 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount += 1;
            animator.SetInteger(jumpCountHash, jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount];
            appliedMovement.y = initialJumpVelocities[jumpCount];
        } else if(!isJumpedPressed && isJumping && charactercontroller.isGrounded)
        {
            isJumping = false;
        }
    }

    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        jumpCount = 0;
    }

    void handleGravity()
    {
        bool isfalling = currentMovement.y <= 0.0f || !isJumpedPressed;
        float fallMultiplier = 2.0f;


        if (charactercontroller.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                if(jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash, jumpCount);
                }
            }
            currentMovement.y = groundedgravity;
            currentMovement.y = groundedgravity;
        }
        else if (isfalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (JumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * 0.5f, -20.0f);
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (JumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * 0.5f;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        Debug.Log(isJumpPressed);
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnMovementInput (InputAction.CallbackContext context)
    {
        currentmovementinput = context.ReadValue<Vector2>();
        currentMovement.x = currentmovementinput.x;
        currentMovement.z = currentmovementinput.y;
        currentRunMovement.x = currentmovementinput.x * runMultiplier;
        currentRunMovement.z = currentmovementinput.y * runMultiplier;
        isMovementPressed = currentmovementinput.x != 0 || currentmovementinput.y != 0;
    }


    void Update()
    {
        handleRotation();
        handleAnimation();
        if (isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;  
        }

        charactercontroller.Move(appliedMovement * Time.deltaTime);

        handleGravity();
        handleJump();
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if(isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if(!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }

    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        //this is the location we are going toward but without the y axis
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;
        
        if(isMovementPressed )
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame);
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
