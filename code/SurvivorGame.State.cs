using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Survivor.Assets;
using Survivor.Entities.Hammer;
using Survivor.Entities.Zombies;
using Survivor.GameMode;
using Survivor.GameMode.GameModes;
using Survivor.Gamemodes;
using Survivor.Weapons;
using ServerCommand = Sandbox.ConCmd.ServerAttribute;

namespace Survivor;

public partial class SurvivorGame
{
	private static IReadOnlyList<ZombieSpawn> ZombieSpawns { get; set; }
	public static  BaseGameMode               GAME_MODE    => Current.GameMode;

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

		GameMode ??= new SurvivorGameMode();

		SessionInfosCommand();
	}

	public IReadOnlyList<ZombieSpawn> GetAvailableZombiesSpawns()
	{
		var spawns = ZombieSpawns ??= All.OfType<ZombieSpawn>().Where( zs => zs.IsEnabled ).ToArray();
		return spawns.Where( zs => zs.CanSpawn ).ToArray();
	}

	public bool SpawnZombies<TZombie>( int amount = 1 ) where TZombie : BaseZombie, new()
	{
		var spawns = GetAvailableZombiesSpawns();
		if ( spawns.Count <= 0 )
		{
			Log.Warning( "No zombie spawn found." );
			return false;
		}

		for ( var i = 0; i < amount; i++ )
		{
			var zombieSpawn = spawns[Rand.Int( 0, spawns.Count - 1 )];
			_ = new TZombie { Position = zombieSpawn.Position + Vector3.Up * 5, Rotation = zombieSpawn.Rotation };
		}

		GameMode.EnemiesRemaining += amount;
		return true;
	}

	public bool SpawnZombies( int amount = 1 )
	{
		var spawns = GetAvailableZombiesSpawns();
		if ( spawns.Count <= 0 )
		{
			Log.Warning( "No zombie spawn found." );
			return false;
		}

		for ( int i = 0; i < amount; i++ )
		{
			var zombieSpawn = spawns[Rand.Int( 0, spawns.Count - 1 )];
			switch ( Rand.Int( 2 ) )
			{
				case 0:
					_ = new TinyPuncherZombie { Position = zombieSpawn.Position + Vector3.Up * 5, Rotation = zombieSpawn.Rotation };
					break;
				case 1:
					_ = new PuncherZombie { Position = zombieSpawn.Position + Vector3.Up * 5, Rotation = zombieSpawn.Rotation };
					break;
				case 2:
					_ = new ShooterZombie { Position = zombieSpawn.Position + Vector3.Up * 5, Rotation = zombieSpawn.Rotation };
					break;
			}
		}

		GameMode.EnemiesRemaining += amount;
		return true;
	}
}
