using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Survivor.Players;

namespace Survivor.UI.Hud;

public class MoneyLabel : Label
{
	private readonly Label _label;

	public MoneyLabel()
	{
		_label = Add.Label( "0 $", "value" );
	}

	//TODO: Should be the number of the current money instead of "0": 
	public override void Tick()
	{
		if ( Local.Pawn is not SurvivorPlayer player )
			return;
		_label.Text = $"{player.Money} $";
	}
}
