using Sandbox;
using Survivor.Utils;

// ReSharper disable PartialTypeWithSinglePart

namespace Survivor.Entities.Zombies;

public sealed partial class TinyPuncherZombie : PuncherZombie
{
	public TinyPuncherZombie()
	{
		// Ignored
	}

	protected override void Prepare()
	{
		base.Prepare();
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 30, 2 ) );
		EyePosition = Position + Vector3.Up * 64;
		EnableHitboxes = true;
		UsePhysicsCollision = true;
		MoveSpeed = InchesUtils.FromMeters( 9 );
		AttackRange = InchesUtils.FromMeters( 1f );
		AttackDamages = 5;
		Health = 20;
		Scale = 0.5f;
	}
}
