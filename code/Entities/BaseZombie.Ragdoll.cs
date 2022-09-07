using System.Linq;
using Sandbox;

namespace Survivor.Entities;

public partial class BaseZombie
{
	private static readonly EntityLimit Limit = new() { MaxTotal = 20 };

	[ClientRpc]
	private void BecomeRagdollOnClient( Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force, int bone )
	{
		var ent = new ModelEntity
		{
				Position = Position,
				Rotation = Rotation,
				Scale = Scale,
				UsePhysicsCollision = true,
				EnableAllCollisions = true
		};
		ent.Tags.Add( "ragdoll", "solid", "debris" );
		ent.SetModel( GetModelName() );
		ent.CopyBonesFrom( this );
		ent.CopyBodyGroups( this );
		ent.CopyMaterialGroup( this );
		ent.CopyMaterialOverrides( this );
		ent.TakeDecalsFrom( this );
		ent.EnableAllCollisions = true;
		ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
		ent.RenderColor = RenderColor;
		ent.PhysicsGroup.Velocity = velocity;
		ent.PhysicsEnabled = true;

		foreach ( var child in Children )
		{
			if ( child is not ModelEntity modelEntity )
				continue;
			// TODO: Use Tags : Need to add tags to clothes when spawning them on zombies
			var model = modelEntity.GetModelName();
			if ( !model.Contains( "clothes" ) )
				continue;

			var clothing = new ModelEntity();
			clothing.SetModel( model );
			clothing.SetParent( ent, true );
			clothing.RenderColor = modelEntity.RenderColor;
			clothing.CopyBodyGroups( modelEntity );
			clothing.CopyMaterialGroup( modelEntity );
		}

		if ( damageFlags.HasFlag( DamageFlags.Bullet ) ||
		     damageFlags.HasFlag( DamageFlags.PhysicsImpact ) )
		{
			PhysicsBody body = bone > 0 ? ent.GetBonePhysicsBody( bone ) : null;
			if ( body != null )
				body.ApplyImpulseAt( forcePos, force * body.Mass );
			else
				ent.PhysicsGroup.ApplyImpulse( force );
		}

		if ( damageFlags.HasFlag( DamageFlags.Blast ) )
		{
			if ( ent.PhysicsGroup != null )
			{
				ent.PhysicsGroup.AddVelocity( (Position - (forcePos + Vector3.Down * 100.0f)).Normal * (force.Length * 0.2f) );
				var angularDir = (Rotation.FromYaw( 90 )                                                             * force.WithZ( 0 ).Normal).Normal;
				ent.PhysicsGroup.AddAngularVelocity( angularDir * (force.Length                                      * 0.02f) );
			}
		}

		Limit.Watch( ent );
		//ent.DeleteAsync( 10.0f );
	}
}
