using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class CurrentWaveLabel : Panel
{
	private Label _label;

	public CurrentWaveLabel()
	{
		_label = Add.Label( "0", "value" );
	}

	//TODO: Should be the number of the current wave instead of "0"
	public override void Tick()
	{
		_label.Text = "Wave: " + "6";
	}
}
