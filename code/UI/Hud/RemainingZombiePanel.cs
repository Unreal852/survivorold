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

	//TODO: Should be the number of remaining zombie instead of "0"
	public override void Tick()
	{
		_label.Text = "Remaining Zombie: " + "0";
	}
}
