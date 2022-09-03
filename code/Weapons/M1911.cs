using System.Collections.Generic;
using Sandbox;
using SWB_Base;
using SWB_Base.Attachments;
using SWB_Base.Bullets;

namespace Survivor.Weapons;

[Library( "survivor_colt_m1911", Title = "Colt M1911" )]
public class M1911 : WeaponBase
{
	public override HoldType HoldType        => HoldType.Pistol;
	public override string   ViewModelPath   => "models/weapons/v_glock18.vmdl";
	public override string   WorldModelPath  => "models/weapons/glock18.vmdl";
	public override string   HandsModelPath  => "models/first_person/first_person_arms.vmdl";
	public override AngPos   ViewModelOffset => new() { Angle = new Angles( 0f, 5.217f, 0f ), Pos = new Vector3( 13.043f, 18.261f, -7.391f ) };
	public override string   Icon            => "icons/m1911_logo_02.png";

	public M1911()
	{
		// Todo: This should not be here
		UISettings.ShowHealthCount = false;
		UISettings.ShowHealthIcon = false;
		General = new WeaponInfo { DrawTime = 1f, ReloadTime = 1f, };
		Primary = new ClipInfo
		{
				Ammo = 17,
				AmmoType = AmmoType.Pistol,
				ClipSize = 17,
				BulletSize = 3f,
				BulletType = new HitScanBullet(),
				Damage = 15f,
				Force = 3f,
				Spread = 0.08f,
				Recoil = 0.35f,
				RPM = 300,
				FiringType = FiringType.semi,
				ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1f, Rotation = 0.1f },
				DryFireSound = "swb_rifle.empty",
				ShootSound = "sounds/weapons/colt_m1911/colt_m1911_shot_01.sound",
				BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
				MuzzleFlashParticle = "particles/swb/muzzle/flash_medium.vpcf",
				BulletTracerParticle = "particles/swb/tracer/phys_tracer_medium.vpcf",
		};

		RunAnimData = new AngPos { Angle = new Angles( 10, 40, 0 ), Pos = new Vector3( 5, 0, 0 ) };
		ZoomAnimData = new AngPos { Angle = new Angles( -0.19f, -5.25f, 0f ), Pos = new Vector3( -11.666f, 0f, 1.64f ) };

		AttachmentCategories = new List<AttachmentCategory>()
		{
				new()
				{
						Name = AttachmentCategoryName.Muzzle,
						BoneOrAttachment = "muzzle",
						Attachments = new List<AttachmentBase>()
						{
								new M1911Silencer
								{
										MuzzleFlashParticle = "particles/swb/muzzle/flash_medium_silenced.vpcf",
										ShootSound = "sounds/weapons/colt_m1911/colt_m1911_shot_01.sound",
										ViewParentBone = "root",
										ViewTransform
												= new Transform
												{
														Position = new Vector3( 17.204f, 0.86f, 3.871f ),
														Rotation = Rotation.From( new Angles( 0f, 0f, 0f ) ),
														Scale = 5f
												},
										WorldParentBone = "root",
										WorldTransform = new Transform
										{
												Position = new Vector3( 17.204f, 0.86f, 3.871f ),
												Rotation = Rotation.From( new Angles( 0f, 0f, 0f ) ),
												Scale = 5f
										},
								}
						}
				},
		};
	}
}

public class M1911Silencer : Silencer
{
	public override string Name      => "Silencer";
	public override string IconPath  => "attachments/swb/muzzle/silencer_pistol/ui/icon.png";
	public override string ModelPath => "models/attachments/glock18_silencer.vmdl";
}
