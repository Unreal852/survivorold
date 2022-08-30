using Sandbox;
using SWB_Base;
using SWB_Base.Bullets;

namespace Survivor.Weapons
{
	[Library( "survivor_colt_m1911", Title = "Colt M1911" )]
	public class M1911 : WeaponBase
	{
		public override HoldType HoldType       => HoldType.Pistol;
		public override string   ViewModelPath  => "models/1911.vmdl";
		public override string   WorldModelPath => "models/1911.vmdl";
		public override string   Icon           => "icons/m1911_logo_02.png";

		public M1911()
		{
			// Todo: This should not be here
			UISettings.ShowHealthCount = false;
			UISettings.ShowHealthIcon = false;
			General = new WeaponInfo { DrawTime = 1f, ReloadTime = 1f, };
			Primary = new ClipInfo
			{
					Ammo = 7,
					AmmoType = AmmoType.Pistol,
					ClipSize = 7,
					BulletSize = 3f,
					BulletType = new PistolBullet(),
					Damage = 15f,
					Force = 3f,
					Spread = 0.08f,
					Recoil = 0.35f,
					RPM = 300,
					FiringType = FiringType.semi,
					ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1f, Rotation = 0.1f },
					DryFireSound = "swb_rifle.empty",
					ShootSound = "sounds/m1911_shot_01.sound",
					BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
					MuzzleFlashParticle = "particles/swb/muzzle/flash_medium.vpcf",
					BulletTracerParticle = "particles/swb/tracer/phys_tracer_medium.vpcf",
			};

			RunAnimData = new AngPos { Angle = new Angles( 10, 40, 0 ), Pos = new Vector3( 5, 0, 0 ) };
			ZoomAnimData = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( -0.99f, 0f, 2.235f ) };
		}

		public override void CreateHudElements()
		{
			base.CreateHudElements();
		}
	}
}
