using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.World;

public partial class MysteryBoxTimer : WorldPanel
{
	private static MysteryBoxTimer _instance;

	private TimeSince _sinceSpawned;
	private Label     _label;
	private float     _deleteAfter;

	public MysteryBoxTimer( float deleteAfter )
	{
		StyleSheet.Load( "UI/World/MysteryBoxTimer.scss" );

		_label = Add.Label( deleteAfter.CeilToInt().ToString(), "value" );
		_deleteAfter = deleteAfter;
		_sinceSpawned = 0;
	}

	public override void Tick()
	{
		_label.Text = $"{(_deleteAfter - _sinceSpawned).CeilToInt()}";
		if ( _sinceSpawned >= _deleteAfter )
		{
			_label.Delete( true );
			Delete( true );
		}
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
