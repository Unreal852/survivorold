using Survivor.Assets;
using Survivor.Weapons.Bullets;
using SWB_Base;

namespace Survivor.Weapons;

public abstract partial class ABaseWeapon : WeaponBase
{
	protected ABaseWeapon( string weaponAssetName )
	{
		Asset = ResourceLibrary.Get<WeaponAsset>( $"config/weapons/{weaponAssetName}.weapon" );
		if ( Asset == null )
		{
			Log.Error( $"No weapon asset found with name '{weaponAssetName}'" );
			return;
		}

		ViewModelPath = Asset.ViewModel;
		WorldModelPath = Asset.WorldModel;

		General = Asset.GetWeaponInfos();
		Primary = Asset.GetPrimaryClipInfos();

		UISettings.ShowHealthCount = false;
		UISettings.ShowHealthIcon = false;
		UISettings.ShowFireMode = false;
		UISettings.ShowWeaponIcon = false;
		UISettings.ShowAmmoCount = false;
	}

	public          WeaponAsset Asset          { get; private set; }
	public override string      ViewModelPath  { get; }
	public override string      WorldModelPath { get; }
}
