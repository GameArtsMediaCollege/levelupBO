using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class PlayerJumpState : PlayerBaseState
{

    IEnumerator IJumpRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        ctx.JumpCount = 0;
    }
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("entered the jumpstate");
        HandleJump();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        ctx.Animator.SetBool(ctx.IsJumpingHash, false);
        ctx.IsJumpingAnimating = false;
        ctx.CurrentJumpResetRoutine = ctx.StartCoroutine(IJumpRoutine());
        if (ctx.JumpCount == 3)
        {
            ctx.JumpCount = 0;
            ctx.Animator.SetInteger(ctx.JumpCountHash, ctx.JumpCount);
        }
    }
    public override void CheckSwitchStates()
    {
        if (ctx.CharacterController.isGrounded)
        {
            SwitchState(factory.Grounded());
        }
    }
    public override void InitializeSubState()
    {

    }

    void HandleJump()
    {
        if (ctx.JumpCount < 3 && ctx.CurrentJumpResetRoutine != null)
        {
            ctx.StopCoroutine(ctx.CurrentJumpResetRoutine);
        }
        ctx.Animator.SetBool(ctx.IsJumpingHash, true);
        ctx.IsJumpingAnimating = true;
        ctx.IsJumping = true;
        ctx.JumpCount += 1;
        ctx.Animator.SetInteger(ctx.JumpCountHash, ctx.JumpCount);
        ctx.CurrentMovementY = ctx.InitialJumpVelocities[ctx.JumpCount];
        ctx.AppliedMovementY = ctx.InitialJumpVelocities[ctx.JumpCount];
    }

    void HandleGravity()
    {
        bool isfalling = ctx.CurrentMovementY <= 0.0f || !ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isfalling)
        {
            float previousYVelocity = ctx.CurrentMovementY;
            ctx.CurrentMovementY = ctx.CurrentMovementY + (ctx.JumpGravities[ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            ctx.AppliedMovementY = Mathf.Max((previousYVelocity + ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
        else
        {
            float previousYVelocity = ctx.CurrentMovementY;
            ctx.CurrentMovementY = ctx.CurrentMovementY + (ctx.JumpGravities[ctx.JumpCount] * Time.deltaTime);
            ctx.AppliedMovementY = (previousYVelocity + ctx.CurrentMovementY) * 0.5f;
        }
    }


}

