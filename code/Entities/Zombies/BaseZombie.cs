using Sandbox;
using Survivor.Assets;
using Survivor.Navigation;
using Survivor.Performance;
using Survivor.Players;
using Survivor.Utils;
using Survivor.Weapons;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable MemberCanBePrivate.Global

namespace Survivor.Entities.Zombies;

// TODO: Huge performance hit scaling with the number of zombies

public abstract partial class BaseZombie : BaseNpc
{
	[ConVar.Replicated( "zombie_debug" )]
	public static bool ZombiesDrawDebug { get; set; } = false;

	private readonly   string[]  _moveTags = { "solid", "passbullets", "player" };
	protected readonly NavSteer  NavSteer  = new();
	protected readonly BBox      BBox      = BBox.FromHeightAndRadius( 64, 4 );
	protected          Vector3   InputVelocity;
	protected          Vector3   LookDirection;
	protected          TimeSince SinceLastAttack;
	protected          TimeSince SinceLastMoan;
	protected          TimeSince SinceSurroundingCheck;
	protected          float     NextMoanIn;
	protected readonly float     SurroundingCheckRate = 0.5f;

	public BaseZombie()
	{
		// Ignored
	}

	public          string     FriendlyName  { get; set; } = "Zombie";
	public          float      MoveSpeed     { get; set; } = 1f;
	public          float      AttackSpeed   { get; set; } = 1f;
	public          float      AttackDamages { get; set; } = 1f;
	public          float      AttackForce   { get; set; } = 1f;
	public          float      AttackRange   { get; set; } = 1f;
	public          float      EyeHeight     { get; set; } = 1.6f;
	public abstract ZombieType ZombieType    { get; }
	public override Ray        AimRay        => new(EyePosition, LookDirection);

	public Entity Target
	{
		get => NavSteer.TargetEntity;
		set
		{
			if ( value is not { IsValid: true } )
				return;
			NavSteer.TargetEntity = value;
		}
	}

	protected virtual void Prepare()
	{
		var data = ZombieAsset.GetResource( ZombieType );
		if ( data == null )
		{
			Log.Error( $"Missing data for {GetType().Name}" );
			Delete();
			return;
		}

		data.Apply( this );

		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );
		EyePosition = Position + Vector3.Up * 64; // TODO: Mode this in zombie asset
		EnableHitboxes = true;
		UsePhysicsCollision = true;

		NextMoanIn = Rand.Float( 1.5f, 10f );

		FindTarget();
	}

	private void FindTarget()
	{
		if ( IsClient )
			return;
		var clients = Client.All;
		var client = clients[Rand.Int( 0, clients.Count - 1 )];
		if ( client.Pawn is SurvivorPlayer player )
			NavSteer.TargetEntity = player;
	}

	public void SetTarget( Vector3 position, bool force = false )
	{
		Host.AssertServer();
		NavSteer.TargetPosition = position;
		if ( force )
			NavSteer.TargetEntity = null;
	}

	public override void Spawn()
	{
		base.Spawn();
		Prepare();
	}

	public override void OnKilled()
	{
		base.OnKilled();
		if ( !IsServer )
			return;
		if ( LastAttacker is SurvivorPlayer player )
		{
			player.Money += Rand.Int( 5, 10 );
			player.Client.AddInt( "kills" );
		}
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );

		if ( info.Attacker is not SurvivorPlayer attacker )
			return;

		// Note - sending this only to the attacker!
		attacker.DidDamage( To.Single( attacker ), info.Position, info.Damage, Health,
				Health.LerpInverse( 100, 0 ) );

		if ( info.Weapon is ABaseWeapon weapon && weapon.UISettings.ShowHitmarker && !weapon.UISettings.HideAll )
		{
			attacker.ShowHitmarker( To.Single( attacker ), Health <= 0.0f, weapon.UISettings.PlayHitmarkerSound );
		}

		// TODO: Target change
	}

	public override void OnServerUpdate()
	{
		using var serverUpdateProfiling = Profiler.Scope( $"{nameof(BaseZombie)}::{nameof(OnServerUpdate)}" );

		EyePosition = Position + Vector3.Up * InchesUtils.FromMeters( EyeHeight );
		InputVelocity = 0;
		if ( NavSteer != null )
		{
			using var navSteerTickProfiling = Profiler.Scope( $"{nameof(NavSteer)}::{nameof(NavSteer.Tick)}" );
			{
				NavSteer.Tick( Position );
			}

			if ( !NavSteer.Output.Finished )
			{
				InputVelocity = NavSteer.Output.Direction.Normal;
				Velocity = Velocity.AddClamped( InputVelocity * Time.Delta * 500, MoveSpeed );
			}
		}

		using var moveProfiling = Profiler.Scope( $"{nameof(BaseZombie)}::{nameof(Move)}" );
		{
			Move( Time.Delta );
		}

		var walkVelocity = Velocity.WithZ( 0 );
		if ( walkVelocity.Length > 0.5f )
		{
			var turnSpeed = walkVelocity.Length.LerpInverse( 0, 100 );
			var targetRotation = Rotation.LookAt( walkVelocity.Normal, Vector3.Up );
			Rotation = Rotation.Lerp( Rotation, targetRotation, turnSpeed * Time.Delta * 20.0f );
		}

		var animHelper = new CitizenAnimationHelper( this );

		//LookDirection = Vector3.Lerp( LookDirection, Target?.EyePosition ?? InputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );
		LookDirection = Vector3.Lerp( LookDirection, Target?.AimRay.Position ?? InputVelocity.WithZ( 0 ) * 1000,
				Time.Delta * 100.0f );
		animHelper.WithLookAt( LookDirection );
		animHelper.WithVelocity( Velocity );
		animHelper.WithWishVelocity( InputVelocity );

		using var checkSurroundingsProfiling = Profiler.Scope( $"{nameof(BaseZombie)}::{nameof(CheckSurroundings)}" );
		{
			CheckSurroundings( ref animHelper );
		}

		if ( CanAttack( NavSteer?.TargetEntity ) )
		{
			Attack( ref animHelper, NavSteer!.TargetEntity );
		}

		if ( SinceLastMoan >= NextMoanIn )
		{
			Sound.FromEntity( "zombie_moan", this );
			SinceLastMoan = 0;
			NextMoanIn = Rand.Float( 1.5f, 10f );
		}

		if ( ZombiesDrawDebug )
			DebugDraw();
	}

	protected virtual bool CanAttack( Entity entity )
	{
		if ( SinceLastAttack < AttackSpeed )
			return false;
		return entity is { IsValid: true, Health: > 0.0f }
		    && entity.Position.DistanceSquared( Position ) <= (AttackRange * AttackRange);
	}

	protected virtual void Attack( ref CitizenAnimationHelper animHelper, Entity entity )
	{
		SinceLastAttack = 0;
	}

	protected virtual void CheckSurroundings( ref CitizenAnimationHelper animationHelper )
	{
		if ( SinceSurroundingCheck < SurroundingCheckRate )
			return;
		foreach ( var entity in FindInSphere( Position + Vector3.Up * 39, 39.0f ) )
		{
			switch ( entity )
			{
				case DoorEntity doorEntity:
					doorEntity.Open( this );
					break;
				case Prop prop when CanAttack( prop ):
					Attack( ref animationHelper,
							prop ); // TODO: Maybe set the prop as a temporary target, because right now, the CheckSurroundings rate jam the real attack rate
					break;
			}
		}

		SinceSurroundingCheck = 0;
	}

	protected virtual void Move( float timeDelta )
	{
		MoveHelper move = new(Position, Velocity, _moveTags) { MaxStandableAngle = 50 };
		move.Trace = move.Trace.Ignore( this ).Size( BBox );

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

			if ( InputVelocity.Length > 0 )
			{
				var movement = move.Velocity.Dot( InputVelocity.Normal );
				move.Velocity -= movement * InputVelocity.Normal;
				move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
				move.Velocity += movement * InputVelocity.Normal;
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

	protected virtual void DebugDraw()
	{
		NavSteer?.DebugDrawPath();
		DebugOverlay.Line( EyePosition, LookDirection, Target != null ? Color.Red : Color.Yellow );
		DebugOverlay.Box( PhysicsBody, Color.Yellow );
	}
}
