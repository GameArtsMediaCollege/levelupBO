

using System.Collections.Generic;
public class PlayerStateFactory
{
    enum PlayerStates
    {
        idle, 
        walk,
        run, 
        grounded,
        jump, 
        fall
    }

    PlayerStateMachine context;
    Dictionary<PlayerStates, PlayerBaseState> states = new Dictionary<PlayerStates, PlayerBaseState>();
    public PlayerStateFactory(PlayerStateMachine currentcontext)
    {
        context = currentcontext;
        states[PlayerStates.idle] = new PlayerIdleState(context, this);
        states[PlayerStates.walk] = new PlayerWalkState(context, this);
        states[PlayerStates.run] = new PlayerRunState(context, this);
        states[PlayerStates.jump] = new PlayerJumpState(context, this);
        states[PlayerStates.grounded] = new PlayerGroundedState(context, this);
        states[PlayerStates.fall] = new PlayerFallState(context, this);
    }
    public PlayerBaseState Idle()
    {
        return states[PlayerStates.idle];
    }
    public PlayerBaseState Walk()
    {
        return states[PlayerStates.walk];
    }
    public PlayerBaseState Run()
    {
        return states[PlayerStates.run];
    }
    public PlayerBaseState Jump()
    {
        return states[PlayerStates.jump];
    }
    public PlayerBaseState Grounded()
    {
        return states[PlayerStates.grounded];
    }
    public PlayerBaseState Fall()
    {
        return states[PlayerStates.fall];
    }
}
