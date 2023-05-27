using System;
using Sandbox;
using Sandbox.UI;
using Survivor.Extensions;
using Survivor.Players.Controllers;
using Survivor.Players.Inventory;
using Survivor.Weapons;
using SWB_Base;
using SWB_Player;
using PlayerNoclipController = SWB_Player.PlayerNoclipController;

namespace Survivor.Players;

public sealed partial class SurvivorPlayer : PlayerBase
{
	private readonly ClothingContainer _clothing = new();
	private readonly WorldInput _worldInput = new();
	private TimeSince _sinceUseInteraction;
	private TimeSince _sinceLastDamage;
	private TimeSince _sinceLastSprint;
	private TimeSince _sinceDowned;

	public SurvivorPlayer()
	{
		base.Inventory = new SurvivorPlayerInventory( this );
	}

	public SurvivorPlayer( IClient client ) : this()
	{
		_clothing.LoadFromClient( client );
	}

	public bool SuppressPickupNotices { get; set; } = true;
	public bool GodMode { get; set; }
	public float HealthRegenSpeed { get; set; } = 5.0f;
	public float HealthRegenDelay { get; set; } = 2.0f;
	public float StaminaConsumeSpeed { get; set; } = 20.0f;
	public float StaminaRegenSpeed { get; set; } = 15.0f;
	public float StaminaRegenDelay { get; set; } = 2.0f;
	public TimeSince SinceRespawn { get; set; } = 0;

	[Net]
	public float MaxHealth { get; set; }

	[Net]
	public float MaxStamina { get; set; }

	[Net]
	public float Stamina { get; set; }

	[Net]
	public bool IsDowned { get; set; }

	[Net, Change( nameof( OnMoneyChanged ) )]
	public int Money { get; set; }

	public new SurvivorPlayerInventory Inventory => (SurvivorPlayerInventory)base.Inventory;

	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		_clothing.DressEntity( this );

		if ( DevController is PlayerNoclipController )
			DevController = null;

		Controller = new SurvivorPlayerWalkController();
		Animator = new PlayerBaseAnimator();
		CameraMode = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		Health = MaxHealth = 100;
		Stamina = MaxStamina = 100;

		SetDowned( false );

		ClearAmmo();

		if ( SurvivorGame.GAME_MODE?.State is GameState.Playing or GameState.Dev )
			base.Inventory.Add( new WeaponFN57(), true );

		SinceRespawn = 0;
	}

	public bool TryUse()
	{
		if ( !(_sinceUseInteraction > 0.5) )
			return false;

		_sinceUseInteraction = 0;
		return true;
	}

	private void TickPlayerInput()
	{
		if ( Input.Pressed( "Slot1" ) )
			Inventory.SetActiveSlot( 0 );
		else if ( Input.Pressed( "Slot2" ) )
			Inventory.SetActiveSlot( 1 );
		else if ( Input.Pressed( "Slot3" ) )
			Inventory.SetActiveSlot( 2 );

		switch ( Input.MouseWheel )
		{
			case > 0:
				Inventory.SetActiveSlot( Inventory.GetActiveSlot() + 1 );
				break;
			case < 0:
				Inventory.SetActiveSlot( Inventory.GetActiveSlot() - 1 );
				break;
		}

		if ( Input.Pressed( "View" ) )
		{
			if ( CameraMode is ThirdPersonCamera )
				CameraMode = new FirstPersonCamera();
			else
				CameraMode = new ThirdPersonCamera();
		}
	}

	private void SetDowned( bool downed )
	{
		IsDowned = downed;
		if ( downed )
		{
			_sinceDowned = 0;
			SetAnimParameter( "sit", 2 );
			SetAnimParameter( "sit_pose", 8.0f );
		}
		else
		{
			SetAnimParameter( "sit", 0 );
			SetAnimParameter( "sit_pose", 0 );
		}
	}

	private void OnMoneyChanged( int oldMoney, int newMoney )
	{
		Using = null; // This is to force the glow to update
	}

	public override void Respawn()
	{
		base.Respawn();
		Prepare();
	}

	public override void BuildInput()
	{
		_worldInput.Ray = new Ray( EyePosition, EyeRotation.Forward );
		_worldInput.MouseLeftPressed = Input.Down( "use" );
		if ( _worldInput.MouseLeftPressed )
		{
			if ( _worldInput.Hovered != null )
				Log.Info( $"Hovered: {_worldInput.Hovered}" );
			if ( _worldInput.Active != null )
				Log.Info( $"Active: {_worldInput.Active}" );
		}

		if ( Stamina <= 0 )
			Input.Clear( "run" );

		base.BuildInput();
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		if ( LifeState != LifeState.Alive )
			return;

		// Input requested a weapon switch
		if ( ActiveChildInput != null )
			ActiveChild = ActiveChildInput;

		TickPlayerUse();
		TickPlayerUseClient();
		TickPlayerInput();

		// Health regen
		if ( Health < MaxHealth && _sinceLastDamage >= HealthRegenDelay )
		{
			Health = Math.Clamp( Health + HealthRegenSpeed * Time.Delta, 0, MaxHealth );
		}

		// Stamina
		if ( Controller is SurvivorPlayerWalkController { IsSprinting: true } )
		{
			_sinceLastSprint = 0;
			Stamina = Math.Clamp( Stamina - StaminaConsumeSpeed * Time.Delta, 0, MaxStamina );
		}
		else if ( Stamina < MaxStamina && _sinceLastSprint >= StaminaRegenDelay )
		{
			Stamina = Math.Clamp( Stamina + StaminaRegenSpeed * Time.Delta, 0, MaxStamina );
		}

		SimulateActiveChild( cl, ActiveChild );
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( GodMode || SinceRespawn < 1.5 || info.HasTag( "physical" ) )
			return;
		base.TakeDamage( info );
		_sinceLastDamage = 0;
		this.ProceduralHitReaction( info );
		PlaySound( "player.hit_01" );
	}

	public override void OnKilled()
	{
		if ( !IsDowned )
		{
			SetDowned( true );
			return;
		}

		if ( IsDowned && _sinceDowned < 10 )
			return;

		base.OnKilled();

		Inventory.DropActive()?.Delete();
		Inventory.DeleteContents();

		BecomeRagdollOnClient( Velocity, LastDamage.Position, LastDamage.Force,
				LastDamage.BoneIndex, LastDamage.HasTag( "bullet" ), LastDamage.HasTag( "physicsimpact" ),
				LastDamage.HasTag( "blast" ) );

		Controller = null;
		CameraMode = new DeathCamera();

		EnableAllCollisions = false;
		EnableDrawing = false;
	}

	[ClientRpc]
	private void BecomeRagdollOnClient( Vector3 velocity, Vector3 forcePos, Vector3 force,
										int bone, bool bullet, bool physicsImpact, bool blast )
	{
		this.BecomeRagdoll( velocity, forcePos, force, bone, bullet, physicsImpact, blast );
	}
}
