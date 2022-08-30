using System.Linq;
using Sandbox;
using Survivor.Players.Inventory;
using Survivor.Tools;
using SWB_Base;

namespace Survivor.Players;

public partial class SurvivorPlayer : PlayerBase
{
	private readonly ClothingContainer _clothing = new();
	private          TimeSince         _timeSinceDropped;

	public SurvivorPlayer()
	{
		Inventory = new SurvivorPlayerInventory( this );
	}

	public SurvivorPlayer( Client client ) : this()
	{
		_clothing.LoadFromClient( client );
	}

	public bool SuppressPickupNotices { get; set; } = true;

	private void Prepare()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		_clothing.DressEntity( this );

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

		// TODO: Add weapons to inventory
		Inventory.Add( new PhysTool(), true );
		ActiveChild = Inventory.Active;

		SuppressPickupNotices = false;

		SwitchToBestWeapon();
	}

	public void SwitchToBestWeapon()
	{
		var best = Children
		          .Select( x => x as WeaponBase )
		          .Where( x => x.IsValid() && x.IsUsable() )
		          .MaxBy( x => x.BucketWeight );

		if ( best == null )
		{
			return;
		}

		ActiveChild = best;
	}

	public override void Respawn()
	{
		Prepare();
		base.Respawn();
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

	public override void OnKilled()
	{
		base.OnKilled();

		Inventory.DropActive();
		Inventory.DeleteContents();

		BecomeRagdollOnClient( LastDamage.Force, GetHitboxBone( LastDamage.HitboxIndex ) );

		Controller = null;
		CameraMode = new SpectateRagdollCamera();

		EnableAllCollisions = false;
		EnableDrawing = false;
	}
}
