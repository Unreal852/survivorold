using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sandbox;
using Survivor.Navigation;
using Survivor.Players;
using Survivor.Utils;

namespace Survivor.Entities.Zombies;

// TODO: Whole movement system isn't that performant at all

public partial class BaseZombie : BaseNpc
{
	[ConVar.Replicated] public static bool    nav_drawpath { get; set; } = false;
	private                           Vector3 _inputVelocity;
	private                           Vector3 _lookDirection;

	public BaseZombie()
	{
		// Ignored
	}

	public float     MoveSpeed       { get; set; }
	public float     AttackSpeed     { get; set; } = 1f;
	public float     AttackDamages   { get; set; } = 5;
	public float     AttackRange     { get; set; }
	public NavSteer  NavSteer        { get; set; } = new();
	public TimeSince SinceLastAttack { get; set; } = 0;

	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );
		EyePosition = Position + Vector3.Up * 64;
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );
		EnableHitboxes = true;
		UsePhysicsCollision = true;

		Tags.Add( "zombie" );
		RenderColor = Color.Green;
		Health = 100;
		MoveSpeed = InchesUtils.FromMeters( 7 );
		AttackRange = InchesUtils.FromMeters( 1 );

		FindTarget();
	}

	private void FindTarget()
	{
		if ( IsClient )
			return;
		var clients = Client.All;
		Client client = clients[Rand.Int( 0, clients.Count - 1 )];
		if ( client.Pawn is SurvivorPlayer player )
			NavSteer.TargetEntity = player;
	}

	public override void Spawn()
	{
		base.Spawn();
		Prepare();
	}

	public override void OnKilled()
	{
		base.OnKilled();
		if ( IsServer && LastAttacker is SurvivorPlayer player )
		{
			SurvivorGame.Current.GameMode.EnemiesRemaining--;
			player.Money += Rand.Int( 5, 10 );
			player.Client.AddInt( "kills" );
		}
	}

	public override void OnServerUpdate()
	{
		_inputVelocity = 0;

		if ( NavSteer != null )
		{
			NavSteer.Tick( Position );

			if ( !NavSteer.Output.Finished )
			{
				_inputVelocity = NavSteer.Output.Direction.Normal;
				Velocity = Velocity.AddClamped( _inputVelocity * Time.Delta * 500, MoveSpeed );
			}

			if ( nav_drawpath )
				NavSteer.DebugDrawPath();
		}

		Move( Time.Delta );

		var walkVelocity = Velocity.WithZ( 0 );
		if ( walkVelocity.Length > 0.5f )
		{
			var turnSpeed = walkVelocity.Length.LerpInverse( 0, 100 );
			var targetRotation = Rotation.LookAt( walkVelocity.Normal, Vector3.Up );
			Rotation = Rotation.Lerp( Rotation, targetRotation, turnSpeed * Time.Delta * 20.0f );
		}

		var animHelper = new CitizenAnimationHelper( this );

		_lookDirection = Vector3.Lerp( _lookDirection, _inputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );
		animHelper.WithLookAt( EyePosition + _lookDirection );
		animHelper.WithVelocity( Velocity );
		animHelper.WithWishVelocity( _inputVelocity );

		var entities = FindInSphere( Position, 20.0f ).ToArray();
		DoorEntity door = entities.OfType<DoorEntity>().FirstOrDefault();
		door?.Open( this );
		if ( NavSteer?.TargetEntity != null && NavSteer.TargetEntity.IsValid
		                                    && NavSteer.TargetEntity.Health                        > 0.0f
		                                    && NavSteer.TargetEntity.Position.Distance( Position ) < AttackRange
		                                    && SinceLastAttack                                     > AttackSpeed )
		{
			animHelper.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
			SetAnimParameter( "b_attack", true );
			NavSteer.TargetEntity.TakeDamage( DamageInfo.Generic( AttackDamages ) );
			SinceLastAttack = 0;
		}
	}

	protected virtual void Move( float timeDelta )
	{
		var bbox = BBox.FromHeightAndRadius( 64, 4 );
		MoveHelper move = new(Position, Velocity) { MaxStandableAngle = 50 };
		move.Trace = move.Trace.Ignore( this ).Size( bbox );

		if ( !Velocity.IsNearlyZero( 0.001f ) )
		{
			move.TryUnstuck();
			move.TryMoveWithStep( timeDelta, 30 );
		}

		var tr = move.TraceDirection( Vector3.Down * 10.0f );
		if ( move.IsFloor( tr ) )
		{
			GroundEntity = tr.Entity;
			if ( !tr.StartedSolid )
				move.Position = tr.EndPosition;

			if ( _inputVelocity.Length > 0 )
			{
				var movement = move.Velocity.Dot( _inputVelocity.Normal );
				move.Velocity -= movement * _inputVelocity.Normal;
				move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
				move.Velocity += movement * _inputVelocity.Normal;
			}
			else
				move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
		}
		else
		{
			GroundEntity = null;
			move.Velocity += Vector3.Down * 900 * timeDelta;
		}

		Position = move.Position;
		Velocity = move.Velocity;
	}
}
