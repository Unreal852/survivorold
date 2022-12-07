using System.Linq;
using Sandbox;
using Survivor.Entities;
using Survivor.Entities.Zombies;
using Survivor.Players;
using Survivor.UI.World;
using ServerCommand = Sandbox.ConCmd.ServerAttribute;
using ClientCommand = Sandbox.ConCmd.ClientAttribute;
using AdminServerCommand = Sandbox.ConCmd.AdminAttribute;

// ReSharper disable StringLiteralTypo

namespace Survivor;

public partial class SurvivorGame
{
	[ClientCommand( "test" )]
	public static void SpawnWorldInteractablePanel()
	{
		_ = new WorldInteractablePanel() { Position = Local.Pawn.AimRay.Position };
	}

	[ServerCommand( "forcestart" )]
	public static void ForceStartGameCommand()
	{
		GAME_MODE?.SetGameState( GameState.Playing );
	}

	[ServerCommand( "devstart" )]
	public static void ForceDevStartGameCommand()
	{
		GAME_MODE?.SetGameState( GameState.Dev );
	}

	[ServerCommand( "sessioninfos" )]
	public static void SessionInfosCommand()
	{
		Log.Info( "SESSION INFOS ----" );
		Log.Info( $"\tMap: {Map.Name}" );
		Log.Info( $"\tGameMode: {Current.GameMode.GameModeName} ({Current.GameMode.GetType()})" );
		Log.Info( $"\tDifficulty: {Current.GameMode.Difficulty}" );
	}

	[AdminServerCommand( "imgod" )]
	public static void GodModeCommand()
	{
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller is SurvivorPlayer player )
			player.GodMode = !player.GodMode;
	}

	[AdminServerCommand( "mapreset" )]
	public static void CleanUpCommand()
	{
		Map.Reset( DefaultCleanupFilter );
	}

	[AdminServerCommand( "imrich" )]
	public static void GiveMoneyCommand( int amount = 10000 )
	{
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller is SurvivorPlayer player )
			player.Money += amount;
	}

	[AdminServerCommand( "setmoney" )]
	public static void ClearMoneyCommand( int amount )
	{
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller is SurvivorPlayer player )
			player.Money = amount;
	}

	[AdminServerCommand( "impoor" )]
	public static void ClearMoneyCommand()
	{
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller is SurvivorPlayer player )
			player.Money = 0;
	}

	[AdminServerCommand( "spawnz" )]
	public static void SpawnZombiesCommand( int amount = 1, string zombieType = "" )
	{
		if ( Current == null )
			return;
		var success = zombieType switch
		{
				"tiny"    => Current.SpawnZombies<TinyPuncherZombie>( amount ),
				"puncher" => Current.SpawnZombies<PuncherZombie>( amount ),
				"shooter" => Current.SpawnZombies<ShooterZombie>( amount ),
				_         => Current.SpawnZombies( amount ),
		};

		if ( success > 0 )
			Log.Info( "Zombies Spawned !" );
	}

	[AdminServerCommand( "killnpc" )]
	public static void ClearZombiesCommand()
	{
		foreach ( var npc in All.OfType<BaseNpc>().ToArray() )
			npc.OnKilled();
	}

	[AdminServerCommand( "setnpctpos" )]
	public static void SetZombiesTargetPosition()
	{
		long callerId = ConsoleSystem.Caller.Id;
		var caller = ConsoleSystem.Caller?.Pawn;
		if ( caller == null )
		{
			Log.Warning( $"{nameof(SetZombiesTargetPosition)} command caller is null" );
			return;
		}

		var trace = Trace.Ray( caller.AimRay, 500 )
		                 .UseHitboxes()
		                 .Ignore( caller )
		                 .Size( 2 )
		                 .Run();
		var zombies = All.OfType<BaseZombie>();
		foreach ( var zombie in zombies )
			zombie.SetTarget( trace.EndPosition, true );
	}
}
