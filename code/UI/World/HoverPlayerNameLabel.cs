using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.World;

public class HoverPlayerNameLabel : Panel
{
	public HoverPlayerNameLabel( string playerName )
	{
		Add.Label( playerName, "value" );
	}
}
