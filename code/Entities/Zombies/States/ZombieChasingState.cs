using Survivor.StateMachine;

namespace Survivor.Entities.Zombies.States;

public class ZombieChasingState : BaseState
{
	public ZombieChasingState( DefaultStateMachine defaultStateMachine, StateFactory stateFactory ) : base( defaultStateMachine, stateFactory )
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
