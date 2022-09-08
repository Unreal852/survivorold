namespace Survivor.StateMachine;

public abstract class BaseState
{
	protected BaseState( DefaultStateMachine defaultStateMachine, StateFactory stateFactory )
	{
		DefaultStateMachine = defaultStateMachine;
		StateFactory = stateFactory;
	}

	public DefaultStateMachine DefaultStateMachine { get; }
	public StateFactory        StateFactory        { get; }

	public abstract void OnEnterState();
	public abstract void OnUpdateState();
	public abstract void OnExitState();
	public abstract void CheckSwitchStates();
	public abstract void InitSubState();

	private void UpdateStates()
	{
	}

	protected void SwitchState( BaseState newState )
	{
		OnExitState();
		newState.OnEnterState();
		DefaultStateMachine.CurrentState = newState;
	}

	protected void SetSuperState()
	{
	}

	protected void SetSubState()
	{
	}
}
