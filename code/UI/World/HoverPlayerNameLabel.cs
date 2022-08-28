using Sandbox.UI.Construct;

namespace Sandbox.UI.World;

public class HoverPlayerNameLabel : Panel
{
	public HoverPlayerNameLabel( string playerName )
	{
		Add.Label( playerName, "value" );
	}
}
