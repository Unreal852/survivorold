using System;
using Sandbox.UI.Construct;

namespace Sandbox.UI;

public class NameHud : Panel
{
	private Label _label;

	public NameHud()
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
