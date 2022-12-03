using Sandbox;
using Survivor.Extensions;
using Survivor.Players;
using SWB_Base;

namespace Survivor.Weapons.Bullets;

public class TraceBullet : BulletBase
{
	public override void FireSV( WeaponBase weapon, Vector3 startPos, Vector3 endPos, Vector3 forward, float spread, float force, float damage, float bulletSize,
	                             bool isPrimary )
	{
		Fire( weapon, startPos, endPos, forward, spread, force, damage, bulletSize, isPrimary );
	}

	public override void FireCL( WeaponBase weapon, Vector3 startPos, Vector3 endPos, Vector3 forward, float spread, float force, float damage, float bulletSize,
	                             bool isPrimary )
	{
		Fire( weapon, startPos, endPos, forward, spread, force, damage, bulletSize, isPrimary );
	}
	
	private void Fire( WeaponBase weapon, Vector3 startPos, Vector3 endPos, Vector3 forward, float spread, float force, float damage, float bulletSize,
	                   bool isPrimary, int refireCount = 0 )
	{
		var tr = weapon.TraceBulletEx( startPos, endPos, bulletSize );
		if ( !tr.Hit )
			return;
		var isValidEnt = tr.Entity.IsValid;
		if ( !isValidEnt )
			return;
		var canPenetrate = SurfaceUtil.CanPenetrate( tr.Surface );
		if ( Host.IsClient )
		{ 
			// Impact
			tr.Surface.DoBulletImpact( tr );

			var tracerParticle = isPrimary ? weapon.Primary.BulletTracerParticle : weapon.Secondary.BulletTracerParticle;

			// Tracer
			if ( !string.IsNullOrEmpty( tracerParticle ) )
			{
				if ( Rand.Int( 1 ) == 0 )
					TracerEffects( weapon, tracerParticle, tr.EndPosition );
			}
		}

		if ( Host.IsServer )
		{
			if ( tr.Entity is SurvivorPlayer && !SurvivorGame.VarFriendlyFire )
				return;
			using ( Prediction.Off() )
			{
				// Damage
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 25 * force, damage )
				                           .UsingTraceResult( tr )
				                           .WithAttacker( weapon.Owner )
				                           .WithWeapon( weapon );

				tr.Entity.TakeDamage( damageInfo );
			}
		}

		// Re-run the trace if we can penetrate
		if ( canPenetrate )
		{
			if ( refireCount > 10 ) return;

			Fire( weapon, tr.HitPosition + tr.Direction * 10, endPos, forward, spread, force, damage, bulletSize, isPrimary, ++refireCount );
		}
	}

	private void TracerEffects( WeaponBase weapon, string tracerParticle, Vector3 endPos )
	{
		var firingViewModel = weapon.GetEffectModel();

		if ( firingViewModel == null ) return;

		var effectData = weapon.GetMuzzleEffectData( firingViewModel );
		var effectEntity = effectData.Item1;
		var muzzleAttach = effectEntity.GetAttachment( effectData.Item2 );
		var tracer = Particles.Create( tracerParticle );
		tracer.SetPosition( 1, muzzleAttach.GetValueOrDefault().Position );
		tracer.SetPosition( 2, endPos );
	}
}
