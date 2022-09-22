using Sandbox;
using Survivor.Players;
using SWB_Base;

// ReSharper disable PartialTypeWithSinglePart

namespace Survivor.Entities.Zombies;

public partial class PuncherZombie : BaseZombie
{
	public PuncherZombie()
	{
		// Ignored
	}

	public override ZombieType ZombieType { get; } = ZombieType.Puncher;

	protected override void Prepare()
	{
		base.Prepare();
	}

	protected override void Attack( ref CitizenAnimationHelper animHelper, Entity entity )
	{
		animHelper.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
		SetAnimParameter( "b_attack", true );
		entity.TakeDamage( DamageInfo.Generic( AttackDamages ).WithAttacker( this ).WithForce( AttackForce ) );
		// if ( entity is SurvivorPlayer player )
		// {
		// 	var force = player.Position - Position;
		// 	PushObject( entity, force.WithZ(3) * 5, Time.Delta );
		// }

		SinceLastAttack = 0;
	}

	protected void PushObject( Entity entity, Angles angle, float time )
	{
		var force = Rotation.From( angle ).Forward * 5000 * time;

		var isPhysics = false;
		if ( entity.PhysicsGroup is { BodyCount: > 0 } )
		{
			foreach ( var body in entity.PhysicsGroup.Bodies )
			{
				if ( body.BodyType != PhysicsBodyType.Dynamic )
					continue;
				isPhysics = true;
				break;
			}
		}

		if ( isPhysics )
		{
			foreach ( var body in entity.PhysicsGroup.Bodies )
			{
				body.ApplyImpulse( force * body.Mass );
			}

			Log.Info( "physic" );
		}
		else
		{
			// Players...

			if ( force.z > 1 && entity.GroundEntity != null )
			{
				entity.GroundEntity = null;
			}

			entity.Velocity += force;
		}
	}
}
