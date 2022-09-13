using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Players;
using Survivor.Utils;

namespace Survivor.UI.World;

public class InteractableIndicator : WorldPanel
{
	private Label          _label;
	private SurvivorPlayer _player;

	public InteractableIndicator( SurvivorPlayer player )
	{
		StyleSheet.Load( "UI/World/InteractableIndicator.scss" );
		_label = Add.Label( "Press E to interact", "title" );
		_player = player;
		SceneObject.Flags.ViewModelLayer = true;
	}

	public override void Tick()
	{
		if ( _player.Using == null )
			return;
		var pos = _player.EyePosition + CurrentView.Rotation.Forward * InchesUtils.FromMeters( 1f );
		Position = pos.WithZ( pos.z - 3.5f );
		Rotation = Rotation.LookAt( -CurrentView.Rotation.Forward );
	}
}
