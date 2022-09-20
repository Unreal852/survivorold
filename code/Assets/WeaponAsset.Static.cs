using System.Collections.Generic;
using Survivor.Weapons;

namespace Survivor.Assets;

public sealed partial class WeaponAsset
{
	private static readonly Dictionary<WeaponType, WeaponAsset> Weapons = new();

	private static void RegisterIfNotExists( WeaponAsset weaponAsset )
	{
		if ( Weapons.ContainsKey( weaponAsset.WeaponType ) )
			Weapons.Remove( weaponAsset.WeaponType );
		Weapons.Add( weaponAsset.WeaponType, weaponAsset );
	}

	public static WeaponAsset GetWeaponAsset( WeaponType weaponType )
	{
		return Weapons.ContainsKey( weaponType ) ? Weapons[weaponType] : default;
	}
}
