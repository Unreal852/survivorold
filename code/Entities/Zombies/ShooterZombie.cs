using Sandbox;
using Survivor.Players;
using Survivor.Utils;

namespace Survivor.Entities.Zombies;

// TODO: Whole movement system isn't that performant at all

public partial class ShooterZombie : BaseZombie
{
	private ModelEntity _weaponEntity;

	public ShooterZombie()
	{
		// Ignored
	}

	protected override void Prepare()
	{
		base.Prepare();
		MoveSpeed = InchesUtils.FromMeters( 4 );
		AttackRange = InchesUtils.FromMeters( 25 );
		AttackSpeed = 3;
		AttackDamages = 15f;
		Health = 50;

		_weaponEntity = new ModelEntity( "models/weapons/pistols/glock_18/wm_glock18.vmdl" );
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
		if ( !tr.Hit )
			return;
		if ( tr.Entity is SurvivorPlayer player )
			player.TakeDamage( DamageInfo.FromBullet( tr.HitPosition, 3, AttackDamages )
			                             .UsingTraceResult( tr )
			                             .WithAttacker( this, _weaponEntity )
			                             .WithHitBody( tr.Body ) );
	}
}
