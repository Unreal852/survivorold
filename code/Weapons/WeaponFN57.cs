using System.Collections.Generic;
using Sandbox;
using Survivor.Weapons.Attachements;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_fn57", Title = "FN57" )]
public sealed class WeaponFN57 : AbstractWeapon
{
	public WeaponFN57() : base( "fn57" )
	{
		if ( Asset == null )
			return;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.9f, Rotation = 0.4f }; // TODO: Move this into WeaponAsset
		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.185f, -3.566f, 0.301f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( 0.46f, -0.14f, 0f ), Pos = new Vector3( -3.943f, -0.49f, 0.774f ) };
		ViewModelOffset = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -10.56f, -2.4f ) };

		AttachmentCategories = new()
		{
			new()
			{
				Name = AttachmentCategoryName.Sight,
				BoneOrAttachment = "sight",
				Attachments = new List<AttachmentBase>
				{
						new RedDotSight
						{
								ZoomAnimData = new AngPos { Angle = new Angles(0.46f, -0.14f, 0f), Pos = new Vector3(-3.941f, -0.49f, 0.504f) },
								BodyGroup = "sight"
						}
				}
			}
		};
	}

	public override string ViewModelPath => "models/weapons/pistols/fn57/vm_fn57.vmdl";
	public override string WorldModelPath => "models/weapons/pistols/fn57/wm_fn57.vmdl";
	public override AngPos ViewModelOffset { get; }
}
