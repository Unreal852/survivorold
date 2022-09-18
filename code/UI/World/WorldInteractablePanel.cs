using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Survivor.UI.World;

public partial class WorldInteractablePanel : WorldPanel
{
	public WorldInteractablePanel()
	{
		StyleSheet.Load( "UI/World/WorldInteractablePanel.scss" );

		var label = Add.Label( "Test panel", "title" );
	}
}
