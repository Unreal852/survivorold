namespace Survivor.StateMachine;

public class StateFactory
{
	public StateFactory( DefaultStateMachine defaultStateMachine )
	{
		DefaultStateMachine = defaultStateMachine;
	}

	public DefaultStateMachine DefaultStateMachine { get; }
}
