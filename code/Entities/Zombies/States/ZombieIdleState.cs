using Survivor.StateMachine;

namespace Survivor.Entities.Zombies.States;

public class ZombieIdleState : BaseState
{
	public ZombieIdleState( DefaultStateMachine defaultStateMachine, StateFactory stateFactory ) : base( defaultStateMachine, stateFactory )
	{
	}

	public override void OnEnterState()
	{
	}

	public override void OnUpdateState()
	{
		CheckSwitchStates();
		
	}

	public override void OnExitState()
	{
	}

	public override void CheckSwitchStates()
	{
	}

	public override void InitSubState()
	{
	}
}
