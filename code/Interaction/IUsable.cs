using Sandbox;

namespace Survivor.Interaction;

public interface IUsable : IUse
{
	public         string UseMessage { get; }
	public         int    Cost       { get; }
	public virtual bool   HasCost    => Cost > 0;
}
