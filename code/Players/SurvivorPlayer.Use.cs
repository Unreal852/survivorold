using Sandbox;

namespace Survivor.Players;

public sealed partial class SurvivorPlayer
{
	// TODO: This is badly written, its only here for testing

	private void TickPlayerUseClient()
	{
		if ( !Host.IsClient )
			return;
		Using = FindUsable();
	}
}
