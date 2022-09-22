using System.Diagnostics;
using Sandbox;
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

		// ViewModelPath = Asset.ViewModel;
		// WorldModelPath = Asset.WorldModel;

		General = Asset.GetWeaponInfos();
		Primary = Asset.GetPrimaryClipInfos();

		UISettings.ShowHealthCount = false;
		UISettings.ShowHealthIcon = false;
		UISettings.ShowFireMode = false;
		UISettings.ShowWeaponIcon = false;
		UISettings.ShowAmmoCount = false;
	}

	public WeaponAsset Asset { get; private set; }

	public void UpdateAsset( WeaponAsset asset )
	{
		if ( asset == null )
		{
			Log.Error("The specified asset is null !!!");
			return;
		}
		Asset = asset;
		General = Asset.GetWeaponInfos();
		Primary = Asset.GetPrimaryClipInfos();
	}

	// public override void ShootBullet( float spread, float force, float damage, float bulletSize, bool isPrimary )
	// {
	// 	// Spread
	// 	// var forward = Owner.EyeRotation.Forward;
	// 	// forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
	// 	// forward = forward.Normal;
	// 	// var endPos = Owner.EyePosition + forward * 999999;
	//
	// 	var forward = Rotation.Forward;
	// 	var endPos = Rotation.Forward * 999999;
	// 	var muzzle = GetModelAttachment( "muzzle" );
	// 	Log.Info( muzzle.Value.Position );
	// 	DebugOverlay.Sphere( muzzle.Value.Position, 2, Color.Red );
	// 	DebugOverlay.Line(muzzle.Value.Position, endPos, Color.Blue);
	//
	// 	// Server Bullet
	// 	if ( isPrimary )
	// 	{
	// 		Primary.BulletType.FireSV( this, muzzle.Value.Position, endPos, forward, spread, force, damage, bulletSize, isPrimary );
	// 	}
	// 	else
	// 	{
	// 		Secondary.BulletType.FireSV( this, muzzle.Value.Position, endPos, forward, spread, force, damage, bulletSize, isPrimary );
	// 	}
	//
	// 	// Client bullet
	// 	ShootClientBullet( muzzle.Value.Position, endPos, forward, spread, force, damage, bulletSize, isPrimary );
	// }
	//
	// public override void ShootClientBullet( Vector3 startPos, Vector3 endPos, Vector3 forward, float spread, float force, float damage, float bulletSize,
	//                                         bool isPrimary )
	// {
	// 	if ( Owner == null ) return;
	//
	// 	if ( isPrimary )
	// 	{
	// 		Primary.BulletType.FireCL( this, startPos, endPos, forward, spread, force, damage, bulletSize, isPrimary );
	// 	}
	// 	else
	// 	{
	// 		Secondary.BulletType.FireCL( this, startPos, endPos, forward, spread, force, damage, bulletSize, isPrimary );
	// 	}
	// }
}
