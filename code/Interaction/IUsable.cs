using Sandbox;

namespace Survivor.Interaction;

public interface IUsable : IUse
{
	// TODO: Custom color for prefix, suffix, message 
	
	public virtual string UsePrefix  => "Press";
	public virtual string UseSuffix  => string.Empty;
	public virtual string UseMessage => string.Empty;
	public         int    UseCost    { get; }
	public virtual bool   HasCost    => UseCost > 0;
}
