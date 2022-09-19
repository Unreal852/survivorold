using Sandbox;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_kriss_vector", Title = "KRISS Vector" )]
public sealed class KrissVector : ABaseWeapon
{
	public KrissVector() : base( "kriss_vector" )
	{
		if ( Asset == null )
			return;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.2f, Rotation = 0.1f }; // TODO: Move this into WeaponAsset
		RunAnimData = new AngPos { Angle = new Angles(27.7f, 39.95f, 0f), Pos = new Vector3(4.303f, -4.057f, 0.861f) };
		ZoomAnimData = new AngPos { Angle = new Angles(0.17f, -0.15f, 0f), Pos = new Vector3(-5.892f, -5f, 0.297f) };
		ViewModelOffset = new AngPos { Angle = new Angles(0f, 0f, 0f), Pos = new Vector3(1.058f, -4.795f, -2.298f) };
	}

	public override string   ViewModelPath   => "models/weapons/pdw/kriss_vector/vm_kriss_vector.vmdl";
	public override string   WorldModelPath  => "models/weapons/pdw/kriss_vector/wm_kriss_vector.vmdl";
	public override HoldType HoldType        { get; } = HoldType.Rifle; // TODO: Move this into WeaponAsset
	public override AngPos   ViewModelOffset { get; }
}
