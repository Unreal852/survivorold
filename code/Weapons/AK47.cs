using System.Collections.Generic;
using Sandbox;
using Survivor.Weapons.Bullets;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_ak47", Title = "AK47" )]
public sealed class AK47 : WeaponBase
{
	public override HoldType HoldType        => HoldType.Rifle;
	public override string   ViewModelPath   => "models/weapons/assault_rifles/ak47/vm_ak47.vmdl";
	public override string   WorldModelPath  => "models/weapons/assault_rifles/ak47/wm_ak47.vmdl";
	public override AngPos   ViewModelOffset { get; } = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -5f, 0f ) };

	//public override string   HandsModelPath => "models/first_person/first_person_arms.vmdl";

	public AK47()
	{
		// Todo: This should not be here
		UISettings.ShowHealthCount = false;
		UISettings.ShowHealthIcon = false;
		General = new WeaponInfo { DrawTime = 1f, ReloadTime = 2.8f, };
		Primary = new ClipInfo
		{
				Ammo = 30,
				AmmoType = AmmoType.Rifle,
				ClipSize = 30,
				BulletSize = 3f,
				BulletType = new TraceBullet(),
				Damage = 10f,
				Force = 5f,
				Spread = 0.08f,
				Recoil = 0.35f,
				RPM = 900,
				FiringType = FiringType.auto,
				ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.2f, Rotation = 0.1f },
				ShootAnim = "w_fire",
				DryFireSound = "swb_rifle.empty",
				ShootSound = "sounds/weapons/ak47/ak_47_shot_01.sound",
				BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
				MuzzleFlashParticle = "particles/swb/muzzle/flash_medium.vpcf",
				BulletTracerParticle = "",
				InfiniteAmmo = InfiniteAmmoType.reserve
		};

		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.184f, 0f, 8.476f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( -14.635f, -5f, 7.812f ) };
	}

	protected override void ShootEffects( string muzzleFlashParticle, string bulletEjectParticle, string shootAnim )
	{
		ViewModelEntity?.SetAnimParameter( "w_fire_speed", 3f );
		base.ShootEffects( muzzleFlashParticle, bulletEjectParticle, shootAnim );
	}

	public override void StartReloadEffects( bool isEmpty, string reloadAnim = null )
	{
		ViewModelEntity?.SetAnimParameter( "w_reloading", true );
	}
}
