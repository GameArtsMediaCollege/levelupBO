using UnityEngine;
using System.Collections;

public class PlayerJumpState : PlayerBaseState, iRootState
{

    IEnumerator IJumpRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        Ctx.JumpCount = 0;
    }
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }
    public override void EnterState()
    {
        InitializeSubState();
        setupJumpVariables();
        HandleJump();
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
        if (Ctx.IsJumpPressed)
        {
            Ctx.RequireNewJumpPress = true;
        }
        Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(IJumpRoutine());
        if (Ctx.JumpCount == 3)
        {
            Ctx.JumpCount = 0;
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        }
    }
    public override void CheckSwitchStates()
    {
        if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }
    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }

    void setupJumpVariables()
    {
        float timetoApex = Ctx.MaxJumpTie / 2;
        float gravity = (-2 * Ctx.MaxJumpHeight) / Mathf.Pow(timetoApex, 2);
        float initialJumpVelocity = (2 * Ctx.MaxJumpHeight) / timetoApex;
        float secondJumpGravity = (-2 * (Ctx.MaxJumpHeight * 1.5f)) / Mathf.Pow((timetoApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (Ctx.MaxJumpHeight * 1.5f)) / (timetoApex * 1.25f);
        float thirdJumpGravity = (-2 * (Ctx.MaxJumpHeight * 2f)) / Mathf.Pow((timetoApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (Ctx.MaxJumpHeight * 2f)) / (timetoApex * 1.5f);

        Ctx.InitialJumpVelocities[1] = initialJumpVelocity;
        Ctx.InitialJumpVelocities[2] = secondJumpInitialVelocity;
        Ctx.InitialJumpVelocities[3] = thirdJumpInitialVelocity;

        Ctx.JumpGravities[0] = gravity;
        Ctx.JumpGravities[1] = gravity;
        Ctx.JumpGravities[2] = secondJumpGravity;
        Ctx.JumpGravities[3] = thirdJumpGravity;
    }

    void HandleJump()
    {
        if (Ctx.JumpCount < 3 && Ctx.CurrentJumpResetRoutine != null)
        {
            Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
        }
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);
        Ctx.IsJumping = true;
        Ctx.JumpCount += 1;
        Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        //the actual jump
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
    }

    public void HandleGravity()
    {
        bool isfalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        float fallMultiplier = Ctx.FallMultiplier;

        if (isfalling)
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * 0.5f;
        }
    }


}

