using Sandbox;
using Sandbox.UI;

namespace Survivor.UI.Hud;

public class MainPlayerHud : HudEntity<RootPanel>
{
	public MainPlayerHud()
	{
		if ( !IsClient )
			return;
		RootPanel.StyleSheet.Load( "Resources/UI/MainPlayerHud.scss" );
		RootPanel.AddChild<HealthLabel>();
		RootPanel.AddChild<PlayerNameLabel>();
		RootPanel.AddChild<RemainingZombiePanel>();
		RootPanel.AddChild<CurrentWaveLabel>();
		RootPanel.AddChild<CurrentAmmoLabel>();
		RootPanel.AddChild<CurrentGrenadeLabel>();
		RootPanel.AddChild<CurrentMoneyLabel>();
	}
}
