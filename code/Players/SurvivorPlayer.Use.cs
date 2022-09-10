using Sandbox;
using Survivor.UI.World;

namespace Survivor.Players;

public partial class SurvivorPlayer
{
	// TODO: This is badly written, its only here for testing

	private InteractableIndicator _interactableIndicator;

	public void TickPlayerUseClient()
	{
		if ( !Host.IsClient )
			return;
		Using = FindUsable( );
		if ( Using != null && _interactableIndicator == null )
		{
			_interactableIndicator = new InteractableIndicator( this );
		}
		else if ( Using == null && _interactableIndicator != null )
		{
			_interactableIndicator?.Delete();
			_interactableIndicator = null;
		}
	}
}
