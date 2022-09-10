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
		Using = FindUsableWithTraceResult( out var tr );
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

	protected virtual Entity FindUsableWithTraceResult( out TraceResult traceResult )
	{
		// First try a direct 0 width line
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 85 )
		              .Ignore( this )
		              .Run();
		traceResult = tr;

		// See if any of the parent entities are usable if we ain't.
		var ent = tr.Entity;
		while ( ent.IsValid() && !IsValidUseEntity( ent ) )
		{
			ent = ent.Parent;
		}

		// Nothing found, try a wider search
		if ( !IsValidUseEntity( ent ) )
		{
			tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 85 )
			          .Radius( 2 )
			          .Ignore( this )
			          .Run();

			// See if any of the parent entities are usable if we ain't.
			ent = tr.Entity;
			while ( ent.IsValid() && !IsValidUseEntity( ent ) )
			{
				ent = ent.Parent;
			}
		}

		// Still no good? Bail.
		if ( !IsValidUseEntity( ent ) ) return null;

		return ent;
	}
}
