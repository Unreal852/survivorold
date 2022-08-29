using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class CurrentGrenadeLabel : Label
{
	private Label _label;

	public CurrentGrenadeLabel()
	{
		_label = Add.Label( "0", "value" );
	}

	//TODO: Should have a picture of a grenade at the right of the current value
	public override void Tick()
	{
		_label.Text = "2" + " ○";
	}
}
