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
		RootPanel.AddChild<RemainingZombiePanel>();
		RootPanel.AddChild<CurrentMoneyLabel>();
		//RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<SurvivorScoreboard<SurvivorScoreboardEntry>>();
		// RootPanel.AddChild<CurrentWaveLabel>();
		// RootPanel.AddChild<CurrentAmmoLabel>();
		// RootPanel.AddChild<CurrentGrenadeLabel>();
		// RootPanel.AddChild<ScoreboardUI>();
	}
}
