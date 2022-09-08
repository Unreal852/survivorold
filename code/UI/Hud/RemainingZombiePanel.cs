using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class RemainingZombiePanel : Panel
{
	private Label _label;

	public RemainingZombiePanel()
	{
		_label = Add.Label( "0", "value" );
	}

	public override void Tick()
	{
		_label.Text = $"☻: {SurvivorGame.Current.GameMode?.EnemiesRemaining ?? 0}";
	}
}
