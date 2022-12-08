using Sandbox;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_ak47", Title = "AK47" )]
public sealed class WeaponAK47 : AbstractWeapon
{
	public WeaponAK47() : base( "ak47" )
	{
		if ( Asset == null )
			return;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.2f, Rotation = 0.1f }; // TODO: Move this into WeaponAsset
		RunAnimData = new() { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.184f, 0f, 8.476f ) };
		ZoomAnimData = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( -14.635f, -5f, 7.812f ) };
		ViewModelOffset = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -5f, 0f ) };
	}

	public override string ViewModelPath  => "models/weapons/assault_rifles/ak47/vm_ak47.vmdl";
	public override string WorldModelPath => "models/weapons/assault_rifles/ak47/wm_ak47.vmdl";
	public override AngPos   ViewModelOffset { get; }
}
