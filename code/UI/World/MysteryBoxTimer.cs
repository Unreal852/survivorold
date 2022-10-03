using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.World;

public partial class MysteryBoxTimer : WorldPanel
{
	private static   MysteryBoxTimer _instance;
	private readonly Label           _label;
	private readonly TimeUntil       _untilDelete;

	private MysteryBoxTimer( float deleteAfter )
	{
		StyleSheet.Load( "UI/World/MysteryBoxTimer.scss" );

		_label = Add.Label( deleteAfter.CeilToInt().ToString(), "value" );
		_untilDelete = deleteAfter;
	}

	public override void Tick()
	{
		_label.Text = $"{MathX.FloorToInt( _untilDelete )}";
		if ( _untilDelete )
			Delete( true );
	}

	[ClientRpc]
	public static void SpawnMysteryBoxTimerClient( Vector3 position, Rotation rotation, float deleteAfter )
	{
		if ( _instance is { IsValid: true } )
			_instance.Delete();
		_instance = new MysteryBoxTimer( deleteAfter ) { Position = position, Rotation = rotation };
	}

	[ClientRpc]
	public static void DeleteMysteryBoxTimerClient()
	{
		if ( _instance is { IsValid: true } )
			_instance.Delete( true );
	}
}
