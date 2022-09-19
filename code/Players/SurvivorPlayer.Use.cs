using Sandbox;

namespace Survivor.Players;

public sealed partial class SurvivorPlayer
{
	private void TickPlayerUseClient()
	{
		if ( !Host.IsClient )
			return;
		Using = FindUsable();
	}
}
