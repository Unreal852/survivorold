using Sandbox;
using Survivor.Extensions;

namespace Survivor.Entities;

public partial class BaseNpc : AnimatedEntity
{
	public DamageInfo LastDamage  { get; private set; }
	public Vector3    EyePosition { get; set; }

	public override void TakeDamage( DamageInfo info )
	{
		LastDamage = info;
		info.Damage *= info.Hitbox.HasTag( "head" ) ? 2 : 1;
		this.ProceduralHitReaction( info );
		base.TakeDamage( info );
	}

	public override void OnKilled()
	{
		base.OnKilled();
		SurvivorGame.GAME_MODE.OnEnemyKilled( this, LastAttacker );
		BecomeRagdollOnClient( Velocity, LastDamage.Flags, LastDamage.Position, LastDamage.Force,
				LastDamage.BoneIndex );
	}

	[Event.Tick.Server]
	public virtual void OnServerUpdate()
	{
	}

	[ClientRpc]
	private void BecomeRagdollOnClient( Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force,
	                                    int bone )
	{
		this.BecomeRagdoll( velocity, damageFlags, forcePos, force, bone );
	}
}
