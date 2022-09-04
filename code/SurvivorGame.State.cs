using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Survivor.Entities;
using Survivor.Entities.Hammer;
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

		GameMode = new SurvivorGameMode();

		// GameMode = VarGameMode.Name switch
		// {
		// 		"survivor" => new SurvivorGameMode(),
		// 		_          => new SurvivorGameMode()
		// };
		//
		// GameMode.Difficulty = VarDifficulty.Name switch
		// {
		// 		"gm_difficulty_easy"      => Difficulty.Easy,
		// 		"gm_difficulty_normal"    => Difficulty.Normal,
		// 		"gm_difficulty_hard"      => Difficulty.Hard,
		// 		"gm_difficulty_legendary" => Difficulty.Legendary,
		// 		_                         => Difficulty.Normal
		// };
	}

	public static bool SpawnZombies( int amount = 1 )
	{
		var spawns = ZombieSpawns ??= All.OfType<ZombieSpawn>().Where( zs => zs.IsEnabled ).ToArray();
		if ( spawns.Count <= 0 )
		{
			Log.Warning( "No zombie spawn found." );
			return false;
		}

		for ( int i = 0; i < amount; i++ )
		{
			ZombieSpawn zombieSpawn = spawns[Rand.Int( 0, spawns.Count - 1 )];
			_ = new Zombie { Position = zombieSpawn.Position + Vector3.Up * 5 };
		}

		return true;
	}
}
