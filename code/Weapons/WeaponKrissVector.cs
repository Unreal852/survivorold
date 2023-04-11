using System.Collections.Generic;
using Sandbox;
using Survivor.Weapons.Attachements;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_kriss_vector", Title = "KRISS Vector" )]
public sealed class WeaponKrissVector : AbstractWeapon
{
	public WeaponKrissVector() : base( "kriss_vector" )
	{
		if ( Asset == null )
			return;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.2f, Rotation = 0.1f }; // TODO: Move this into WeaponAsset
		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 4.303f, -4.057f, 0.861f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( 0.17f, -0.15f, 0f ), Pos = new Vector3( -5.892f, -5f, 0.297f ) };
		ViewModelOffset = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 1.058f, -4.795f, -2.298f ) };

		AttachmentCategories = new()
		{
			new()
			{
				Name = AttachmentCategoryName.Sight,
				BoneOrAttachment = "sight",
				Attachments = new List<AttachmentBase>
				{
						new RmrSight()
						{
								ZoomAnimData = new AngPos { Angle = new Angles(0.02f, 0.49f, 0f), Pos = new Vector3(-5.86f, -4.795f, -0.86f) },
								BodyGroup = "sight",
						}
				}
			}
		};
	}

	public override string ViewModelPath => "models/weapons/smg/kriss_vector/vm_kriss_vector.vmdl";
	public override string WorldModelPath => "models/weapons/smg/kriss_vector/wm_kriss_vector.vmdl";
	public override AngPos ViewModelOffset { get; }
}
