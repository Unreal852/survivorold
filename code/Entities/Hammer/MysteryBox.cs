using System;
using Sandbox;
using SandboxEditor;
using Survivor.Players;
using Survivor.UI.World;
using Survivor.Utils;
using Survivor.Weapons;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_mystery_box" )]
[Title( "Mystery Box" ), Category( "Map" ), Icon( "place" ), Description( "This entity defines a mystery box" )]
[HammerEntity, SupportsSolid, Model( Model = "models/objects/weapon_box.vmdl", Archetypes = ModelArchetype.animated_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public partial class MysteryBox : AnimatedEntity, IUse
{
	private bool        _isOpening;
	private bool        _isOpened;
	private bool        _isClosing;
	private Entity      _lastUser;
	private ModelEntity _weaponEntity;
	private TimeSince   _sinceOpened;

	public MysteryBox()
	{
	}

	[Property]
	[Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property]
	[Title( "Cost" ), Description( "The price to use this machine" )]
	public int Cost { get; set; } = 0;

	public float StayOpenedDuration { get; set; } = 8f;

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		// TODO: Box panel caching, Maybe network _isOpened for clients to rely on this to show the timer instead of RPCs
	}

	private void SpawnRandomWeapon()
	{
		if ( _weaponEntity != null && _weaponEntity.IsValid )
		{
			_weaponEntity.Delete();
			_weaponEntity = null;
		}

		var wepSpawn = GetAttachment( "weapon" );
		if ( wepSpawn.HasValue )
		{
			if ( Rand.Int( 1 ) == 0 ) // TODO: dummy code
				_weaponEntity = new ModelEntity( "models/weapons/pistols/magnum/wm_magnum.vmdl" ) { Transform = wepSpawn.Value };
			else
				_weaponEntity = new ModelEntity( "models/weapons/assault_rifles/ak47/wm_ak47.vmdl" ) { Transform = wepSpawn.Value, };
		}
		else
			Log.Warning( "Missing 'weapon' attachement on the mystery box" );
	}

	public void OpenBox()
	{
		if ( _isOpening || _isClosing || _isOpened )
			return;
		SpawnRandomWeapon();
		SetAnimParameter( "open", true );
	}

	public void CloseBox()
	{
		if ( _isOpening || _isClosing || !_isOpened )
			return;
		SetAnimParameter( "close", true );
		_lastUser = null;
	}

	public bool OnUse( Entity user )
	{
		if ( !IsEnabled )
			return false;
		if ( user is SurvivorPlayer player && player.Money >= Cost && player.TryUse() )
		{
			if ( !_isOpened )
			{
				_lastUser = player;
				player.Money -= Cost;
				OpenBox();
				return true;
			}

			if ( user == _lastUser && _weaponEntity != null && _weaponEntity.IsValid )
			{
				// TODO: Dummy code
				if ( _weaponEntity.Model.ResourceName.Contains( "ak47" ) )
					player.Inventory.Add( new AK47(), true );
				else
					player.Inventory.Add( new Magnum(), true );
				MysteryBoxTimer.DeleteMysteryBoxTimerClient();
				_weaponEntity.Delete();
				CloseBox();
				return true;
			}

			return false;
		}

		return false;
	}

	public bool IsUsable( Entity user )
	{
		return !_isOpening && !_isClosing;
	}

	protected override void OnAnimGraphTag( string tag, AnimGraphTagEvent fireMode )
	{
		if ( !IsServer )
			return;
		if ( tag == "Opening" )
			_isOpening = fireMode == AnimGraphTagEvent.Start;
		else if ( tag == "Closing" )
			_isClosing = fireMode == AnimGraphTagEvent.Start;
		else if ( tag == "Opened" )
		{
			_sinceOpened = 0;
			_isOpened = fireMode == AnimGraphTagEvent.Start;
			if ( _isOpened )
			{
				var timerSpawn = GetAttachment( "timer" );
				if ( !timerSpawn.HasValue )
					return;
				MysteryBoxTimer.SpawnMysteryBoxTimerClient( timerSpawn.Value.Position, timerSpawn.Value.Rotation, StayOpenedDuration );
			}
		}
	}

	[Event.Tick.Server]
	public void OnTick()
	{
		if ( _weaponEntity != null && _weaponEntity.IsValid ) // TODO: Maybe we can parent it instead of doing this
			_weaponEntity.Transform = GetAttachment( "weapon" ).Value;
		if ( _isOpened && _sinceOpened > StayOpenedDuration )
			CloseBox();
	}
}
