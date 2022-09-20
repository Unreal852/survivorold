using Sandbox;

namespace Survivor.Interaction;

public interface IUsable : IUse
{
	public virtual string UseMessage => string.Empty;
	public         int    UseCost    { get; }
	public virtual bool   HasCost    => UseCost > 0;
}
