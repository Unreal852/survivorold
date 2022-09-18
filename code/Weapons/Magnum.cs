using Sandbox;
using Survivor.Weapons.Bullets;
using SWB_Base;

namespace Survivor.Weapons;

[Library( "survivor_magnum", Title = "Magnum" )]
public sealed class Magnum : BaseWeapon
{
	public Magnum()
	{
		if ( !InitializeWeapon( "magnum" ) )
			return;
		ViewModelPath = Asset.ViewModel;
		WorldModelPath = Asset.WorldModel;
		Primary.ScreenShake = new ScreenShake { Length = 0.08f, Delay = 0.02f, Size = 1.9f, Rotation = 0.4f };

		RunAnimData = new AngPos { Angle = new Angles( 27.7f, 39.95f, 0f ), Pos = new Vector3( 6.955f, -28.402f, 2.965f ) };
		DuckAnimData = new AngPos { Angle = new Angles( 5.08f, -2.89f, -25.082f ), Pos = new Vector3( -49.547f, 0f, 2.885f ) };
		ZoomAnimData = new AngPos { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( -32.063f, -11f, 8.161f ) };
	}
	
	public override string   ViewModelPath   { get; }
	public override string   WorldModelPath  { get; }
	public override HoldType HoldType        { get; } = HoldType.Pistol;
	public override AngPos   ViewModelOffset { get; } = new() { Angle = new Angles( 0f, 0f, 0f ), Pos = new Vector3( 0f, -30f, 0f ) };
}
