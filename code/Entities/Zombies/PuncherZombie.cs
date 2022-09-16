using Sandbox;
using Survivor.GameResources;
using Survivor.Utils;

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
		base.Attack( ref animHelper, entity );
	}
}
