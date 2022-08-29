using System.Linq;
using Sandbox;
using Survivor.Entities;
using ServerCommand = Sandbox.ConCmd.ServerAttribute;

namespace Survivor;

public partial class SurvivorGame
{
	[ServerCommand( "spawnm" )]
	public static void SpawnModel( string modelName, int amount = 1 )
	{
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
			var prop = new Prop { Position = trace.EndPosition };
			prop.SetModel( modelName );
			CleanupManager.AddEntity( callerId, prop );
			if ( prop.PhysicsBody == null || prop.PhysicsGroup.BodyCount != 1 )
				continue;

			var point = prop.PhysicsBody.FindClosestPoint( trace.EndPosition );
			var delta = point - trace.EndPosition;
			prop.PhysicsBody.Position -= delta;
		}
	}

	[ServerCommand( "cleanall" )]
	public static void CleanUp()
	{
		CleanupManager.CleanAll();
		//Map.Reset(DefaultCleanupFilter);
	}

	[ServerCommand( "spawnz" )]
	public static void SpawnZombies( int amount )
	{
		var spawns = All.OfType<ZombieSpawn>().ToArray();
		if ( spawns.Length <= 0 )
		{
			Log.Warning( "No zombie spawn found." );
			return;
		}

		for ( int i = 0; i < amount; i++ )
		{
			ZombieSpawn zombieSpawn = spawns[Rand.Int( 0, spawns.Length - 1 )];
			Model model = zombieSpawn.ModelToSpawn;
			if ( model == null )
			{
				Log.Error( "The specified model is null" );
				continue;
			}

			var prop = new Prop() { Position = zombieSpawn.Position + Vector3.Up * 2 };
			prop.SetModel( model.Name );
		}
	}
}
