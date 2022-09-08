using Sandbox;
using Survivor.Gamemodes;
using Survivor.UI.Hud;

// ReSharper disable All

namespace Survivor;

/// <inheritdoc />
[Library( "survivor", Title = "Survivor" )]
public partial class SurvivorGame : Game
{
	public new static SurvivorGame Current => (SurvivorGame)Game.Current;

	public SurvivorGame()
	{
		if ( IsServer )
		{
			_ = new PlayerHud();
			Global.TickRate = 30;
		}
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );
		Assert.NotNull( GameMode );

		GameMode?.OnClientJoin( this, client );
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
		Assert.NotNull( GameMode );
		GameMode?.OnClientDisconnected( this, cl, reason );
	}

	public override void DoPlayerDevCam( Client client )
	{
		Assert.NotNull( GameMode );
		GameMode?.OnDoPlayerDevCam( this, client );
	}

	public override void DoPlayerNoclip( Client client )
	{
		Assert.NotNull( GameMode );
		GameMode?.OnDoPlayerNoclip( this, client );
	}

	public new static bool DefaultCleanupFilter( string className, Entity ent )
	{
		if ( ent is BaseGameMode )
			return false;
		return Game.DefaultCleanupFilter( className, ent );
	}
}
