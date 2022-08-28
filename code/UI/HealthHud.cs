using Sandbox.UI.Construct;

namespace Sandbox.UI;

public class HealthHud : Panel
{
	private Label _label;

	public HealthHud()
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
