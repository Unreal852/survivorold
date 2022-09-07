using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox;
using Survivor.Entities;
using Survivor.Entities.Hammer;
using Survivor.GameMode.GameModes;
using Survivor.Gamemodes;
using ServerCommand = Sandbox.ConCmd.ServerAttribute;

namespace Survivor;

public partial class SurvivorGame
{
	private static IReadOnlyList<ZombieSpawn> ZombieSpawns { get; set; }
	[Net] public   BaseGameMode               GameMode     { get; set; }

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

	public bool SpawnZombies( int amount = 1 )
	{
		var spawns = ZombieSpawns ??= All.OfType<ZombieSpawn>().Where( zs => zs.IsEnabled ).ToArray();
		spawns = spawns.Where( zs => zs.CanSpawn).ToArray();
		if ( spawns.Count <= 0 )
		{
			Log.Warning( "No zombie spawn found." );
			return false;
		}

		for ( int i = 0; i < amount; i++ )
		{
			ZombieSpawn zombieSpawn = spawns[Rand.Int( 0, spawns.Count - 1 )];
			_ = new BaseZombie { Position = zombieSpawn.Position + Vector3.Up * 5 };
		}

		GameMode.EnemiesRemaining += amount;
		return true;
	}
}
