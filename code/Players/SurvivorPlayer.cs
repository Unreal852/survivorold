using System;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Survivor.Extensions;
using Survivor.Players.Controllers;
using Survivor.Players.Inventory;
using Survivor.Weapons;
using SWB_Base;
using PlayerNoclipController = SWB_Base.PlayerNoclipController;

namespace Survivor.Players;

public sealed partial class SurvivorPlayer : PlayerBase
{
	private readonly ClothingContainer _clothing   = new();
	private readonly WorldInput        _worldInput = new();
	private          TimeSince         _sinceUseInteraction;
	private          TimeSince         _sinceLastDamage;
	private          TimeSince         _sinceLastSprint;

	public SurvivorPlayer()
	{
		base.Inventory = new SurvivorPlayerInventory( this );
	}

	public SurvivorPlayer( Client client ) : this()
	{
		_clothing.LoadFromClient( client );
	}

	public bool      SuppressPickupNotices { get; set; } = true;
	public bool      GodMode               { get; set; } = false;
	public float     HealthRegenSpeed      { get; set; } = 5.0f;
	public float     HealthRegenDelay      { get; set; } = 2.0f;
	public float     StaminaConsumeSpeed   { get; set; } = 20.0f;
	public float     StaminaRegenSpeed     { get; set; } = 15.0f;
	public float     StaminaRegenDelay     { get; set; } = 2.0f;
	public TimeSince SinceRespawn          { get; set; } = 0;

	[Net]
	public float MaxHealth { get; set; }

	[Net]
	public float MaxStamina { get; set; }

	[Net]
	public float Stamina { get; set; }

	[Net, Change( nameof(OnMoneyChanged) )]
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
		CameraMode = new FirstPersonCamera( this );

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		Health = MaxHealth = 100;
		Stamina = MaxStamina = 100;

		
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

	private void SwitchToBestWeapon()
	{
		var best = Children.Select( x => x as WeaponBase ).FirstOrDefault( x => x.IsValid() );
		if ( best == null )
			return;

		ActiveChild = best;
	}

	private void TickPlayerInput()
	{
		if ( Input.Pressed( InputButton.Slot1 ) )
			Inventory.SetActiveSlot( 0 );
		else if ( Input.Pressed( InputButton.Slot2 ) )
			Inventory.SetActiveSlot( 1 );
		else if ( Input.Pressed( InputButton.Slot3 ) )
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

		if ( Input.Pressed( InputButton.View ) )
		{
			if ( CameraMode is ThirdPersonCamera )
				CameraMode = new FirstPersonCamera( this );
			else
				CameraMode = new ThirdPersonCamera( this );
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
		_worldInput.MouseLeftPressed = Input.Down( InputButton.Use );
		if ( _worldInput.MouseLeftPressed )
		{
			if ( _worldInput.Hovered != null )
				Log.Info( $"Hovered: {_worldInput.Hovered}" );
			if ( _worldInput.Active != null )
				Log.Info( $"Active: {_worldInput.Active}" );
		}

		if ( Stamina <= 0 )
			Input.ClearButton( InputButton.Run );

		base.BuildInput();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( LifeState != LifeState.Alive )
			return;

		// Input requested a weapon switch
		if ( ActiveChildInput != null )
			ActiveChild = ActiveChildInput;

		if ( LifeState != LifeState.Alive )
			return;

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

		//
		// If the current weapon is out of ammo and we last fired it over half a second ago
		// lets try to switch to a better wepaon
		//
		if ( ActiveChild is WeaponBase weapon && !weapon.IsUsable()
		                                      && weapon.TimeSincePrimaryAttack   > 0.5f
		                                      && weapon.TimeSinceSecondaryAttack > 0.5f )
			SwitchToBestWeapon();
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( GodMode || SinceRespawn < 1.5 || info.Flags == DamageFlags.PhysicsImpact )
			return;
		base.TakeDamage( info );
		this.ProceduralHitReaction( info );
		PlaySound( "player.hit_01" );
		_sinceLastDamage = 0;
	}

	public override void OnKilled()
	{
		base.OnKilled();

		Inventory.DropActive()?.Delete();
		Inventory.DeleteContents();

		BecomeRagdollOnClient( Velocity, LastDamage.Flags, LastDamage.Position, LastDamage.Force,
				LastDamage.BoneIndex );

		Controller = null;
		CameraMode = new SpectateCamera( this );

		EnableAllCollisions = false;
		EnableDrawing = false;
	}

	[ClientRpc]
	private void BecomeRagdollOnClient( Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force,
	                                    int bone )
	{
		this.BecomeRagdoll( velocity, damageFlags, forcePos, force, bone );
	}
}
