using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Players;

namespace Survivor.UI.Hud;

public class PlayerInteractablePanel : Panel
{
	private readonly Label _label;

	public PlayerInteractablePanel()
	{
		StyleSheet.Load( "UI/Hud/PlayerInteractablePanel.scss" );

		_label = Add.Label( "", "value" );
	}

	public override void Tick()
	{
		if ( Local.Pawn is not SurvivorPlayer player )
			return;
		if ( !IsVisible )
			return;
		_label.Text = player.Using == null ? string.Empty : "Press E to interact";
	}
}
