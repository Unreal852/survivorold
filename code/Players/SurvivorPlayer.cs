using System.Linq;
using Sandbox;
using Sandbox.UI;
using Survivor.Players.Controllers;
using Survivor.Players.Inventory;
using Survivor.UI.World;
using Survivor.Weapons;
using SWB_Base;

namespace Survivor.Players;

public sealed partial class SurvivorPlayer : PlayerBase
{
	private readonly ClothingContainer _clothing = new();
	private          TimeSince         _timeSinceDropped;
	private          WorldInput        _worldInput = new();

	public SurvivorPlayer()
	{
		Inventory = new SurvivorPlayerInventory( this );
	}

	public SurvivorPlayer( Client client ) : this()
	{
		_clothing.LoadFromClient( client );
	}

	public       bool      SuppressPickupNotices { get; set; } = true;
	public       bool      GodMode               { get; set; } = false;
	public       TimeSince SinceRespawn          { get; set; } = 0;
	[Net] public int       Money                 { get; set; }

	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		_clothing.DressEntity( this );

		if ( DevController is PlayerNoclipController )
			DevController = null;

		Controller = new PlayerWalkController();
		Animator = new PlayerBaseAnimator();
		CameraMode = new FirstPersonCamera();


		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		Health = 100;

		ClearAmmo();

		SuppressPickupNotices = true;

		Inventory.Add( new Magnum(), true );
		Inventory.Add( new AK47() );

		GiveAmmo( AmmoType.Pistol, 1000 );

		SuppressPickupNotices = false;

		SinceRespawn = 0;

		base.Respawn();
	}

	public void SwitchToBestWeapon()
	{
		// var best = Children
		//           .Select( x => x as WeaponBase )
		//           .Where( x => x.IsValid() && x.IsUsable() )
		//           .MaxBy( x => x.BucketWeight );

		var best = Children.Select( x => x as WeaponBase ).FirstOrDefault( x => x.IsValid() );
		if ( best == null )
			return;

		ActiveChild = best;
	}

	public override void Respawn()
	{
		base.Respawn();
		Prepare();
	}

	public override void BuildInput( InputBuilder input )
	{
		_worldInput.Ray = new Ray( EyePosition, EyeRotation.Forward );
		_worldInput.MouseLeftPressed = input.Down( InputButton.Use);
		if ( _worldInput.MouseLeftPressed )
		{
			Log.Info("Pressed");
			if(_worldInput.Hovered != null)
				Log.Info($"Hovered: {_worldInput.Hovered}");
			if(_worldInput.Active != null)
				Log.Info($"Active: {_worldInput.Active}");	
		}

		base.BuildInput( input );
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		// Input requested a weapon switch
		if ( Input.ActiveChild != null )
		{
			ActiveChild = Input.ActiveChild;
		}

		if ( LifeState != LifeState.Alive )
			return;

		TickPlayerUse();
		TickPlayerUseClient();

		if ( Input.Pressed( InputButton.View ) )
		{
			if ( CameraMode is ThirdPersonCamera )
				CameraMode = new FirstPersonCamera();
			else
				CameraMode = new ThirdPersonCamera();
		}

		if ( Input.Pressed( InputButton.Drop ) )
		{
			var dropped = Inventory.DropActive();
			if ( dropped != null )
			{
				if ( dropped.PhysicsGroup != null )
					dropped.PhysicsGroup.Velocity = Velocity + (EyeRotation.Forward + EyeRotation.Up) * 300;

				_timeSinceDropped = 0;
				SwitchToBestWeapon();
			}
		}

		if ( Input.Pressed( InputButton.Slot1 ) )
			Inventory.SetActiveSlot( 0, true );
		else if ( Input.Pressed( InputButton.Slot2 ) )
			Inventory.SetActiveSlot( 1, true );

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

	public override void StartTouch( Entity other )
	{
		if ( _timeSinceDropped < 1 )
			return;

		base.StartTouch( other );
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( GodMode || SinceRespawn < 1.5 )
			return;
		base.TakeDamage( info );
		this.ProceduralHitReaction( info );
		PlaySound( "sounds/player/player_hit_01.sound" );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		Inventory.DropActive();
		Inventory.DeleteContents();

		BecomeRagdollOnClient( Velocity, LastDamage.Flags, LastDamage.Position, LastDamage.Force, GetHitboxBone( LastDamage.HitboxIndex ) );

		Controller = null;
		CameraMode = new SpectateRagdollCamera();

		EnableAllCollisions = false;
		EnableDrawing = false;
	}
}
