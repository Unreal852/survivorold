using Sandbox;
using Survivor.Weapons.Bullets;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_ak47", Title = "AK47" )]
public sealed class AK47 : BaseWeapon
{
	public AK47()
	{
		if ( !InitializeWeapon( "ak47" ) )
			return;
		ViewModelPath = Asset.ViewModel;
		WorldModelPath = Asset.WorldModel;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.2f, Rotation = 0.1f };
		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.184f, 0f, 8.476f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( -14.635f, -5f, 7.812f ) };
	}

	public override string   ViewModelPath   { get; }
	public override string   WorldModelPath  { get; }
	public override HoldType HoldType        { get; } = HoldType.Rifle;
	public override AngPos   ViewModelOffset { get; } = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -5f, 0f ) };
}
