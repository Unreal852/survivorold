using Sandbox;

namespace Survivor.Entities;

public class Zombie : AnimatedEntity
{
	private float _speed;

	public Zombie()
	{
		// Ignored
	}

	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );
		EyePosition = Position + Vector3.Up * 64;
		//CollisionGroup = CollisionGroup.Player;
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );

		EnableHitboxes = true;

		SetMaterialGroup( Rand.Int( 0, 3 ) );

		_ = new ModelEntity( "models/citizen_clothes/trousers/trousers.smart.vmdl", this );
		_ = new ModelEntity( "models/citizen_clothes/jacket/labcoat.vmdl", this );
		_ = new ModelEntity( "models/citizen_clothes/shirt/shirt_longsleeve.scientist.vmdl", this );

		if ( Rand.Int( 3 ) == 1 )
			_ = new ModelEntity( "models/citizen_clothes/hair/hair_femalebun.black.vmdl", this );
		else if ( Rand.Int( 10 ) == 1 )
			_ = new ModelEntity( "models/citizen_clothes/hat/hat_hardhat.vmdl", this );

		SetBodyGroup( 1, 0 );

		_speed = Rand.Float( 100, 300 );
	}

	public override void Spawn()
	{
		base.Spawn();
		Prepare();
	}
}
