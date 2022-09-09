using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Players;

namespace Survivor.UI.Hud;

public class HealthLabel : Panel
{
	private readonly Label _label;

	public HealthLabel()
	{
		_label = Add.Label( "100", "value" );
	}

	public override void Tick()
	{
		if ( Local.Pawn is not SurvivorPlayer player )
			return;
		_label.Text = $"♥ {player.Health.CeilToInt()}";
	}
}
