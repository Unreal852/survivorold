using Sandbox;
using Survivor.Weapons.Bullets;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_magnum", Title = "Magnum" )]
public sealed class Magnum : BaseWeapon
{
	public override HoldType HoldType        => HoldType.Pistol;
	public override string   ViewModelPath   => "models/weapons/pistols/magnum/vm_magnum.vmdl";
	public override string   WorldModelPath  => "models/weapons/pistols/magnum/wm_magnum.vmdl";
	public override AngPos   ViewModelOffset { get; } = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -30f, 0f ) };

	//public override string   HandsModelPath => "models/first_person/first_person_arms.vmdl";

	public Magnum()
	{
		General = new WeaponInfo { DrawTime = 1f, ReloadTime = 2.8f, };
		Primary = new ClipInfo
		{
				Ammo = 6,
				AmmoType = AmmoType.Pistol,
				ClipSize = 6,
				BulletSize = 3f,
				BulletType = new TraceBullet(),
				Damage = 30f,
				Force = 8f,
				Spread = 0.1f,
				Recoil = 2f,
				RPM = 80,
				FiringType = FiringType.semi,
				ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.9f, Rotation = 0.4f },
				DryFireSound = "swb_rifle.empty",
				ShootAnim = "w_fire",
				ShootSound = "sounds/weapons/magnum/magnum_shot_01.sound",
				BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
				MuzzleFlashParticle = "particles/swb/muzzle/flash_medium.vpcf",
				BulletTracerParticle = "particles/swb/tracer/phys_tracer_medium.vpcf",
				InfiniteAmmo = InfiniteAmmoType.reserve
		};


		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.955f, -28.402f, 2.965f ) };
		DuckAnimData = new AngPos { Angle = new Angles( 5.08f, -2.89f, -25.082f ), Pos = new Vector3( -49.547f, 0f, 2.885f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( -32.063f, -11f, 8.161f ) };
	}

	protected override void ShootEffects( string muzzleFlashParticle, string bulletEjectParticle, string shootAnim )
	{
		ViewModelEntity?.SetAnimParameter( "w_fire_speed", 1f );
		base.ShootEffects( muzzleFlashParticle, bulletEjectParticle, shootAnim );
	}

	public override void StartReloadEffects( bool isEmpty, string reloadAnim = null )
	{
		// TODO: Move with WeaponInfos
		ViewModelEntity?.SetAnimParameter( "w_reloading", true );
	}
}
