using System.Linq;
using Sandbox;
using Sandbox.Players;
using Sandbox.UI;
using Sandbox.UI.World;

namespace Sandbox.UI.World;

public class PlayerNameWorldPanel : WorldPanel
{
	private readonly SurvivorPlayer _player;

	public PlayerNameWorldPanel( SurvivorPlayer player )
	{
		StyleSheet.Load( "/UI/World/PlayerNameWorldPanel.scss" );
		_player = player;
		var hoverPlayerNameLabel = new HoverPlayerNameLabel( Client.All.FirstOrDefault( c => c.Pawn == player )?.Name ?? Local.DisplayName );
		AddChild( hoverPlayerNameLabel );
		PanelBounds = hoverPlayerNameLabel.Box.Rect;
	}

	public override void Tick()
	{
		base.Tick();
		Vector3 pawnEyePos = _player.EyePosition;
		float z = pawnEyePos.z + 13;
		Transform = new Transform( new Vector3( pawnEyePos.x, pawnEyePos.y - PanelBounds.Center.y / 2, z ), _player.Rotation );
	}
}
