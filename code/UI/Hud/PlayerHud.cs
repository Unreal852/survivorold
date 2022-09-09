using Sandbox;
using Sandbox.UI;

namespace Survivor.UI.Hud;

public class PlayerHud : HudEntity<RootPanel>
{
	public PlayerHud()
	{
		if ( !IsClient )
			return;
		RootPanel.StyleSheet.Load( "Resources/UI/MainPlayerHud.scss" );
		RootPanel.AddChild<HealthLabel>();
		RootPanel.AddChild<RemainingZombieLabel>();
		RootPanel.AddChild<MoneyLabel>();
		RootPanel.AddChild<WaveLabel>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<SurvivorScoreboard<SurvivorScoreboardEntry>>();
	}
}
