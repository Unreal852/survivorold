using Sandbox;
using Survivor.Gamemodes;
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
		if ( IsServer )
		{
			Global.TickRate = 30;
			_ = new PlayerHudEntity();
		}
	}

	public override void ClientJoined( Client client )
	{
		GameMode?.OnClientJoin( client );
		base.ClientJoined( client );
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
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

	public override void DoPlayerDevCam( Client client )
	{
		Assert.NotNull( GameMode );
		GameMode?.OnDoPlayerDevCam( client );
	}

	public override void DoPlayerNoclip( Client client )
	{
		Assert.NotNull( GameMode );
		GameMode?.OnDoPlayerNoclip( client );
	}

	public new static bool DefaultCleanupFilter( string className, Entity ent )
	{
		if ( ent is BaseGameMode )
			return false;
		return GameManager.DefaultCleanupFilter( className, ent );
	}
}
