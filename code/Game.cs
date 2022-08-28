using System;
using System.Linq;
using Sandbox.Internal;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Sandbox;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
public class MyGame : Game
{
	public MyGame()
	{
		if ( IsServer )
		{
			_ = new SimpleHud();
		}
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var player = new UnrealPlayer();
		client.Pawn = player;

		// Get all of the spawnpoints
		var spawnPoints = All.OfType<SpawnPoint>();

		// chose a random one
		var randomSpawnPoint = spawnPoints.MinBy( x => Guid.NewGuid() );

		// if it exists, place the pawn there
		if ( randomSpawnPoint == null )
			return;

		var tx = randomSpawnPoint.Transform;
		tx.Position += Vector3.Up * 50.0f; // raise it up
		player.Transform = tx;
	}

	public override void DoPlayerDevCam( Client client )
	{
		EntityComponentAccessor components = client.Components;
		DevCamera devCamera = components.Get<DevCamera>( true );
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
		if ( player.Pawn is not Player pawn )
			return;
		if ( pawn.DevController is NoclipController )
		{
			Log.Info( "Noclip Mode Off" );
			pawn.DevController = null;
		}
		else
		{
			Log.Info( "Noclip Mode On" );
			pawn.DevController = new NoclipController();
		}
	}
}
