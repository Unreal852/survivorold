namespace Survivor.StateMachine;

public class DefaultStateMachine
{
	public BaseState    CurrentState { get; set; }
	public StateFactory StateFactory { get; set; }

	public T GetStateFactory<T>() where T : notnull, StateFactory
	{
		return (T)StateFactory;
	}
}
