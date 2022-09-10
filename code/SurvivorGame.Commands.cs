using System;
using System.Linq;
using Sandbox;
using Survivor.Entities;
using Survivor.Entities.Zombies;
using Survivor.Players;
using ServerCommand = Sandbox.ConCmd.ServerAttribute;

namespace Survivor;

public partial class SurvivorGame
{
	[ServerCommand( "sessioninfos" )]
	public static void SessionInfosCommand()
	{
		Log.Info( "SESSION INFOS ----" );
		Log.Info( $"\tGameMode: {Current.GameMode.Name} ({Current.GameMode.GetType()})" );
		Log.Info( $"\tDifficulty: {Current.GameMode.Difficulty}" );
	}

	[ServerCommand( "mapreset" )]
	public static void CleanUpCommand()
	{
		Map.Reset( DefaultCleanupFilter );
	}

	[ServerCommand( "givemoney" )]
	public static void GiveMoney( int amount = 100 )
	{
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller is SurvivorPlayer player )
			player.Money += amount;
	}

	[ServerCommand( "spawnz" )]
	public static void SpawnZombiesCommand( int amount = 1 )
	{
		if ( Current is { } game && game.SpawnZombies( amount ) )
			Log.Info( "Zombies Spawned !" );
	}

	[ServerCommand( "killnpc" )]
	public static void ClearZombiesCommand()
	{
		foreach ( BaseNpc npc in All.OfType<BaseNpc>().ToArray() )
			npc.Delete();
		GAME_MODE.EnemiesRemaining = 0;
	}

	[ServerCommand( "setnpctpos" )]
	public static void SetZombiesTargetPosition()
	{
		long callerId = ConsoleSystem.Caller.Id;
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller == null )
		{
			Log.Warning( $"{nameof(SetZombiesTargetPosition)} command caller is null" );
			return;
		}

		var trace = Trace.Ray( caller.EyePosition, caller.EyePosition + caller.EyeRotation.Forward * 500 )
		                 .UseHitboxes()
		                 .Ignore( caller )
		                 .Size( 2 )
		                 .Run();
		var zombies = All.OfType<BaseZombie>();
		foreach ( BaseZombie zombie in zombies )
			zombie.SetTarget( trace.EndPosition, true );
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
}
