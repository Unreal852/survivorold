using Sandbox;

namespace Survivor.Entities;

public partial class Zombie
{
	[ClientRpc]
	private void BecomeRagdollOnClient( Vector3 force, int forceBone )
	{
		var ent = new ModelEntity { Position = Position, Rotation = Rotation, PhysicsEnabled = true, UsePhysicsCollision = true };

		Tags.Add( "debris" );

		ent.SetModel( GetModelName() );
		ent.SetMaterialGroup( GetMaterialGroup() );
		ent.CopyBonesFrom( this );
		ent.TakeDecalsFrom( this );
		ent.SetRagdollVelocityFrom( this );
		ent.DeleteAsync( 20.0f );

		// Copy the clothes over
		foreach ( var child in Children )
		{
			if ( child is ModelEntity e )
			{
				var model = e.GetModelName();
				if ( model != null && !model.Contains( "clothes" ) )
					continue;

				var clothing = new ModelEntity();
				clothing.SetModel( model );
				clothing.SetParent( ent, true );
			}
		}

		ent.PhysicsGroup.AddVelocity( force * 5 );

		if ( forceBone >= 0 )
		{
			var body = ent.GetBonePhysicsBody( forceBone );
			if ( body != null )
				body.ApplyForce( force * 1000 );
			else
				ent.PhysicsGroup.AddVelocity( force );
		}
	}
}
