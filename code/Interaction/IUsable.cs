using Sandbox;

namespace Survivor.Interaction;

public interface IUsable : IUse
{
	public string UseMessage { get; }
}
