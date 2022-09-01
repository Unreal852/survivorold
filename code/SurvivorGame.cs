using Sandbox;
using Sandbox.Internal;
using Survivor.Players;
using Survivor.Players.Controllers;
using Survivor.UI.Hud;

namespace Survivor;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
[Library( "survivor", Title = "Survivor" )]
public partial class SurvivorGame : Game
{
	public SurvivorGame()
	{
		if ( IsServer )
		{
			_ = new MainPlayerHud();
		}
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var player = new SurvivorPlayer( client );
		player.Respawn();
		client.Pawn = player;
	}

	public override void DoPlayerDevCam( Client client )
	{
		EntityComponentAccessor components = client.Components;
		var devCamera = components.Get<DevCamera>( true );
		if ( devCamera == null )
		{
			var component = new DevCamera();
			components = client.Components;
			components.Add( component );
		}
		else
			devCamera.Enabled = !devCamera.Enabled;
	}

	public override void DoPlayerNoclip( Client player )
	{
		if ( player.Pawn is not SurvivorPlayer pawn )
			return;
		if ( pawn.DevController is PlayerNoclipController )
		{
			Log.Info( "Noclip Mode Off" );
			pawn.DevController = null;
		}
		else
		{
			Log.Info( "Noclip Mode On" );
			pawn.DevController = new PlayerNoclipController();
		}
	}
}
