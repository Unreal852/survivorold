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

	protected override void Attack( ref CitizenAnimationHelper animHelper )
	{
		animHelper.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
		SetAnimParameter( "b_attack", true );
		NavSteer.TargetEntity.TakeDamage( DamageInfo.Generic( AttackDamages ) );
		base.Attack( ref animHelper );
	}
}
