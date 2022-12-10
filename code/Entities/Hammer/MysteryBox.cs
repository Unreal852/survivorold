using System;
using Editor;
using Sandbox;
using Survivor.Assets;
using Survivor.Interaction;
using Survivor.Players;
using Survivor.UI.World;
using Survivor.Weapons;

// resharper disable all

namespace Survivor.Entities.Hammer;

[Library( "survivor_mystery_box" )]
[Category( "Map" ), Icon( "place" )]
[Title( "Mystery Box" ), Description( "This entity defines a mystery box" )]
[HammerEntity, SupportsSolid, Model( Model = "models/objects/weapons_box.vmdl", Archetypes = ModelArchetype.animated_model )]
[RenderFields, VisGroup( VisGroup.Dynamic )]
public sealed partial class MysteryBox : AnimatedEntity, IUsable
{
	private WeaponWorldModel _weaponEntity;
	private TimeSince        _sinceOpened;

	public MysteryBox()
	{
	}

	[Property]
	[Title( "Enabled" ), Description( "Unchecking this will prevent this door from being bought" )]
	public bool IsEnabled { get; set; } = true;

	[Property, Net]
	[Title( "Cost" ), Description( "The price to use this machine" )]
	public int Cost { get; set; } = 0;

	public float StayOpenedDuration { get; set; } = 8f;

	[Net]
	public bool IsOpened { get; set; }

	[Net]
	public bool IsOpening { get; set; }

	[Net]
	public bool IsClosing { get; set; }

	[Net]
	public WeaponAsset WeaponAsset { get; set; }

	[Net]
	public Entity LastUser { get; set; }

	public int    UseCost    => Cost;
	public string UseMessage => IsOpened && WeaponAsset != null ? $"Take {WeaponAsset.DisplayName}" : "May the luck be with you";
	public bool   HasCost    => !IsOpened && Cost > 0;

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
	}

	// TODO: Make events for thoses
	protected override void OnAnimGraphTag( string tag, AnimGraphTagEvent fireMode )
	{
		if ( !IsServer )
			return;
		if ( tag == "Opening" )
			IsOpening = fireMode == AnimGraphTagEvent.Start;
		else if ( tag == "Closing" )
			IsClosing = fireMode == AnimGraphTagEvent.Start;
		else if ( tag == "Opened" )
		{
			_sinceOpened = 0;
			IsOpened = fireMode == AnimGraphTagEvent.Start;
			if ( IsOpened )
			{
				var timerSpawn = GetAttachment( "timer" );
				if ( !timerSpawn.HasValue )
					return;
				MysteryBoxTimer.SpawnMysteryBoxTimerClient( timerSpawn.Value.Position, timerSpawn.Value.Rotation, StayOpenedDuration );
			}
		}
	}

	private void SpawnRandomWeapon()
	{
		DeleteWeapon();

		var wepSpawn = GetAttachment( "weapon" );
		if ( wepSpawn.HasValue )
		{
			var weaponTypes = Enum.GetValues<WeaponType>();
			var randIndex = Game.Random.Int( 0, weaponTypes.Length - 1 );
			WeaponAsset = WeaponAsset.GetWeaponAsset( weaponTypes[randIndex] );
			_weaponEntity = new WeaponWorldModel( WeaponAsset ) { Transform = wepSpawn.Value, Parent = this };
		}
		else
			Log.Warning( "Missing 'weapon' attachement on the mystery box" );
	}

	private void DeleteWeapon()
	{
		WeaponAsset = null;
		if ( _weaponEntity != null && _weaponEntity.IsValid )
		{
			_weaponEntity.Components.RemoveAll();
			_weaponEntity.Delete();
			_weaponEntity = null;
		}
		else
			_weaponEntity = null;
	}

	public void OpenBox()
	{
		if ( IsOpening || IsClosing || IsOpened )
			return;
		SpawnRandomWeapon();
		SetAnimParameter( "open", true );
	}

	public void CloseBox()
	{
		if ( IsOpening || IsClosing || !IsOpened )
			return;
		SetAnimParameter( "close", true );
		LastUser = null;
	}

	public bool OnUse( Entity user )
	{
		if ( !IsEnabled )
			return false;
		if ( user is SurvivorPlayer player && player.TryUse() )
		{
			if ( !IsOpened && player.Money >= Cost )
			{
				LastUser = player;
				player.Money -= Cost;
				OpenBox();
				return false;
			}

			if ( IsOpened && user == LastUser && WeaponAsset != null )
			{
				player.Inventory.Add( WeaponAsset.CreateWeaponInstance(), true );
				MysteryBoxTimer.DeleteMysteryBoxTimerClient();
				DeleteWeapon();
				CloseBox();
			}
		}

		return false;
	}

	public bool IsUsable( Entity user )
	{
		if ( IsOpened )
			return user == LastUser;
		return !IsOpening && !IsClosing;
	}

	[Event.Tick.Server]
	public void OnTick()
	{
		if ( IsOpening || IsClosing && _weaponEntity != null && _weaponEntity.IsValid )
			_weaponEntity.Transform = GetAttachment( "weapon" ).Value; // TODO: Maybe we can parent it instead of doing this
		if ( IsOpened && _sinceOpened > StayOpenedDuration )
			CloseBox();
	}
}
