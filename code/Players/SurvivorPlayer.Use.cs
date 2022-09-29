using Sandbox;
using Survivor.Interaction;

namespace Survivor.Players;

public sealed partial class SurvivorPlayer
{
	private void TickPlayerUseClient()
	{
		if ( !Host.IsClient )
			return;
		var wasUsing = Using;
		Using = FindUsable();
		if ( wasUsing != null && wasUsing != Using )
			OnStopUsingClient( wasUsing );
		if ( Using != null && wasUsing != Using )
			OnStartUsingClient( Using );
	}

	private void OnStartUsingClient( Entity entity )
	{
		if(entity is IGlow glow)
			glow.SetGlow(true);
	}

	private void OnStopUsingClient( Entity entity )
	{
		if(entity is IGlow glow)
			glow.SetGlow(false);
	}
}
