using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sandbox;
using Survivor.Navigation;
using Survivor.Players;
using Survivor.Utils;
using Survivor.Weapons;

namespace Survivor.Entities.Zombies;

// TODO: Whole movement system isn't that performant at all

public partial class PuncherZombie : BaseZombie
{
	public PuncherZombie()
	{
		// Ignored
	}

	protected override void Prepare()
	{
		base.Prepare();
		MoveSpeed = InchesUtils.FromMeters( 5 );
		AttackRange = InchesUtils.FromMeters( 1.5f );
		AttackDamages = 10;
	}

	protected override void Attack( ref CitizenAnimationHelper animHelper )
	{
		animHelper.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
		SetAnimParameter( "b_attack", true );
		NavSteer.TargetEntity.TakeDamage( DamageInfo.Generic( AttackDamages ) );
		base.Attack( ref animHelper );
	}
}
