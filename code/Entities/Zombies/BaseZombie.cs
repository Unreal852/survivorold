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
	[ConVar.Replicated] public static bool      nav_drawpath { get; set; } = false;
	private readonly                  NavSteer  _navSteer = new();
	private readonly                  BBox      _bBox     = BBox.FromHeightAndRadius( 64, 4 );
	private                           Vector3   _inputVelocity;
	private                           Vector3   _lookDirection;
	private                           TimeSince _timeSinceLastAttack;

	public BaseZombie()
	{
		// Ignored
	}

	public float MoveSpeed     { get; set; }
	public float AttackSpeed   { get; set; } = 1f;
	public float AttackDamages { get; set; } = 5;
	public float AttackRange   { get; set; }

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
			_navSteer.TargetEntity = player;
	}

	public void SetTarget( Entity entity )
	{
		if ( entity is not { IsValid: true } )
			return;
		_navSteer.TargetEntity = entity;
	}

	public void SetTarget( Vector3 position, bool force = false )
	{
		_navSteer.TargetPosition = position;
		if ( force )
			_navSteer.TargetEntity = null;
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

		if ( _navSteer != null )
		{
			_navSteer.Tick( Position );

			if ( !_navSteer.Output.Finished )
			{
				_inputVelocity = _navSteer.Output.Direction.Normal;
				Velocity = Velocity.AddClamped( _inputVelocity * Time.Delta * 500, MoveSpeed );
			}

			if ( nav_drawpath )
				_navSteer.DebugDrawPath();
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
		if ( _navSteer?.TargetEntity != null && _navSteer.TargetEntity.IsValid
		                                     && _navSteer.TargetEntity.Health                        > 0.0f
		                                     && _navSteer.TargetEntity.Position.Distance( Position ) < AttackRange
		                                     && _timeSinceLastAttack                                 > AttackSpeed )
		{
			animHelper.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
			SetAnimParameter( "b_attack", true );
			_navSteer.TargetEntity.TakeDamage( DamageInfo.Generic( AttackDamages ) );
			_timeSinceLastAttack = 0;
		}
	}

	protected virtual void Move( float timeDelta )
	{
		MoveHelper move = new(Position, Velocity) { MaxStandableAngle = 50 };
		move.Trace = move.Trace.Ignore( this ).Size( _bBox );

		if ( !Velocity.IsNearlyZero( 0.001f ) )
		{
			move.TryUnstuck();
			if ( GroundEntity != null )
				move.TryMoveWithStep( timeDelta, 30 );
			else
				move.TryMove( timeDelta );
		}

		var tr = move.TraceDirection( Vector3.Down * 10.0f );
		if ( move.IsFloor( tr ) )
		{
			SetAnimParameter( "b_grounded", true );
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
			SetAnimParameter( "b_grounded", false );
		}

		Position = move.Position;
		Velocity = move.Velocity;
	}
}
