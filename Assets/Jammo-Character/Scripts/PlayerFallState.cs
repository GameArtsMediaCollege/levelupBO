using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerstateFactory): base (currentContext, playerstateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Ctx.Animator.SetBool(Ctx.IsFallingHash, true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleGravity();
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool(Ctx.IsFallingHash, false);
    }

    void HandleGravity()
    {
        float previousYveloity = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY = Ctx.CurrentMovementY + Ctx.Gravity * Time.deltaTime;
        Ctx.AppliedMovementY = Mathf.Max((previousYveloity + Ctx.CurrentMovementY) * 0.5f, -20.0f);

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
}
