using Sandbox;

// ReSharper disable PartialTypeWithSinglePart

namespace Survivor.Entities.Zombies;

public sealed partial class ShooterZombie : BaseZombie
{
	private ModelEntity _weaponEntity;
	private Trace       _trace;

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
		_trace = Trace.Ray( 0, 0 ).Ignore( this ).Ignore( _weaponEntity ).UseHitboxes().WithoutTags( "zombie", "debris" );
	}

	public override void OnKilled()
	{
		base.OnKilled();
		_weaponEntity?.Delete();
		_weaponEntity = null;
	}

	protected override bool CanAttack( Entity entity )
	{
		if ( !base.CanAttack( entity ) )
			return false;
		var tr = _trace.FromTo( EyePosition, entity.EyePosition ).Run();
		return tr.Hit && tr.Entity == entity;
	}

	protected override void Attack( ref CitizenAnimationHelper animHelper, Entity entity )
	{
		SinceLastAttack = 0;
		animHelper.HoldType = CitizenAnimationHelper.HoldTypes.Pistol;
		SetAnimParameter( "b_attack", true );
		Sound.FromEntity( "sounds/weapons/ak47/ak_47_shot_01.sound", this );
		// TODO: Make the zombie aim realisticaly
		// var forward = _navSteer.Target;
		// forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * 0.08f * 0.25f;
		// forward = forward.Normal;
		// var endPos = EyePosition + forward * 999999;
		if ( Rand.Float() >= 0.3f ) // Fake miss
			return;
		var endPos = entity.EyePosition + Vector3.Down * 5;
		var tr = _trace.FromTo( EyePosition, endPos ).Run();
		if ( !tr.Hit )
			return;
		tr.Entity.TakeDamage( DamageInfo.FromBullet( tr.HitPosition, 3, AttackDamages )
		                                .UsingTraceResult( tr )
		                                .WithAttacker( this, _weaponEntity )
		                                .WithHitBody( tr.Body ) );
	}
}
