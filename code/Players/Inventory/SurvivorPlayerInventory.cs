using System;
using System.Linq;
using Sandbox;
using SWB_Base;

namespace Survivor.Players.Inventory;

public class SurvivorPlayerInventory : InventoryBase
{
	// TODO: Rework inventory to support only required things
	// TODO: Main Weapon, Secondary Weapon, Grenade, Buffs

	public SurvivorPlayerInventory( PlayerBase player ) : base( player )
	{
	}

	public override bool Add( Entity ent, bool makeActive = false )
	{
		if ( Owner is not SurvivorPlayer player )
			return false;

		if ( ent is WeaponBase weapon )
		{
			if ( IsCarryingType( ent.GetType() ) )
			{
				// Inventory bug workaround (duplicate pickup)
				if ( weapon.TimeSinceActiveStart == 0 )
					return false;

				var ammo = weapon.Primary.Ammo;
				var ammoType = weapon.Primary.AmmoType;

				if ( ammo > 0 )
				{
					player.GiveAmmo( ammoType, ammo );

					if ( !player.SuppressPickupNotices )
					{
						Sound.FromWorld( "dm.pickup_ammo", ent.Position );
						PickupFeed.OnPickup( To.Single( player ), $"+{ammo} {ammoType}" );
					}
				}

				// Despawn it
				weapon.Delete();
				return false;
			}

			if ( !player.SuppressPickupNotices )
			{
				Sound.FromWorld( "dm.pickup_weapon", ent.Position );
			}
		}

		return base.Add( ent, makeActive );
	}

	public bool IsCarryingType( Type t )
	{
		return List.Any( x => x.GetType() == t );
	}
}
