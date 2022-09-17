using System.Collections.Generic;
using Sandbox;
using Survivor.Entities.Zombies;
using Survivor.Utils;
using SWB_Base;
using BaseWeapon = Survivor.Weapons.BaseWeapon;

namespace Survivor.Assets;

[GameResource( "Weapon", "weapon", "Describes a weapon", Icon = "military_tech" )]
public class WeaponAsset : GameResource
{
	[Category( "General" )]
	public string Name { get; set; }

	[Category( "Behaviour" )]
	public float DrawTime { get; set; } = 0.5f;

	[Category( "Behaviour" )]
	public float ReloadTime { get; set; } = 1f;

	[Category( "Behaviour" )]
	public float ReloadEmptyTime { get; set; } = -1f;

	[Category( "Behaviour" )]
	public float BoltBackTime { get; set; } = -1f;

	[Category( "Behaviour" )]
	public float BoltBackEjectDelay { get; set; } = 0f;

	[Category( "Animations" )]
	public string ReloadAnim { get; set; } = "reload";

	[Category( "Animations" )]
	public string ReloadEmptyAnim { get; set; } = "reload_empty";

	[Category( "Animations" )]
	public string DrawAnim { get; set; } = "deploy";

	[Category( "Animations" )]
	public string DrawEmptyAnim { get; set; } = "";

	[Category( "Animations" )]
	public string BoltBackAnim { get; set; } = "boltback";

	[Category( "Primary Clip" )]
	public AmmoType AmmoType { get; set; } = AmmoType.Pistol;

	[Category( "Primary Clip" )]
	public FiringType FiringType { get; set; } = FiringType.semi;

	[Category( "Primary Clip" )]
	public int Ammo { get; set; } = 10;

	[Category( "Primary Clip" )]
	public int ClipSize { get; set; } = 10;

	[Category( "Primary Clip" )]
	public int Bullets { get; set; } = 1;

	[Category( "Primary Clip" )]
	public float BulletSize { get; set; } = 0.1f;

	[Category( "Primary Clip" )]
	public float Damage { get; set; } = 5;

	[Category( "Primary Clip" )]
	public float Force { get; set; } = 0.1f;

	[Category( "Primary Clip" )]
	public float Spread { get; set; } = 0.1f;

	[Category( "Primary Clip" )]
	public float Recoil { get; set; } = 0.1f;

	[Category( "Primary Clip" )]
	public int RPM { get; set; } = 200;

	// [Category( "Animations" )]
	// public ScreenShake ScreenShake { get; set; }

	[Category( "Animations" )]
	public string ShootAnim { get; set; } = "fire";

	[Category( "Animations" )]
	public string ShootZoomedAnim { get; set; }

	[Category( "Animations" )]
	public string DryFireSound { get; set; }

	[Category( "Animations" )]
	public string ShootSound { get; set; }

	[Category( "Animations" )]
	public string BulletEjectParticle { get; set; }

	[Category( "Animations" )]
	public string MuzzleFlashParticle { get; set; }

	[Category( "Animations" )]
	public string BarrelSmokeParticle { get; set; } = "particles/swb/muzzle/barrel_smoke.vpcf";

	[Category( "Animations" )]
	public string BulletTracerParticle { get; set; } = "particles/swb/tracer/tracer_medium.vpcf";

	public void Apply( BaseWeapon weapon )
	{
	}

	protected override void PostLoad()
	{
	}

	protected override void PostReload()
	{
	}
}
