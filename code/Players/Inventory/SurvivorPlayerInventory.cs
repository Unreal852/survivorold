using System;
using System.Linq;
using Sandbox;
using SWB_Base;

namespace Survivor.Players.Inventory;

public class SurvivorPlayerInventory : InventoryBase
{
	public SurvivorPlayerInventory( PlayerBase player ) : base( player )
	{
	}

	public override bool Add( Entity ent, bool makeActive = false )
	{
		if ( Owner is not SurvivorPlayer player )
			return false;
		var weapon = ent as WeaponBase;
		var showNotice = !player.SuppressPickupNotices;

		if ( weapon != null && IsCarryingType( ent.GetType() ) )
		{
			// Inventory bug workaround (duplicate pickup)
			if ( weapon.TimeSinceActiveStart == 0 )
				return false;

			var ammo = weapon.Primary.Ammo;
			var ammoType = weapon.Primary.AmmoType;

			if ( ammo > 0 )
			{
				player.GiveAmmo( ammoType, ammo );

				if ( showNotice )
				{
					Sound.FromWorld( "dm.pickup_ammo", ent.Position );
					PickupFeed.OnPickup( To.Single( player ), $"+{ammo} {ammoType}" );
				}
			}

			// Despawn it
			weapon.Delete();
			return false;
		}

		if ( weapon != null && showNotice )
		{
			Sound.FromWorld( "dm.pickup_weapon", ent.Position );
		}

		return base.Add( ent, makeActive );
	}

	public bool IsCarryingType( Type t )
	{
		return List.Any( x => x.GetType() == t );
	}
}
