using System.Collections.Generic;
using Sandbox;
using SWB_Base;
using SWB_Base.Attachments;

namespace Survivor.Weapons;

[Library( "survivor_fn57", Title = "FN57" )]
public sealed class FN57 : ABaseWeapon
{
	public FN57() : base( "fn57" )
	{
		if ( Asset == null )
			return;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.9f, Rotation = 0.4f }; // TODO: Move this into WeaponAsset
		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.185f, -12.172f, 0.301f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( 0.46f, -0.14f, 0f ), Pos = new Vector3( -3.928f, -4.795f, 0.745f ) };
		ViewModelOffset = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -10.56f, -2.4f ) };

		AttachmentCategories = new();
		AttachmentCategories.Add( new()
		{
				Name = AttachmentCategoryName.Sight,
				BoneOrAttachment = "sight",
				Attachments = new List<AttachmentBase>
				{
						new RedDot
						{
								ZoomAnimData = new AngPos { Angle = new Angles( 0.72f, -0.12f, 0f ), Pos = new Vector3( -3.921f, -4.795f, 0.544f ) },
								ViewParentBone = "sight",
								ViewTransform =
										new Transform { Position = new Vector3( 0, 0, 0 ), Rotation = Rotation.From( new Angles( 0.05f, -0.49f, 0f ) ), Scale = 1f },
								WorldParentBone = "sight",
								WorldTransform = new Transform
								{
										Position = new Vector3( 0, 0, 0 ),
										//Rotation = Rotation.From( new Angles( -90f, 0f, -90f ) ),
										Scale = 4f
								}
						}
				}
		} );
	}

	public override string   ViewModelPath   => "models/weapons/pistols/fn57/vm_fn57.vmdl";
	public override string   WorldModelPath  => "models/weapons/pistols/fn57/wm_fn57.vmdl";
	public override HoldType HoldType        { get; } = HoldType.Pistol; // TODO: Move this into WeaponAsset
	public override AngPos   ViewModelOffset { get; }
}

public class RedDot : Sight
{
	public override string Name      => "Reflex";
	public override string IconPath  => "";
	public override string ModelPath => "models/attachments/red_dot.vmdl";
}
