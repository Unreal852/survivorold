using Sandbox;

namespace Survivor.Interaction;

public interface IUsable : IUse
{
	public virtual string UseMessage => string.Empty;
	public         int    Cost       { get; }
	public virtual bool   HasCost    => Cost > 0;
}
