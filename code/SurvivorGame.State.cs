using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Sandbox;
using Survivor.Entities.Hammer;
using Survivor.Entities.Zombies;
using Survivor.GameMode;
using Survivor.GameMode.GameModes;
using Survivor.Gamemodes;
using ServerCommand = Sandbox.ConCmd.ServerAttribute;

namespace Survivor;

public partial class SurvivorGame
{
	public static BaseGameMode GAME_MODE => Current.GameMode;

	public ReadOnlyCollection<ZombieSpawnPoint>      ZombieSpawnPoints       { get; private set; }
	public ReadOnlyCollection<PlayerLobbySpawnPoint> PlayerLobbySpawnsPoints { get; private set; }
	public ReadOnlyCollection<SpawnPoint>            PlayerSpawnPoints       { get; private set; }

	[Net]
	public BaseGameMode GameMode { get; private set; }

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		GameMode = VarGameMode switch
		{
				"survivor" => new SurvivorGameMode(),
				_          => new SurvivorGameMode()
		};

		GameMode.Difficulty = VarDifficulty switch
		{
				"gm_difficulty_easy"      => Difficulty.Easy,
				"gm_difficulty_normal"    => Difficulty.Normal,
				"gm_difficulty_hard"      => Difficulty.Hard,
				"gm_difficulty_legendary" => Difficulty.Legendary,
				_                         => Difficulty.Normal
		};

		LoadSpawnPoints();

		GameMode ??= new SurvivorGameMode();

		SessionInfosCommand();
	}

	private void LoadSpawnPoints()
	{
		var sw = Stopwatch.StartNew();
		var zombieSpawnPoints = new Collection<ZombieSpawnPoint>();
		var playerLobbySpawnPoints = new Collection<PlayerLobbySpawnPoint>();
		var playerSpawnPoints = new Collection<SpawnPoint>();

		foreach ( var entity in All )
		{
			switch ( entity )
			{
				case ZombieSpawnPoint zombieSpawnPoint:
					zombieSpawnPoints.Add( zombieSpawnPoint );
					continue;
				case PlayerLobbySpawnPoint playerLobbySpawnPoint:
					playerLobbySpawnPoints.Add( playerLobbySpawnPoint );
					continue;
				case SpawnPoint spawnPoint:
					playerSpawnPoints.Add( spawnPoint );
					continue;
				default:
					continue;
			}
		}

		ZombieSpawnPoints = new ReadOnlyCollection<ZombieSpawnPoint>( zombieSpawnPoints );
		PlayerLobbySpawnsPoints = new ReadOnlyCollection<PlayerLobbySpawnPoint>( playerLobbySpawnPoints );
		PlayerSpawnPoints = new ReadOnlyCollection<SpawnPoint>( playerSpawnPoints );
		sw.Stop();

		if ( ZombieSpawnPoints.Count == 0 )
			Log.Error( "Map missing zombies spawn points" );
		if ( PlayerLobbySpawnsPoints.Count == 0 )
			Log.Error( "Map missing player lobby spawn points" );
		if ( PlayerSpawnPoints.Count == 0 )
			Log.Error( "Map missing player spawn points" );

		Log.Info( $"Map spawn points loaded in {sw.Elapsed.TotalMilliseconds:F}" );
	}

	public int SpawnZombies<TZombie>( int amount = 1 ) where TZombie : BaseZombie, new()
	{
		var spawns = ZombieSpawnPoints.Where( zs => zs.CanSpawn ).ToArray();
		if ( spawns.Length <= 0 )
		{
			Log.Warning( "No zombie spawn found." );
			return 0;
		}

		for ( var i = 0; i < amount; i++ )
		{
			var zombieSpawn = spawns[Rand.Int( 0, spawns.Length - 1 )];
			_ = new TZombie { Position = zombieSpawn.Position + Vector3.Up * 5, Rotation = zombieSpawn.Rotation };
		}

		GameMode.EnemiesRemaining += amount;
		return amount;
	}

	public int SpawnZombies( int amount = 1 )
	{
		var spawns = ZombieSpawnPoints.Where( zs => zs.CanSpawn ).ToArray();
		if ( spawns.Length <= 0 )
		{
			Log.Error( "This map is missing zombies spawns." );
			return 0;
		}

		for ( int i = 0; i < amount; i++ )
		{
			var zombieSpawn = spawns[Rand.Int( 0, spawns.Length - 1 )];
			if ( Rand.Float() <= 0.2 )
			{
				if ( Rand.Float() <= 0.3 )
					_ = new ShooterZombie { Position = zombieSpawn.Position + Vector3.Up * 5, Rotation = zombieSpawn.Rotation };
				else
					_ = new TinyPuncherZombie { Position = zombieSpawn.Position + Vector3.Up * 5, Rotation = zombieSpawn.Rotation };
			}
			else
				_ = new PuncherZombie { Position = zombieSpawn.Position + Vector3.Up * 5, Rotation = zombieSpawn.Rotation };
		}

		GameMode.EnemiesRemaining += amount;
		return amount;
	}
}
