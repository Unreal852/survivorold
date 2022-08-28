using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Hud;

namespace Survivor.UI.Hud;

public class MainPlayerHud : HudEntity<RootPanel>
{
	public MainPlayerHud()
	{
		if ( !IsClient )
			return;
		RootPanel.StyleSheet.Load( "/UI/Hud/MainPlayerHud.scss" );
		RootPanel.AddChild<HealthLabel>();
		RootPanel.AddChild<PlayerNameLabel>();
	}
}
