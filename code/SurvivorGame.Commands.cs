using System;
using System.Linq;
using Sandbox;
using Survivor.Entities;
using Survivor.Entities.Hammer;
using Survivor.Tools;
using ServerCommand = Sandbox.ConCmd.ServerAttribute;

namespace Survivor;

public partial class SurvivorGame
{
	[ServerCommand( Name = "spawnm", Help = "Spawn a model of the given type" )]
	public static void SpawnModel( string modelName, int amount = 1 )
	{
		if ( !modelName.Contains( '/' ) )
			modelName = $"models/{modelName}";
		if ( !modelName.EndsWith( ".vmdl" ) )
			modelName += ".vmdl";

		long callerId = ConsoleSystem.Caller.Id;
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller == null )
		{
			Log.Warning( $"{nameof(SpawnModel)} command caller is null" );
			return;
		}

		for ( int i = 0; i < amount; i++ )
		{
			var trace = Trace.Ray( caller.EyePosition, caller.EyePosition + caller.EyeRotation.Forward * 500 )
			                 .UseHitboxes()
			                 .Ignore( caller )
			                 .Size( 2 )
			                 .Run();
			var prop = new Prop { Position = trace.EndPosition * Vector3.Up * 10.0f };
			prop.SetModel( modelName );
			CleanupManager.AddEntity( callerId, prop );
			if ( prop.PhysicsBody == null || prop.PhysicsGroup.BodyCount != 1 )
				continue;

			var point = prop.PhysicsBody.FindClosestPoint( trace.EndPosition );
			var delta = point - trace.EndPosition;
			prop.PhysicsBody.Position -= delta;
		}

		Log.Info( $"Spawned {amount} {modelName}" );
	}

	[ServerCommand( "cleanall" )]
	public static void CleanUp()
	{
		CleanupManager.CleanAll();
		//Map.Reset(DefaultCleanupFilter);
	}

	[ServerCommand( "spawnz" )]
	public static void SpawnZombiesCommand( int amount = 1 )
	{
		if ( SpawnZombies( amount ) )
			Log.Info( "Zombies Spawned !" );
	}

	[ServerCommand( "setzombiestpos" )]
	public static void SetZombiesTargetPosition()
	{
		long callerId = ConsoleSystem.Caller.Id;
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller == null )
		{
			Log.Warning( $"{nameof(SpawnModel)} command caller is null" );
			return;
		}

		var trace = Trace.Ray( caller.EyePosition, caller.EyePosition + caller.EyeRotation.Forward * 500 )
		                 .UseHitboxes()
		                 .Ignore( caller )
		                 .Size( 2 )
		                 .Run();
		var zombies = All.OfType<Zombie>();
		foreach ( Zombie zombie in zombies )
			zombie.NavSteer.TargetPosition = trace.EndPosition;
	}

	[ServerCommand( Name = "tphere", Help = "Teleport the specified player at the current camera position" )]
	public static void TeleportPlayerToCurrentCameraPosition( string playerName = null )
	{
		var devCamera = ConsoleSystem.Caller.Components.Get<DevCamera>();
		if ( devCamera == null )
		{
			Log.Warning( "DEV CAM is null" );
			return;
		}

		if ( string.IsNullOrWhiteSpace( playerName ) )
		{
			foreach ( Client client in Client.All )
				client.Pawn.Position = devCamera.Entity.Position;
			return;
		}

		Client clientToTeleport = Client.All.FirstOrDefault( cl => cl.Name.Equals( playerName, StringComparison.OrdinalIgnoreCase ) );
		if ( clientToTeleport == null )
		{
			Log.Warning( $"No client found with the name '{playerName}'" );
			return;
		}

		clientToTeleport.Pawn.Position = devCamera.Entity.Position;
	}

	[ServerCommand( Name = "stool" )]
	public static void SpawnTool()
	{
		long callerId = ConsoleSystem.Caller.Id;
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller == null )
		{
			Log.Warning( $"{nameof(SpawnModel)} command caller is null" );
			return;
		}

		var trace = Trace.Ray( caller.EyePosition, caller.EyePosition + caller.EyeRotation.Forward * 500 )
		                 .UseHitboxes()
		                 .Ignore( caller )
		                 .Size( 2 )
		                 .Run();
		var tool = _ = new PhysTool() { Position = trace.EndPosition };
		CleanupManager.AddEntity( callerId, tool );
		if ( tool.PhysicsBody == null || tool.PhysicsGroup.BodyCount != 1 )
			return;

		var point = tool.PhysicsBody.FindClosestPoint( trace.EndPosition );
		var delta = point - trace.EndPosition;
		tool.PhysicsBody.Position -= delta;
	}
}
