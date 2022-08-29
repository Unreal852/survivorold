using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.Hud;

public class PlayerNameLabel : Panel
{
	private readonly Label _label;

	public PlayerNameLabel()
	{
		_label = Add.Label( Local.DisplayName, "value" );
	}

	public override void Tick()
	{
		var player = Local.Pawn;
		if ( player == null ) return;
		_label.Text = $"{Local.DisplayName}, {Local.Client.Ping}ms ";
	}
}
