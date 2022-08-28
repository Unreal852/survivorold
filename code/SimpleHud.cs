using Sandbox.UI;

namespace Sandbox;

public class SimpleHud : HudEntity<RootPanel>
{
	public SimpleHud()
	{
		if ( !IsClient )
			return;
		RootPanel.StyleSheet.Load( "/UI/SimpleHud.scss" );
		RootPanel.AddChild<HealthHud>();
		RootPanel.AddChild<NameHud>();
	}
}
