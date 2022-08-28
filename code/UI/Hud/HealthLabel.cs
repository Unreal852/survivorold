using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class HealthLabel : Panel
{
	private Label _label;

	public HealthLabel()
	{
		_label = Add.Label( "100", "value" );
	}

	public override void Tick()
	{
		var player = Local.Pawn;
		if ( player == null ) return;

		_label.Text = $"{player.Health.CeilToInt()}";
	}
}
