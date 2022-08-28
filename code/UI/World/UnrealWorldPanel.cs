using System;
using System.Linq;
using Sandbox.UI.Construct;

namespace Sandbox.UI.World;

public class UnrealWorldPanel : WorldPanel
{
	private readonly HoverPlayerNameLabel _hoverPlayerNameLabel;
	private readonly UnrealPlayer         _player;

	public UnrealWorldPanel( UnrealPlayer player )
	{
		StyleSheet.Load( "/UI/World/UnrealWorldPanel.scss" );
		_player = player;
		_hoverPlayerNameLabel = new HoverPlayerNameLabel( Client.All.FirstOrDefault( c => c.Pawn == player )?.Name ?? Local.DisplayName );
		AddChild( _hoverPlayerNameLabel );
		PanelBounds = _hoverPlayerNameLabel.Box.Rect;
	}

	public override void Tick()
	{
		base.Tick();
		Vector3 pawnEyePos = _player.EyePosition;
		float z = pawnEyePos.z + 13;
		Transform = new Transform( new Vector3( pawnEyePos.x, pawnEyePos.y - PanelBounds.Center.y / 2, z ), _player.Rotation );
	}
}
