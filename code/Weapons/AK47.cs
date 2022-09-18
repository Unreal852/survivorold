using Sandbox;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_ak47", Title = "AK47" )]
public sealed class AK47 : ABaseWeapon
{
	public AK47() : base( "ak47" )
	{
		if ( Asset == null )
			return;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.2f, Rotation = 0.1f }; // TODO: Move this into WeaponAsset
		RunAnimData = new() { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.184f, 0f, 8.476f ) };
		ZoomAnimData = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( -14.635f, -5f, 7.812f ) };
		ViewModelOffset = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -5f, 0f ) };
	}

	public override HoldType HoldType        { get; } = HoldType.Rifle; // TODO: Move this into WeaponAsset
	public override AngPos   ViewModelOffset { get; }
}
