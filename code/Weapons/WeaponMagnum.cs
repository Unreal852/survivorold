using Sandbox;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_magnum", Title = "Magnum" )]
public sealed class WeaponMagnum : ABaseWeapon
{
	public WeaponMagnum() : base( "magnum" )
	{
		if ( Asset == null )
			return;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.9f, Rotation = 0.4f }; // TODO: Move this into WeaponAsset

		RunAnimData = new() { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.955f, -28.402f, 2.965f ) };
		//DuckAnimData = new() { Angle = new Angles( 5.08f, -2.89f, -25.082f ), Pos = new Vector3( -49.547f, 0f, 2.885f ) };
		ZoomAnimData = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( -32.063f, -11f, 8.161f ) };
		ViewModelOffset = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -30f, 0f ) };
	}

	public override string ViewModelPath  => "models/weapons/pistols/magnum/vm_magnum.vmdl";
	public override string WorldModelPath => "models/weapons/pistols/magnum/wm_magnum.vmdl";
	public override AngPos   ViewModelOffset { get; }
}
