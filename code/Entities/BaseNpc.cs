using Sandbox;
using Survivor.HitBox;

namespace Survivor.Entities;

public partial class BaseNpc : AnimatedEntity
{
	public DamageInfo LastDamage { get; private set; }

	public override void TakeDamage( DamageInfo info )
	{
		LastDamage = info;
		info.Damage *= GetHitboxGroup( info.HitboxIndex ) == (int)HitboxGroup.Head ? 2 : 1;
		this.ProceduralHitReaction( info );

		base.TakeDamage( info );

		//TODO: Change target
	}

	public override void OnKilled()
	{
		base.OnKilled();
		BecomeRagdollOnClient( Velocity, LastDamage.Flags, LastDamage.Position, LastDamage.Force, GetHitboxBone( LastDamage.HitboxIndex ) );
	}

	[Event.Tick.Server]
	public virtual void OnServerUpdate()
	{
	}
}
