using Survivor.Assets;
using Survivor.Weapons.Bullets;
using SWB_Base;

namespace Survivor.Weapons;

public abstract partial class BaseWeapon : WeaponBase
{
	protected BaseWeapon( string weaponAssetName )
	{
		Asset = ResourceLibrary.Get<WeaponAsset>( $"config/weapons/{weaponAssetName}.weapon" );
		if ( Asset == null )
		{
			Log.Error( $"No weapon asset found with name '{weaponAssetName}'" );
			return;
		}

		ViewModelPath = Asset.ViewModel;
		WorldModelPath = Asset.WorldModel;

		General = new WeaponInfo
		{
				DrawTime = Asset.DrawTime,
				ReloadTime = Asset.ReloadTime,
				ReloadEmptyTime = Asset.ReloadEmptyTime,
				BoltBackTime = Asset.BoltBackTime,
				BoltBackEjectDelay = Asset.BoltBackEjectDelay,
				ReloadAnim = Asset.ReloadAnim,
				ReloadEmptyAnim = Asset.ReloadEmptyAnim,
				DrawAnim = Asset.DrawAnim,
				DrawEmptyAnim = Asset.DrawEmptyAnim,
				BoltBackAnim = Asset.BoltBackAnim
		};
		Primary = new ClipInfo
		{
				Ammo = Asset.Ammo,
				AmmoType = Asset.AmmoType,
				ClipSize = Asset.ClipSize,
				BulletSize = Asset.BulletSize,
				Bullets = Asset.Bullets,
				Damage = Asset.Damage,
				Force = Asset.Force,
				Spread = Asset.Spread,
				Recoil = Asset.Recoil,
				RPM = Asset.RPM,
				FiringType = Asset.FiringType,
				BulletType = new TraceBullet(),
				ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.9f, Rotation = 0.4f },
				DryFireSound = Asset.DryFireSound,
				ShootAnim = Asset.ShootAnim,
				ShootSound = Asset.ShootSound,
				ShootZoomedAnim = Asset.ShootZoomedAnim,
				BulletEjectParticle = Asset.BulletEjectParticle,
				MuzzleFlashParticle = Asset.MuzzleFlashParticle,
				BulletTracerParticle = Asset.BulletTracerParticle,
				BarrelSmokeParticle = Asset.BarrelSmokeParticle
		};

		UISettings.ShowHealthCount = false;
		UISettings.ShowHealthIcon = false;
		UISettings.ShowFireMode = false;
	}

	public          WeaponAsset Asset          { get; private set; }
	public override string      ViewModelPath  { get; }
	public override string      WorldModelPath { get; }
}
