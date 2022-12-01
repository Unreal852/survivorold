using System;
using SWB_Base;
using AmmoType = Survivor.Weapons.AmmoType;
using SwbAmmoType = SWB_Base.AmmoType;

namespace Survivor.Extensions;

public static class AmmoTypeExtensions
{
	public static SwbAmmoType ToSwbAmmoType( this AmmoType ammoType )
	{
		return ammoType switch
		{
				AmmoType.Pistol    => AmmoTypes.Pistol,
				AmmoType.Revolver  => AmmoTypes.Revolver,
				AmmoType.Shotgun   => AmmoTypes.Shotgun,
				AmmoType.SMG       => AmmoTypes.SMG,
				AmmoType.Rifle     => AmmoTypes.Rifle,
				AmmoType.Sniper    => AmmoTypes.Sniper,
				AmmoType.LMG       => AmmoTypes.LMG,
				AmmoType.Crossbow  => AmmoTypes.Crossbow,
				AmmoType.RPG       => AmmoTypes.RPG,
				AmmoType.Explosive => AmmoTypes.Explosive,
				AmmoType.Grenade   => AmmoTypes.Grenade,
				_                  => throw new Exception( "This should not happen" )
		};
	}
}
