using Sandbox.UI.Construct;

namespace Sandbox.UI.World;

public class HoverPlayerNameLabel : Panel
{
	private readonly Label _label;

	public HoverPlayerNameLabel( string playerName )
	{
		_label = Add.Label( playerName, "value" );
	}
}
