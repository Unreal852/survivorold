using SWB_Base;

namespace Survivor.Weapons;

public abstract partial class BaseWeapon : WeaponBase
{
	protected BaseWeapon()
	{
		UISettings.ShowHealthCount = false;
		UISettings.ShowHealthIcon = false;
		UISettings.ShowFireMode = false;
	}
}
