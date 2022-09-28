using System.Collections.Generic;
using Sandbox;
using SWB_Base;
using SWB_Base.Attachments;

namespace Survivor.Weapons;

[Library( "survivor_kriss_vector", Title = "KRISS Vector" )]
public sealed class WeaponKrissVector : ABaseWeapon
{
	public WeaponKrissVector() : base( "kriss_vector" )
	{
		if ( Asset == null )
			return;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.2f, Rotation = 0.1f }; // TODO: Move this into WeaponAsset
		RunAnimData = new AngPos { Angle = new Angles(27.7f, 39.95f, 0f), Pos = new Vector3(4.303f, -4.057f, 0.861f) };
		ZoomAnimData = new AngPos { Angle = new Angles(0.17f, -0.15f, 0f), Pos = new Vector3(-5.892f, -5f, 0.297f) };
		ViewModelOffset = new AngPos { Angle = new Angles(0f, 0f, 0f), Pos = new Vector3(1.058f, -4.795f, -2.298f) };
		
		AttachmentCategories = new();
		AttachmentCategories.Add( new()
		{
				Name = AttachmentCategoryName.Sight,
				BoneOrAttachment = "sight",
				Attachments = new List<AttachmentBase>
				{
						new Rmr()
						{
								ZoomAnimData = new AngPos { Angle = new Angles(0.02f, 0.49f, 0f), Pos = new Vector3(-5.86f, -4.795f, -0.86f) },
								ViewParentBone = "sight",
								ViewTransform =
										new Transform { Position = new Vector3( 0, 0, 0 ), Scale = 1f },
								WorldParentBone = "sight",
								WorldTransform = new Transform
								{
										Position = new Vector3( 0, 0, 0 ),
										//Rotation = Rotation.From( new Angles( -90f, 0f, -90f ) ),
										Scale = 1f
								}
						}
				}
		} );
	}

	public override string ViewModelPath  => "models/weapons/smg/kriss_vector/vm_kriss_vector.vmdl";
	public override string WorldModelPath => "models/weapons/smg/kriss_vector/wm_kriss_vector.vmdl";
	public override AngPos   ViewModelOffset { get; }
}

public class Rmr : Sight
{
	public override string Name      => "Trijicon RMR";
	public override string IconPath  => "";
	public override string ModelPath => "models/attachments/trijicon_rmr.vmdl";
}
