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
		var gameMode = SurvivorGame.GAME_MODE;
		if ( gameMode == null )
			return;
		
		switch ( gameMode.State )
		{
			case GameState.Starting:
				_secondaryMessageLabel.Text = $"The game will start in {MathX.FloorToInt(gameMode.Until)} second(s)";
				return;
			case GameState.Lobby:
				int missingPlayers = Game.Clients.Count - SurvivorGame.VarMinimumPlayers;
				_secondaryMessageLabel.Text = $"Waiting for {missingPlayers} player(s)...";
				break;
		}
	}
}
