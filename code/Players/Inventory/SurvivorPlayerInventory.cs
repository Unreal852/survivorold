using System;
using System.Collections.Generic;
using Sandbox;
using Survivor.Weapons;
using SWB_Base;

namespace Survivor.Players.Inventory;

public class SurvivorPlayerInventory : IInventoryBase
{
	// TODO: Main Weapon, Secondary Weapon, Grenade, Buffs

	public SurvivorPlayerInventory( SurvivorPlayer player )
	{
		Owner = player;
	}

	public SurvivorPlayer Owner { get; init; }
	public List<Entity>   Slots { get; set; } = new();

	public virtual Entity Active
	{
		get => Owner.ActiveChild;
		protected set => Owner.ActiveChild = value;
	}

	public int Count()
	{
		return Slots.Count;
	}

	public virtual bool CanAdd( Entity ent )
	{
		return ent is AbstractWeapon bc && bc.CanCarry( Owner );
	}

	public void OnChildAdded( Entity child )
	{
		if ( !CanAdd( child ) )
			return;

		if ( Slots.Contains( child ) )
			throw new Exception( "Trying to add to inventory multiple times. This is gated by Entity:OnChildAdded and should never happen!" );

		if ( AreWeaponsSlotsFull() )
		{
			var activeSlot = GetActiveSlot();
			Slots.RemoveAt( activeSlot );
			Slots.Insert( activeSlot, child );
			return;
		}

		Slots.Add( child );
	}

	public void OnChildRemoved( Entity child )
	{
		if ( Slots.Remove( child ) )
		{
		}
	}

	public void DeleteContents()
	{
		Host.AssertServer();

		foreach ( var baseWeapon in Slots.ToArray() )
			baseWeapon.Delete();
		Slots.Clear();
	}

	public Entity GetSlot( int i )
	{
		if ( Slots.Count <= i )
			return null;
		if ( i < 0 )
			return null;
		return Slots[i];
	}

	public int GetActiveSlot()
	{
		var ae = Active;
		var count = Count();

		for ( int i = 0; i < count; i++ )
		{
			if ( Slots[i] == ae )
				return i;
		}

		return -1;
	}

	public bool SetActiveSlot( int i, bool allowEmpty = false )
	{
		var entity = GetSlot( i );
		if ( Active == entity )
			return false;
		if ( !allowEmpty && entity == null )
			return false;
		if ( !entity.IsValid )
			return false;
		Active = entity;
		return true;
	}

	public bool SwitchActiveSlot( int iDelta, bool loop )
	{
		var count = Count();
		if ( count == 0 ) return false;

		var slot = GetActiveSlot();
		var nextSlot = slot + iDelta;

		if ( loop )
		{
			while ( nextSlot < 0 ) nextSlot += count;
			while ( nextSlot >= count ) nextSlot -= count;
		}
		else
		{
			if ( nextSlot < 0 ) return false;
			if ( nextSlot >= count ) return false;
		}

		return SetActiveSlot( nextSlot );
	}

	public bool SetActive( Entity ent )
	{
		if ( Active == ent ) return false;
		if ( !Contains( ent ) ) return false;

		Active = ent;
		return true;
	}

	public bool Drop( Entity ent )
	{
		if ( !Host.IsServer )
			return false;

		if ( !Contains( ent ) )
			return false;

		ent.Parent = null;

		if ( ent is CarriableBase bc )
			bc.OnCarryDrop( Owner );

		return true;
	}

	public Entity DropActive()
	{
		if ( !Host.IsServer || Owner is not PlayerBase player )
			return null;

		var ent = player.ActiveChild;

		if ( ent is AbstractWeapon { CanDrop: true } && Drop( ent ) )
		{
			player.ActiveChild = null;
			return ent;
		}

		return null;
	}

	public bool Contains( Entity ent )
	{
		return Slots.Contains( ent );
	}

	public bool AreWeaponsSlotsFull()
	{
		return Slots.Count >= 2 && Slots[0] != null && Slots[1] != null;
	}

	public bool Add( Entity ent, bool makeActive = false )
	{
		Host.AssertServer();

		if ( ent is AbstractWeapon weapon )
		{
			if ( TryGetWeapon( weapon.Asset.WeaponType, out var ownedWep ) )
			{
				ownedWep.RefillAmmoReserve();
				if ( !Owner.SuppressPickupNotices )
				{
					Sound.FromWorld( "dm.pickup_ammo", ent.Position );
				}

				return false;
			}
		}

		// Can't pickup if already owned
		if ( ent.Owner != null )
			return false;

		if ( !CanAdd( ent ) )
			return false;

		if ( ent is not CarriableBase carriable )
			return false;

		if ( !carriable.CanCarry( Owner ) )
			return false;

		ent.Parent = Owner;
		carriable.OnCarryStart( Owner );

		if ( makeActive )
		{
			SetActive( ent );
		}

		return true;
	}

	public bool HasWeapon( WeaponType weaponType )
	{
		foreach ( var entity in Slots )
		{
			if ( entity is not AbstractWeapon slotWep )
				continue;
			if ( slotWep.Asset.WeaponType == weaponType )
				return true;
		}

		return false;
	}

	public bool HasWeapon( AbstractWeapon weapon )
	{
		return HasWeapon( weapon.Asset.WeaponType );
	}

	public bool TryGetWeapon( WeaponType weaponType, out AbstractWeapon weapon )
	{
		foreach ( var entity in Slots )
		{
			if ( entity is AbstractWeapon slotWep && slotWep.Asset.WeaponType == weaponType )
			{
				weapon = slotWep;
				return true;
			}
		}

		weapon = null;
		return false;
	}
}
