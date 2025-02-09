using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory){ }
    public override void EnterState()
    {
        Debug.Log("we entered the grounded state");
        ctx.CurrentMovementY = ctx.GroundedGravity;
        ctx.AppliedMovementY = ctx.GroundedGravity; 
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {

    }
    public override void CheckSwitchStates()
    {
        //jump when button is pressed
        if (ctx.IsJumpPressed)
        {
            SwitchState(factory.Jump());
        }
    }
    public override void InitializeSubState()
    {

    }
}
