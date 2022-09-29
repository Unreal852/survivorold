using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class GameInfosHud : Panel
{
	private readonly Label _currentWaveLabel;
	private readonly Label _enemiesRemainingLabel;

	public GameInfosHud()
	{
		StyleSheet.Load( "UI/Hud/GameInfosHud.scss" );

		_currentWaveLabel = Add.Label( "Wave 0", "wave" );
		_enemiesRemainingLabel = Add.Label( "Enemies 0", "enemiesRemaining" );
	}

	public override void Tick()
	{
		if ( SurvivorGame.GAME_MODE == null )
			return;
		_currentWaveLabel.Text = $"Wave {SurvivorGame.GAME_MODE.CurrentWave}";
		_enemiesRemainingLabel.Text = $"Enemies {SurvivorGame.GAME_MODE.EnemiesRemaining}";
	}
}
