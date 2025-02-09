

using TMPro.EditorUtilities;
using UnityEditor.Rendering.LookDev;
using System.Collections.Generic;
using System.Collections;
using System;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerstateFactory)
    {
        ctx = currentContext;
        factory = playerstateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    void UpdateStates()
    {

    }

    protected void SwitchState(PlayerBaseState newState)
    {
        //currentstate exits state
        ExitState();
        //new state enters state
        newState.EnterState();

        //switch currentstate of context
        ctx.CurrentState = newState;
    }

    protected void SetSuperState()
    {

    }

    protected void SetSubState()
    {

    }

}
