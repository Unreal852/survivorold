using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class LobbyHud : Panel
{
	private readonly Label _primaryMessageLabel;
	private readonly Label _secondaryMessageLabel;

	public LobbyHud()
	{
		StyleSheet.Load( "UI/Hud/LobbyHud.scss" );

		_primaryMessageLabel = Add.Label( "Lobby", "primaryMessage" );
		_secondaryMessageLabel = Add.Label( "Waiting for players...", "secondaryMessage" );
	}

	public override void Tick()
	{
		if ( SurvivorGame.GAME_MODE == null )
			return;

		if ( SurvivorGame.GAME_MODE.State == GameState.Starting )
		{
			_secondaryMessageLabel.Text = $"The game will start in {SurvivorGame.GAME_MODE.Counter} second(s)";
			return;
		}

		int missingPlayers = Client.All.Count - SurvivorGame.VarMinimumPlayers;
		if ( missingPlayers > 0 )
			_secondaryMessageLabel.Text = $"Waiting for {missingPlayers} player(s)...";
	}
}
