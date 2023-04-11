using Sandbox;
using Sandbox.Diagnostics;
using Survivor.UI.Hud;

// ReSharper disable All

namespace Survivor;

/// <inheritdoc />
[Library( "survivor", Title = "Survivor" )]
public partial class SurvivorGame : GameManager
{
	public new static SurvivorGame Current { get; private set; }

	public SurvivorGame()
	{
		Current = this;
		if ( Game.IsServer )
		{
			Game.TickRate = 30;
			_ = new PlayerHudEntity();
		}
	}

	public override void ClientJoined( IClient client )
	{
		GameMode?.OnClientJoin( client );
		base.ClientJoined( client );
	}

	public override void ClientDisconnect( IClient cl, NetworkDisconnectionReason reason )
	{
		GameMode?.OnClientDisconnected( cl, reason );
		base.ClientDisconnect( cl, reason );
	}

	public override void MoveToSpawnpoint( Entity pawn )
	{
		if ( GameMode != null )
			GameMode.MovePlayerToSpawnPoint( pawn );
		else
			base.MoveToSpawnpoint( pawn );
	}

	public override void DoPlayerDevCam( IClient client )
	{
		Assert.NotNull( GameMode );
		GameMode?.OnDoPlayerDevCam( client );
	}
}
