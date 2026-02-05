
public abstract class PlayerBaseState
{
    private bool isRootState = false;
    private PlayerStateMachine ctx;
    private PlayerStateFactory factory;
    private PlayerBaseState currentSubstate;
    private PlayerBaseState currentSuperState;

    protected bool IsRootState {  set { isRootState = value; } }
    protected PlayerStateMachine Ctx {  get { return ctx; } }
    protected PlayerStateFactory Factory { get { return factory; } }

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

    public void UpdateStates()
    {
        UpdateState();
        if(currentSubstate != null)
        {
            currentSubstate.UpdateStates();
        }
    }

    public void ExitStates()
    {
        ExitState();
        if(currentSubstate != null)
        {
            currentSubstate.ExitStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        //currentstate exits state
        ExitState();
        //new state enters state
        newState.EnterState();
        if (isRootState)
        {
            //switch currentstate of context
            ctx.CurrentState = newState;
        }
        else if(currentSuperState != null)
        {
            currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubstate)
    {
        currentSubstate = newSubstate;
        newSubstate.SetSuperState(this);
    }

}
