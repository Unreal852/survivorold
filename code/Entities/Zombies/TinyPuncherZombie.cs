using Sandbox;
using Survivor.Utils;

// ReSharper disable PartialTypeWithSinglePart

namespace Survivor.Entities.Zombies;

// TODO: Whole movement system isn't that performant at all

public sealed partial class TinyPuncherZombie : PuncherZombie
{
	public TinyPuncherZombie()
	{
		// Ignored
	}

	protected override void Prepare()
	{
		base.Prepare();
		MoveSpeed = InchesUtils.FromMeters( 9 );
		AttackRange = InchesUtils.FromMeters( 1f );
		AttackDamages = 5;
		Health = 20;
		Scale = 0.4f;
	}
}
