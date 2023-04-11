using System;
using Sandbox;
using Survivor.Extensions;
using Survivor.Weapons;
using Survivor.Weapons.Bullets;
using SWB_Base;

namespace Survivor.Assets;

[GameResource( "Weapon", "wpn", "Describes a weapon", Icon = "military_tech" )]
public sealed partial class WeaponAsset : GameResource
{
	[Category( "General" )]
	public string DisplayName { get; set; }

	[Category( "General" ), Title( "C# Type Name" ), Description( "This must match the C# class name" )]
	public string ClassName { get; set; }

	[Category( "General" )]
	public WeaponType WeaponType { get; set; }

	[Category( "General" )]
	public HoldType HoldType { get; set; } = HoldType.Pistol;

	[Category( "Model" ), Description( "The first person model" ), ResourceType( "vmdl" )]
	public string ViewModel { get; set; }

	[Category( "Model" ), ResourceType( "vmdl" )]
	public string WorldModel { get; set; }

	[Category( "Model" )]
	public int FieldOfView { get; set; } = 90;

	[Category( "Behaviour" )]
	public float DrawTime { get; set; } = 1f;

	[Category( "Behaviour" )]
	public float ReloadTime { get; set; } = 1f;

	[Category( "Behaviour" )]
	public float ReloadEmptyTime { get; set; } = -1f;

	[Category( "Behaviour" )]
	public float BoltBackTime { get; set; } = -1f;

	[Category( "Behaviour" )]
	public float BoltBackEjectDelay { get; set; } = 0f;

	[Category( "Ammo" )]
	public Weapons.AmmoType AmmoType { get; set; } = Survivor.Weapons.AmmoType.Pistol;

	[Category( "Ammo" )]
	public int Ammo { get; set; } = 10;

	[Category( "Ammo" )]
	public int MaxAmmo { get; set; } = 100;

	[Category( "Ammo" )]
	public int ClipSize { get; set; } = 10;

	[Category( "Shooting" )]
	public FiringType FiringType { get; set; } = FiringType.semi;

	[Category( "Shooting" ), Description( "Can a weapon have a bullet in its chamber ?" )]
	public bool BulletCocking { get; set; } = true;

	[Category( "Shooting" ), Description( "The amount of bullets per shot" )]
	public int Bullets { get; set; } = 1;

	[Category( "Shooting" )]
	public float BulletSize { get; set; } = 0.1f;

	[Category( "Shooting" )]
	public float Damage { get; set; } = 5;

	[Category( "Shooting" )]
	public float Force { get; set; } = 0.1f;

	[Category( "Shooting" )]
	public float Spread { get; set; } = 0.1f;

	[Category( "Shooting" )]
	public float Recoil { get; set; } = 0.1f;

	[Category( "Shooting" ), Description( "The rate of fire" )]
	public int RPM { get; set; } = 200;

	[Category( "Animations" )]
	public string ReloadAnim { get; set; } = "reload";

	[Category( "Animations" )]
	public string ReloadEmptyAnim { get; set; } = "reload_empty";

	[Category( "Animations" )]
	public string DrawAnim { get; set; } = "deploy";

	[Category( "Animations" )]
	public string DrawEmptyAnim { get; set; } = "deploy_empty";

	[Category( "Animations" )]
	public string BoltBackAnim { get; set; } = "boltback";

	[Category( "Animations" )]
	public string ShootAnim { get; set; } = "fire";

	[Category( "Animations" )]
	public string ShootZoomedAnim { get; set; }

	[Category( "Sounds" ), ResourceType( "sound" )]
	public string ShootSound { get; set; }

	[Category( "Sounds" ), ResourceType( "sound" )]
	public string DryFireSound { get; set; }

	[Category( "Particles" ), ResourceType( "vpcf" )]
	public string BulletEjectParticle { get; set; } = "particles/pistol_ejectbrass.vpcf";

	[Category( "Particles" ), ResourceType( "vpcf" )]
	public string MuzzleFlashParticle { get; set; } = "particles/swb/muzzle/flash_medium.vpcf";

	[Category( "Particles" ), ResourceType( "vpcf" )]
	public string BarrelSmokeParticle { get; set; } = "particles/swb/muzzle/barrel_smoke.vpcf";

	[Category( "Particles" ), ResourceType( "vpcf" )]
	public string BulletTracerParticle { get; set; } = "particles/swb/tracer/tracer_medium.vpcf";

	[Category( "UI" )]
	public float UiIconScale { get; set; } = 2.0f;

	[Category( "UI" )]
	public Vector3 UiIconOffset { get; set; } = Vector3.Zero;

	public WeaponInfo GetWeaponInfos()
	{
		return new WeaponInfo
		{
			FOV = FieldOfView,
			DrawTime = DrawTime,
			ReloadTime = ReloadTime,
			ReloadEmptyTime = ReloadEmptyTime,
			BoltBackTime = BoltBackTime,
			BoltBackEjectDelay = BoltBackEjectDelay,
			ReloadAnim = ReloadAnim,
			ReloadEmptyAnim = ReloadEmptyAnim,
			DrawAnim = DrawAnim,
			DrawEmptyAnim = DrawEmptyAnim,
			BoltBackAnim = BoltBackAnim
		};
	}

	public ClipInfo GetPrimaryClipInfos()
	{
		return new ClipInfo
		{
			Ammo = Ammo,
			AmmoReserve = MaxAmmo,
			AmmoType = AmmoType.ToSwbAmmoType(),
			ClipSize = ClipSize,
			BulletSize = BulletSize,
			Bullets = Bullets,
			Damage = Damage,
			Force = Force,
			Spread = Spread,
			Recoil = Recoil,
			RPM = RPM,
			FiringType = FiringType,
			BulletType = new TraceBullet(),
			//ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.9f, Rotation = 0.4f },
			DryFireSound = DryFireSound,
			ShootAnim = ShootAnim,
			ShootSound = ShootSound,
			ShootZoomedAnim = ShootZoomedAnim,
			BulletEjectParticle = BulletEjectParticle,
			MuzzleFlashParticle = MuzzleFlashParticle,
			BulletTracerParticle = BulletTracerParticle,
			BarrelSmokeParticle = BarrelSmokeParticle
		};
	}

	public Type GetWeaponClassType()
	{
		// TODO: Cache on post load
		return TypeLibrary.GetType( ClassName ).TargetType;
	}

	public AbstractWeapon CreateWeaponInstance()
	{
		// TODO: Cache on post load
		return TypeLibrary.Create<AbstractWeapon>( ClassName );
	}

	protected override void PostLoad()
	{
		RegisterIfNotExists( this );
	}

	protected override void PostReload()
	{
		RegisterIfNotExists( this );
		if ( !Game.IsServer )
			return;
		foreach ( var entity in Entity.All )
		{
			if ( entity is AbstractWeapon weapon && weapon.Asset.WeaponType == WeaponType )
				weapon.UpdateAsset( this );
		}
	}
}
