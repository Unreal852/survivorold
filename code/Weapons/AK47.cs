﻿using System.Collections.Generic;
using Sandbox;
using Survivor.Weapons.Bullets;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_ak47", Title = "AK47" )]
public class AK47 : WeaponBase
{
	public override HoldType HoldType       => HoldType.Pistol;
	public override string   ViewModelPath  => "models/weapons/assault_rifles/ak47/vm_ak47.vmdl";
	public override string   WorldModelPath => "models/weapons/assault_rifles/ak47/wm_ak47.vmdl";

	//public override string   HandsModelPath => "models/first_person/first_person_arms.vmdl";

	public override int FOV => 54;

	public AK47()
	{
		// Todo: This should not be here
		UISettings.ShowHealthCount = false;
		UISettings.ShowHealthIcon = false;
		General = new WeaponInfo { DrawTime = 1f, ReloadTime = 1f, };
		Primary = new ClipInfo
		{
				Ammo = 30,
				AmmoType = AmmoType.Rifle,
				ClipSize = 30,
				BulletSize = 3f,
				BulletType = new TraceBullet(),
				Damage = 15f,
				Force = 3f,
				Spread = 0.08f,
				Recoil = 0.35f,
				RPM = 900,
				FiringType = FiringType.auto,
				ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1f, Rotation = 0.1f },
				ShootAnim = "shoot",
				DryFireSound = "swb_rifle.empty",
				ShootSound = "sounds/weapons/colt_m1911/colt_m1911_shot_01.sound",
				BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
				MuzzleFlashParticle = "particles/swb/muzzle/flash_medium.vpcf",
				BulletTracerParticle = "particles/swb/tracer/phys_tracer_medium.vpcf",
				InfiniteAmmo = InfiniteAmmoType.reserve
		};

		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 5f, 0f, 0f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( -0.1f, 0.87f, 0f ), Pos = new Vector3( -26.944f, -10f, 2.649f ) };

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