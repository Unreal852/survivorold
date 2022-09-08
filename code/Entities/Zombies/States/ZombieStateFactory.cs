using Survivor.StateMachine;

namespace Survivor.Entities.Zombies.States;

public class ZombieStateFactory : StateFactory
{
	public ZombieStateFactory( DefaultStateMachine defaultStateMachine ) : base( defaultStateMachine )
	{
		Idle = new ZombieIdleState( DefaultStateMachine, this );
		Idle = new ZombieChasingState( DefaultStateMachine, this );
	}

	public BaseState Idle    { get; }
	public BaseState Chasing { get; }
}
