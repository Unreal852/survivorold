using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class WaveLabel : Panel
{
	private readonly Label _label;

	public WaveLabel()
	{
		_label = Add.Label( "0", "value" );
	}

	public override void Tick()
	{
		_label.Text = $"Wave {SurvivorGame.GAME_MODE?.CurrentWave ?? 0}";
	}
}
