using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class LobbyHud : Panel
{
	private readonly Label _messageLabel;

	public LobbyHud()
	{
		StyleSheet.Load( "UI/Hud/LobbyHud.scss" );

		_messageLabel = Add.Label( "", "message" );
	}

	public override void Tick()
	{
		if ( SurvivorGame.GAME_MODE == null )
			return;
		_messageLabel.Text = "Waiting for players";
	}
}
