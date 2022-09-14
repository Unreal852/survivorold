using Sandbox;
using Survivor.Players;
using Survivor.Utils;

// ReSharper disable PartialTypeWithSinglePart

namespace Survivor.Entities.Zombies;

public sealed partial class ShooterZombie : BaseZombie
{
	private ModelEntity _weaponEntity;

	public ShooterZombie()
	{
		// Ignored
	}

	public override ZombieType ZombieType => ZombieType.Shooter;

	protected override void Prepare()
	{
		base.Prepare();
		_weaponEntity = new ModelEntity( "models/weapons/pistols/magnum/wm_magnum.vmdl" );
		_weaponEntity.SetParent( this, true );
	}

	public override void OnKilled()
	{
		base.OnKilled();
		_weaponEntity?.Delete();
		_weaponEntity = null;
	}

	protected override void Attack( ref CitizenAnimationHelper animHelper )
	{
		base.Attack( ref animHelper );
		animHelper.HoldType = CitizenAnimationHelper.HoldTypes.Pistol;
		SetAnimParameter( "b_attack", true );
		Sound.FromEntity( "sounds/weapons/ak47/ak_47_shot_01.sound", this );
		// TODO: Make the zombie aim realisticaly
		// var forward = _navSteer.Target;
		// forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * 0.08f * 0.25f;
		// forward = forward.Normal;
		// var endPos = EyePosition + forward * 999999;
		var endPos = NavSteer.TargetEntity.EyePosition +
		             ((Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * 0.1f * 0.25f).Normal;
		var tr = Trace.Ray( EyePosition, endPos )
		              .UseHitboxes()
		              .Ignore( this )
		              .Ignore( _weaponEntity )
		              .Size( 3 )
		              .Run();
		DebugOverlay.Line( EyePosition, tr.HitPosition, 10f );
		if ( !tr.Hit || tr.Entity is not SurvivorPlayer player )
			return;
		player.TakeDamage( DamageInfo.FromBullet( tr.HitPosition, 3, AttackDamages )
		                             .UsingTraceResult( tr )
		                             .WithAttacker( this, _weaponEntity )
		                             .WithHitBody( tr.Body ) );
	}
}
