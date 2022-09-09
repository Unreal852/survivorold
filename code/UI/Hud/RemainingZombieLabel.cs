using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class RemainingZombieLabel : Panel
{
	private readonly Label _label;

	public RemainingZombieLabel()
	{
		_label = Add.Label( "0", "value" );
	}

	public override void Tick()
	{
		_label.Text = $"☻ {SurvivorGame.GAME_MODE?.EnemiesRemaining ?? 0}";
	}
}
