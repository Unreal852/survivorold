using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class CurrentMoneyLabel : Label
{
	private Label _label;

	public CurrentMoneyLabel()
	{
		_label = Add.Label( "0$", "value" );
	}

	//TODO: Should be the number of the current money instead of "0": 
	public override void Tick()
	{
		_label.Text = "118" + "$";
	}
}
