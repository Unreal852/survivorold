using System.Collections.Generic;
using Sandbox;
using SWB_Base;
using SWB_Base.Attachments;

namespace Survivor.Weapons;

[Library( "survivor_glock_g18", Title = "Glock G18" )]
public class Glock18 : WeaponBase
{
	public override HoldType HoldType       => HoldType.Pistol;
	public override string   ViewModelPath  => "models/weapons/pistols/glock_18/vm_glock18.vmdl";
	public override string   WorldModelPath => "models/weapons/pistols/glock_18/wm_glock18.vmdl";
	public override string   HandsModelPath => "models/first_person/first_person_arms.vmdl";

	//public override AngPos   ViewModelOffset => new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 11.304f, 22.609f, -11.739f ) };

	public Glock18()
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
				RPM = 900,
				FiringType = FiringType.auto,
				ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1f, Rotation = 0.1f },
				DryFireSound = "swb_rifle.empty",
				ShootSound = "sounds/weapons/colt_m1911/colt_m1911_shot_01.sound",
				BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
				MuzzleFlashParticle = "particles/swb/muzzle/flash_medium.vpcf",
				BulletTracerParticle = "particles/swb/tracer/phys_tracer_medium.vpcf",
		};

		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 5f, 0f, 0f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( -0.43f, -0.05f, 0f ), Pos = new Vector3( -4.353f, 0f, 0.51f ) };

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
