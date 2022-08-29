using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class CurrentAmmoLabel : Label
{
	private Label _label;

	public CurrentAmmoLabel()
	{
		_label = Add.Label( "0/0", "value" );
	}

	//TODO: Should be the number of the current munitions and the max munition of the current weapon instead of "32/36"
	public override void Tick()
	{
		_label.Text = "32" + "/" + "36" + " ///";
	}
}
